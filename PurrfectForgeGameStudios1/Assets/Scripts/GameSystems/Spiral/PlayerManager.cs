using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
//using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour, PDamage, MDamage, HealHit
{
    #region Fields/Objects

    [SerializeField] CharacterController characterControl;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject currentWeapon;
    [SerializeField] GameObject currentShield;
    [SerializeField] Animator Anim;

    [SerializeField] public Camera OverHeadCamera;
    [SerializeField] public Camera FPCamera;
    public bool FPActive;

    public static PlayerManager Instance;
    [SerializeField] public Transform castPos;
    [SerializeField] public Transform interactionCastPos;
    [SerializeField] public Transform interactionCastGroundPos;
    //player info
    public int highestFloorCompleted = 0;
    public int playerLevel = 1;
    public int playerCoin;
    int playerLevelMax = 50;
    public float HP;
    public float HPOriginal = 15;
    public float Attack;
    public float AttackOriginal = 5;
    public float Def;
    public float DefOriginal = 1;
    public float Dex;
    public float DexOriginal = 1;
    public float MoveSpeed;
    private float MoveSpeedOriginal = 4;
    public float Stamina;
    public float StaminaOriginal = 5;
    public float dashMult;
    public bool dashing;
    public int jumpCounter;
    public float jumpSpeed;
    public int maxJumps;
    public bool shieldUp;
    public float playerXP = 0;
    public float playerXPReset = 0;
    public int maxBoost;
    private float panSpeed = 6F;
    private float gravity = 20;
    private Vector3 playerVelocity;
    private Vector3 moveDirection;
    private Vector3 playerPOS;
    public float playerShieldMod;
    private Coroutine staminaDrainCoroutine;
    private Coroutine staminaRefillCoroutine;
    public bool HasFloorKey;
    public bool HasBossKey;
    bool Healing = false;
    Vector3 previousPosition;
    Vector3 currentPosition;
    float deltaTime;
    bool swordDrawn;
    bool jumping;
    float currentSpeed;
    //skill systems

    public string chosenElement;
    public int playerSkillPoints = 0;
    //which one of the either or skills are selecter 1 or 2, left or right
    public int tierOne = 0;
    public int tierTwo = 0;
    public int tierThree = 0;

    //only skill tiers one and two can be leveled up
    public int skillOneLevel = 0;
    public int skillTwoLevel = 0;

    //if the player has unlocked skill tier 2
    public bool tierTwoUnlocked = false;
    public bool tierThreeUnlocked = false;

    public ScriptableSkill activeSlotOneSkill;
    public ScriptableSkill activeSlotTwoSkill;
    public ScriptableSkill activeSlotThreeSkill;

    [SerializeField] public List<ScriptableSkill> skillPool;


    #endregion

    #region Processes
    void Awake()
    {
        AwakenProcesses(); //all processes run in Awaken
    }
    void Start()
    {
        swordDrawn = false;
        StartUpProcesses(); //start up processes
    }
    void Update()
    {
        if (currentSpeed < 0.01f)
        {
            Anim.SetBool("Sprinting", false);
        }

        UpdateProcesses(); //update processes
    }
    #endregion

    #region Movement Systems

    public void Turn() //turns First person player left right
    {
        float x = panSpeed * Input.GetAxis("Mouse X"); //grabs left right on mouse
        transform.Rotate(0, x, 0); //rotates player
    }
    public void Walk() //walks player
    {
        if (characterControl.isGrounded) //if the are on the ground
        {
            jumpCounter = 0; //set jump to 0
            jumping = false;
            //playerVelocity = Vector3.zero; //stop their velocity
        }
        if (!characterControl.isGrounded) //if the are on the ground
        {
            jumping = true;
            //playerVelocity = Vector3.zero; //stop their velocity
        }

        moveDirection = (Input.GetAxis("Horizontal") * transform.right) +
            (Input.GetAxis("Vertical") * transform.forward); //set the movedirection
        characterControl.Move(moveDirection * MoveSpeed * Time.deltaTime); //move the player that direction

        currentSpeed = moveDirection.magnitude;

        Anim.SetFloat("Speed", currentSpeed);
    }
    public void Dash() //multiplies movespeed
    {
        if (Input.GetButtonDown("Dash") && !dashing && Stamina > 0) //if left shift is pressed and currently not dashing and enough stamina
        {
            dashing = true; //set dashing to true
            if (currentSpeed > 0)
            {
                Anim.SetBool("Sprinting", true);
                MoveSpeed *= dashMult; //apply mult to movespeed

                if (staminaDrainCoroutine != null) //is stamina drain corountine is not ended
                {
                    StopCoroutine(staminaDrainCoroutine); // Stop draining stamina
                }
                staminaDrainCoroutine = StartCoroutine(StaminaDrain()); //start stamina drain
                if (staminaRefillCoroutine != null) //if refil is not ended
                {
                    StopCoroutine(staminaRefillCoroutine); //stop refill
                    staminaRefillCoroutine = null; //set stamina refill to null for next refill needed
                }
            }
        }

        if (Input.GetButtonUp("Dash") /*&& dashing*/) //if im dashing and dashing is true
        {
            dashing = false; //set dashing false
            Anim.SetBool("Sprinting", false);
            MoveSpeed = MoveSpeedOriginal; //make movespeed normal

            StamSystem();
        }
    }

    void ChangeView() //view changer
    {
        if (Input.GetKeyDown(KeyCode.V)) //on V key
        {
            if (OverHeadCamera.isActiveAndEnabled) //Overhead cam is on
            {
                OverHeadCamera.enabled = false; //turn it off
                FPCamera.enabled = true; //turn on First person
                FPActive = true; //set FirstPerson bool
            }
            else //First person is on
            {
                OverHeadCamera.enabled = true; //turn on OverHead
                FPCamera.enabled = false; //turn off First person
                FPActive = false; //First person flag false
            }

        }
    }
    public void Jump() //to jump
    {
        if (Input.GetButtonDown("Jump") && jumpCounter < maxJumps && !jumping) //on space bar and jump counter is under max jumps allowed
        {
            jumping = true;
            StartCoroutine(JumpTime());
            jumpCounter++; //increase jump counter
            Stamina -= 1; //use stamina
            if (staminaDrainCoroutine != null) //is stamina drain corountine is not ended
            {
                StopCoroutine(staminaDrainCoroutine); // Stop draining stamina
            }
            staminaDrainCoroutine = StartCoroutine(StaminaDrain()); //start stamina drain
            //if (staminaRefillCoroutine != null) //if refil is not ended
            //{
            //    StopCoroutine(staminaRefillCoroutine); //stop refill
            //    staminaRefillCoroutine = null; //set stamina refill to null for next refill needed
            //}
        }
        playerVelocity.y -= gravity * Time.deltaTime; //this applies at ALL TIMES gravity downwards
        characterControl.Move(playerVelocity * Time.deltaTime); //move player downward if able

    }
    IEnumerator JumpTime()
    {
        Anim.SetTrigger("Jump");
        yield return new WaitForSeconds(.5f);
        playerVelocity.y = jumpSpeed;//add upwardsd velocity to player

    }
    #endregion

    #region Combat / Health Systems

    public void Melee()//swing a weapon
    {
        if (currentWeapon != null && !shieldUp) //if there is a weapon and shield down
        {
            if (Input.GetButtonDown("Left Mouse") && !swordDrawn) //left mouse click
            {
                Anim.SetTrigger("DrawSword");
                StartCoroutine(SwordSpawnTime());

                swordDrawn = true;
            }
            else if (Input.GetButtonDown("Left Mouse") && swordDrawn) //left mouse click
            {
                Anim.SetTrigger("Swing");
                StartCoroutine(Swing()); //start a swing
                Anim.SetBool("Swinging", false);
            }
        }
    }
    IEnumerator SwordSpawnTime()
    {
        yield return new WaitForSeconds(.1f);
        Anim.SetBool("SwordDrawn", true);
        currentWeapon.SetActive(true); //turn on weapon
    }
    public void Defend() //pu shield up
    {
        if (currentShield != null) //if shield on
        {
            if (Input.GetButtonDown("Right Mouse")) //on click
            {
                Anim.SetBool("ShieldUp", true);
                currentShield.SetActive(true); //turn on shield
                shieldUp = true; //shield flag true
                playerShieldMod = .5F; //shield mod TO BE PULLED FROM SHIELD EVENTUALLY
            }
            if (Input.GetButtonUp("Right Mouse")) //on mouse up
            {
                currentShield.SetActive(false); //turn shield off
                shieldUp = false; //shield flag off
                playerShieldMod = 0; //turn defense mod off 
                Anim.SetBool("ShieldUp", false);
            }
        }
    }

    public void UseSkills()
    {
        if (activeSlotOneSkill != null && !shieldUp)
        {
            if (Input.GetButtonDown("Skill1") && activeSlotOneSkill != null && Stamina - activeSlotOneSkill.SkillCost >= 0)
            {
                Anim.SetTrigger("Cast");
                Debug.Log("Skill One Used");
                Debug.Log(activeSlotOneSkill);
                //logic to use skill
                activeSlotOneSkill.SkillBehavior(castPos);
                Stamina -= activeSlotOneSkill.SkillCost * activeSlotOneSkill.SkillLevel;
                StamSystem();
            }
            Anim.SetBool("Casting", false);
        }
        if (activeSlotTwoSkill != null && !shieldUp)
        {
            if (Input.GetButtonDown("Skill2") && activeSlotTwoSkill != null && Stamina - activeSlotTwoSkill.SkillCost >= 0)
            {
                Debug.Log("Skill Two Used");
                Debug.Log(activeSlotTwoSkill);
                //logic to use skill
                activeSlotOneSkill.SkillBehavior(castPos);
                Stamina -= activeSlotTwoSkill.SkillCost * activeSlotTwoSkill.SkillLevel;
                StamSystem();
            }
        }
        if (activeSlotThreeSkill != null && !shieldUp)
        {
            if (Input.GetButtonDown("Skill3") && activeSlotThreeSkill != null && Stamina - activeSlotThreeSkill.SkillCost >= 0)
            {
                Debug.Log("Skill Three Used");
                Debug.Log(activeSlotThreeSkill);
                //logic to use skill
                activeSlotOneSkill.SkillBehavior(castPos);
                Stamina -= activeSlotThreeSkill.SkillCost * activeSlotThreeSkill.SkillLevel;
                StamSystem();
            }
        }
    }
    IEnumerator CastTime()
    {
        yield return new WaitForSeconds(1);
    }
    public void DeathCheck() //check to see if player is dead
    {
        if (HP <= 0) //if hp <= 0 
        {
            HP = 0;
            StartCoroutine(DeathDramatics());
        }
        else
        {
            gameManager.youAlive();
        }

    }
    IEnumerator DeathDramatics()
    {
        Anim.SetTrigger("IsDead");
        yield return new WaitForSeconds(4);
        UpdatePlayerUI(); //show it on UI
        gameManager.youDead(); //throw death flags
    }
    public void takeHeal() //takes Heal from the HealHit script
    {
        if (!Healing) //if player not healing
        {
            StartCoroutine(Healer()); //Start a heal 
        }
    }
    IEnumerator Healer() //heal routine
    {
        Healing = true;//flip the heal flag to prevent mult heals
        if (HP + 1 >= HPOriginal) //if hp healed would be over Max
        {
            HP = HPOriginal; //hp is max
            StopCoroutine(Healer()); //stop healing
        }
        else //if player under max
        {
            HP += 1;//add one
        }
        yield return new WaitForSeconds(1F); //one second between heals
        Healing = false; //flip the flag to allow next heal
    }
    public void takeDamage(float damage) //takeDamage from the PDamage(Physical DAmage from enemy to player) script
    {
        if (shieldUp)//if player blocking
        {
            float damageTaken = DamageShieldUPCalc(damage); //damage reduction called by calc function
            HP -= damageTaken; //take health from player
            Stamina -= .1F; //stamina tick for blacking
            StamSystem();
            StartCoroutine(HitFlash()); //start hit flash from being hit in UI
            DeathCheck(); //check if this killed player
        }
        else //if player isnt blocking
        {
            float damageTaken = DamageShieldDownCalc(damage); //damage calc
            HP -= damageTaken; //take hp after calc
            StartCoroutine(HitFlash()); //ouch flash in UI
            DeathCheck(); //is player dead
        }
    }
    public void takeMDamage(float damage) //take magiv damage from enemy to player
    {
        if (shieldUp) //blocking
        {
            float damageTaken = MDamageShieldUPCalc(damage); //magic damage with shield calc
            HP -= damageTaken; //take hp
            Stamina -= .5F; //take stamina
            StamSystem();
            StartCoroutine(HitFlash()); //ouch flash
            DeathCheck();//did player die
        }
        else //not blocking
        {
            float damageTaken = DamageShieldDownCalc(damage); //noraml calc EVENTUALLY WILL BE MDEF
            HP -= damageTaken; //take hp
            StartCoroutine(HitFlash()); //ouch flash in ui
            DeathCheck(); //did player die
        }
    }
    IEnumerator HitFlash() //when player is hit flash in UI
    {
        if (HP > 0) //if there is HP to take
        {
            gameManager.playerHitFlash.SetActive(true); //turn on flash
            yield return new WaitForSeconds(.1F); //wait
            gameManager.playerHitFlash.SetActive(false); //turn off flash
        }
    }
    IEnumerator Swing() //swing
    {
        if (Stamina >= SwingCostCalc()) //is enough stamina
        {
            Anim.SetBool("Swing", true);
            float swingCost = SwingCostCalc(); //calc swing cost
            Stamina -= swingCost; //take stamina
            yield return new WaitForSeconds(0.2F);//wait
            if (staminaRefillCoroutine != null) //if refilling
            {
                StopCoroutine(staminaRefillCoroutine); //stop refill
                staminaRefillCoroutine = null; //refill null
            }
            if (!dashing) //if im not dashing to prevent bugs in stamina not refilling
            {
                staminaRefillCoroutine = StartCoroutine(StaminaRefill()); //start the refill
            }
        }
    }

    public float MDamageShieldUPCalc(float damage) //calc magic damage
    {
        float damageOut; //holder
        float damageReduction = playerShieldMod / 3; //reduce damage on shield
        damageOut = damage - damageReduction; //calc damage
        return damageOut; //send damage
    }
    public float DamageShieldUPCalc(float damage) //calc normal damage
    {
        float damageOut; //holder
        float damageReduction = playerShieldMod / 2; //reduced damage calc
        damageOut = damage - damageReduction; //calc damage
        return damageOut; //send damage
    }
    public float DamageShieldDownCalc(float damage) //no shield up damage
    {
        float damageOut; //holder
        float damageReduction = Def * .025F; //def mod
        damageOut = damage - damageReduction; //calc damage
        return damageOut; //send damage
    }
    public float SwingCostCalc()//calc swing cost
    {
        float costMod = Dex * .0025F; //cost reducer
        float baseCost = 3F - costMod; //cost minus reducer
        return baseCost; //return cost
    }

    #endregion

    #region UI Systems

    public void UpdatePlayerUI() //updates everything on UI
    {

        float hpFillAmount = HP / HPOriginal; //HP updater
        int hpPercentage = (int)(hpFillAmount * 100); //convert hpFillAmount to a percentage for display
        gameManager.playerHP.fillAmount = hpFillAmount; //set fillamount
        gameManager.playerHPText.text = (hpPercentage.ToString() + "%"); //assign the text 


        float xpFillAmount = playerXP / (100 * playerLevel); //XP updater
        int xpPercentage = (int)(xpFillAmount * 100); //xp percentage
        gameManager.playerXPBar.fillAmount = xpFillAmount; //set fill


        float stamFillAmount = Stamina / StaminaOriginal;//StaminaUpdater
        int stamPercentage = (int)(stamFillAmount * 100);//stam precent
        gameManager.playerStamBar.fillAmount = stamFillAmount; //set fill amount

        playerCoin = gameManager.coinPurse.amountHeld; //set player coin amount

        gameManager.playerLvLText.text = playerLevel.ToString(); //set player level 
        gameManager.playerHPStat.text = HPOriginal.ToString(); // set HP stat
        gameManager.playerAttStat.text = AttackOriginal.ToString(); // set Attack stat
        gameManager.playerDefStat.text = DefOriginal.ToString(); //set def stat
        gameManager.playerDexStat.text = DexOriginal.ToString(); //set dex stat
        gameManager.playerStamStat.text = StaminaOriginal.ToString(); //set stam stat
        gameManager.playerCoins.text = playerCoin.ToString(); //set coin
        gameManager.playerAvailableSkillPts.text = playerSkillPoints.ToString();
    }
    private void playerLevelUp() //level up process
    {
        if (playerXP >= (100 * playerLevel) && playerLevel < playerLevelMax) //if player has less than needed xp and not max level
        {
            //show the level up screen
            if (playerLevel % 8 == 0)
            {
                playerSkillPoints += 1;
            }
            int randBoostHP = UnityEngine.Random.Range(1, 10); //increase stat by random number 1-10
            int randBoostAttack = UnityEngine.Random.Range(1, 5); //increase stat by random number 1-5
            int randBoostDef = UnityEngine.Random.Range(1, 3); //increase stat by random number 1-3
            float randBoostMoveSpeed = UnityEngine.Random.Range(.01F, .05F); //increase stat by random number by small amount
            int randBoostStamina = UnityEngine.Random.Range(1, 4); //increase stat by random number 1-4
            int randBoostDex = UnityEngine.Random.Range(1, 3); //increase stat by random number 1-3

            HP += randBoostHP; //apply random boost
            HPOriginal += randBoostHP;//apply random boost
            Attack += randBoostAttack;//apply random boost
            AttackOriginal += randBoostAttack;//apply random boost
            Def += randBoostDef;//apply random boost
            DefOriginal += randBoostDef;//apply random boost
            Dex += randBoostDex;//apply random boost
            DexOriginal += randBoostDex;//apply random boost
            MoveSpeed += randBoostMoveSpeed;//apply random boost
            MoveSpeedOriginal += randBoostMoveSpeed;//apply random boost
            Stamina += randBoostStamina;//apply random boost
            StaminaOriginal += randBoostStamina;//apply random boost
            playerLevel++; //increase player level
            playerXP = 0; //reset playerXp to zero
            UpdatePlayerUI(); //update the UI with new stats
        }
    }

    IEnumerator StaminaDrain() //Stamina drain 
    {
        while (dashing && Stamina > 0) //if dashing and stamina is above 0
        {
            yield return new WaitForSeconds(0.1F); // Adjust time to control the drain rate
            Stamina -= 0.1F; // Adjust the amount drained per second
            UpdatePlayerUI(); //update as we go
            if (Stamina <= 0) //if stamina is empty
            {
                Stamina = 0; //set it to empty
                dashing = false; //stop dashing
                MoveSpeed = MoveSpeedOriginal; // Reset speed if stamina is depleted

                // Stop the drain coroutine
                StamSystem();
                yield break; // Exit the coroutine as the player has no stamina left
            }
        }
    }

    IEnumerator StaminaRefill() //stamina refill
    {
        Debug.Log("Starting StaminaRefill coroutine.");

        while (Stamina < StaminaOriginal) // Continue refilling as long as stamina is below the maximum and not dashing
        {
            // Wait for a short period before refilling
            yield return new WaitForSeconds(0.1F); // Adjust this value to control the refill delay

            // Gradually refill stamina
            Stamina += 0.1F + (Dex * .005F); // Adjust this value for desired refill speed
            Stamina = Mathf.Clamp(Stamina, 0, StaminaOriginal); // Ensure stamina does not exceed the original amount
            UpdatePlayerUI(); //update UI
            Debug.Log($"Refilling stamina. Current stamina: {Stamina}");
        }
        Stamina = StaminaOriginal;// Ensure stamina is capped at maximum
        Debug.Log("Stamina refill complete.");
        staminaRefillCoroutine = null; // Reset coroutine reference
    }

    #endregion

    #region Organizational Systems
    private void StartUpProcesses() //all the provess on start
    {
        GetPlayerPrefs(); //grabs player save data
    }
    private void AwakenProcesses()
    {
        playerPOS = transform.position; //grabs player position
        Cursor.lockState = CursorLockMode.Locked; //Locks cursor
        Cursor.visible = false; //non visible cursor
        if (Instance == null) //checks for player
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); //extra player destoryed
        }
        OverHeadCamera.enabled = true; //turn on over head
        FPCamera.enabled = false; //turn off first person
        FPActive = false; //flip first person flag
    }
    private void UpdateProcesses() //everything for update
    {
        if (!gameManager.isPaused)
        {
            Melee(); //attack
            Defend(); //block
            Walk(); //move
            Turn(); //left right turns
            Jump(); //upwards velocity
            Dash(); //dash mult
            DeathCheck(); //checks for death
            playerLevelUp(); //checks for player level up
            UseSkills();
            ChangeView(); //checks to change view
            TierUnlockedCheck();
            SkillAssigner();
            UpdatePlayerUI(); //updates the ui constantly
            SkillLevelUp();
            CanInteract();
        }
    }
    public void SkillLevelUp()
    {
        if (activeSlotOneSkill != null)
        {
            activeSlotOneSkill.SkillLevel = skillOneLevel;
        }
        if (activeSlotTwoSkill != null)
        {
            activeSlotTwoSkill.SkillLevel = skillTwoLevel;
        }
    }
    public void SkillAssigner()
    {
        if (chosenElement == "Fire")
        {
            if (tierOne == 1)
            {
                activeSlotOneSkill = skillPool[0];
                AssignSkillImageOne(activeSlotOneSkill);
            }
            else if (tierOne == 2)
            {
                activeSlotOneSkill = skillPool[1];
                AssignSkillImageOne(activeSlotOneSkill);
            }
            if (tierTwo == 1)
            {
                activeSlotTwoSkill = skillPool[2];
                AssignSkillImageTwo(activeSlotTwoSkill);
            }
            if (tierThree == 1)
            {
                activeSlotThreeSkill = skillPool[3];
                AssignSkillImageThree(activeSlotThreeSkill);
            }
            else if (tierThree == 2)
            {
                activeSlotThreeSkill = skillPool[4];
                AssignSkillImageThree(activeSlotThreeSkill);
            }
        }
        if (chosenElement == "Ice")
        {
            if (tierOne == 1)
            {
                activeSlotOneSkill = skillPool[5];
                AssignSkillImageOne(activeSlotOneSkill);
            }
            else if (tierOne == 2)
            {
                activeSlotOneSkill = skillPool[6];
                AssignSkillImageOne(activeSlotOneSkill);
            }
            if (tierTwo == 1)
            {
                activeSlotTwoSkill = skillPool[7];
                AssignSkillImageTwo(activeSlotTwoSkill);
            }
            if (tierThree == 1)
            {
                activeSlotThreeSkill = skillPool[8];
                AssignSkillImageThree(activeSlotThreeSkill);
            }
            else if (tierThree == 2)
            {
                activeSlotThreeSkill = skillPool[9];
                AssignSkillImageThree(activeSlotThreeSkill);
            }
        }
        if (chosenElement == "Lightning")
        {
            if (tierOne == 1)
            {
                activeSlotOneSkill = skillPool[10];
                AssignSkillImageOne(activeSlotOneSkill);
            }
            else if (tierOne == 2)
            {
                activeSlotOneSkill = skillPool[11];
                AssignSkillImageOne(activeSlotOneSkill);
            }
            if (tierTwo == 1)
            {
                activeSlotTwoSkill = skillPool[12];
                AssignSkillImageTwo(activeSlotTwoSkill);
            }
            if (tierThree == 1)
            {
                activeSlotThreeSkill = skillPool[13];
                AssignSkillImageThree(activeSlotThreeSkill);
            }
            else if (tierThree == 2)
            {
                activeSlotThreeSkill = skillPool[14];
                AssignSkillImageThree(activeSlotThreeSkill);
            }
        }
    }
    public void AssignSkillImageOne(ScriptableSkill skill)
    {
        gameManager.AssignSkillImageOne(skill);
    }
    public void AssignSkillImageTwo(ScriptableSkill skill)
    {
        gameManager.AssignSkillImageTwo(skill);
    }
    public void AssignSkillImageThree(ScriptableSkill skill)
    {
        gameManager.AssignSkillImageThree(skill);
    }
    public void TierUnlockedCheck()
    {
        if (skillOneLevel == 3)
        {
            tierTwoUnlocked = true;
        }
        if (skillTwoLevel == 2)
        {
            tierThreeUnlocked = true;
        }
    }
    public void StamSystem()
    {
        if (staminaDrainCoroutine != null) //draing
        {
            StopCoroutine(staminaDrainCoroutine); //stop draining
            staminaDrainCoroutine = null; //drain null
        }
        if (staminaRefillCoroutine == null) //refilling null
        {
            staminaRefillCoroutine = StartCoroutine(StaminaRefill()); //start refill
        }
    }
    public void GetPlayerPrefs() //get player SPIRAL save data
    {
        HPOriginal = PlayerPrefs.GetFloat("HPOringal", HPOriginal); //get stat
        HP = HPOriginal; //set player stat
        AttackOriginal = PlayerPrefs.GetFloat("AttackOriginal", AttackOriginal);//get stat
        Attack = AttackOriginal;//set player stat
        DefOriginal = PlayerPrefs.GetFloat("DefOriginal", DefOriginal);//get stat
        Def = DefOriginal;//set player stat
        MoveSpeedOriginal = PlayerPrefs.GetFloat("MoveSpeedOriginal", MoveSpeedOriginal);//get stat
        MoveSpeed = MoveSpeedOriginal;//set player stat
        StaminaOriginal = PlayerPrefs.GetFloat("StaminaOriginal", StaminaOriginal);//get stat
        Stamina = StaminaOriginal;//set player stat
        DexOriginal = PlayerPrefs.GetFloat("DexOriginal", DexOriginal);//get stat
        Dex = DexOriginal;//set player stat
        playerXP = PlayerPrefs.GetFloat("playerXP", playerXP); //get xp
        playerLevel = PlayerPrefs.GetInt("playerLevel", playerLevel); //get level
        maxJumps = PlayerPrefs.GetInt("maxJumps", maxJumps); //get max jumps
        playerCoin = PlayerPrefs.GetInt("playerCoin", playerCoin); //get coins
        highestFloorCompleted = PlayerPrefs.GetInt("highestFloorCompleted", highestFloorCompleted);//save stat

        chosenElement = PlayerPrefs.GetString("ElementChosen", chosenElement);
        playerSkillPoints = PlayerPrefs.GetInt("PlayerSkillPoints", playerSkillPoints);
        tierOne = PlayerPrefs.GetInt("TierOne", tierOne);
        tierTwo = PlayerPrefs.GetInt("TierTwo", tierTwo);
        tierThree = PlayerPrefs.GetInt("TierThree", tierThree);
        skillOneLevel = PlayerPrefs.GetInt("SkillOneLevel", skillOneLevel);
        skillTwoLevel = PlayerPrefs.GetInt("SkillTwoLevel", skillTwoLevel);
    }
    public void SavePlayerPrefs() //save SPIRAL player data
    {
        PlayerPrefs.SetFloat("HPOringal", HPOriginal); //save stat
        PlayerPrefs.SetFloat("AttackOriginal", AttackOriginal);//save stat
        PlayerPrefs.SetFloat("DefOriginal", DefOriginal);//save stat
        PlayerPrefs.SetFloat("DexOriginal", DexOriginal);//save stat
        PlayerPrefs.SetFloat("MoveSpeedOriginal", MoveSpeedOriginal);//save stat
        PlayerPrefs.SetFloat("StaminaOriginal", StaminaOriginal);//save stat
        PlayerPrefs.SetFloat("playerXP", playerXP);//save stat
        PlayerPrefs.SetInt("playerLevel", playerLevel);//save stat
        PlayerPrefs.SetInt("maxJumps", maxJumps);//save stat
        PlayerPrefs.SetInt("playerCoin", playerCoin);//save stat
        PlayerPrefs.SetInt("highestFloorCompleted", highestFloorCompleted);//save stat

        PlayerPrefs.SetString("ElementChosen", chosenElement);
        PlayerPrefs.SetInt("PlayerSkillPoints", playerSkillPoints);
        PlayerPrefs.SetInt("TierOne", tierOne);
        PlayerPrefs.SetInt("TierTwo", tierTwo);
        PlayerPrefs.SetInt("TierThree", tierThree);
        PlayerPrefs.SetInt("SkillOneLevel", skillOneLevel);
        PlayerPrefs.SetInt("SkillTwoLevel", skillTwoLevel);

    }
    public void ResetSetPlayerPrefs() //resets for new game
    {
        PlayerPrefs.SetFloat("HPOringal", 15); //reset stat
        PlayerPrefs.SetFloat("AttackOriginal", 5);//reset stat
        PlayerPrefs.SetFloat("DefOriginal", 1);//reset stat
        PlayerPrefs.SetFloat("DexOriginal", 1);//reset stat
        PlayerPrefs.SetFloat("MoveSpeedOriginal", 4);//reset stat
        PlayerPrefs.SetFloat("StaminaOriginal", 5);//reset stat
        PlayerPrefs.SetFloat("playerXP", 0);//reset stat
        PlayerPrefs.SetInt("playerLevel", 1);//reset stat
        PlayerPrefs.SetInt("maxJumps", 1);//reset stat
        PlayerPrefs.SetInt("playerCoin", playerCoin);//reset stat
        PlayerPrefs.SetInt("highestFloorCompleted", 0);//save stat
        PlayerPrefs.SetString("ElementChosen", null);
        PlayerPrefs.SetInt("PlayerSkillPoints", 0);
        PlayerPrefs.SetInt("TierOne", 0);
        PlayerPrefs.SetInt("TierTwo", 0);
        PlayerPrefs.SetInt("TierThree", 0);
        PlayerPrefs.SetInt("SkillOneLevel", 0);
        PlayerPrefs.SetInt("SkillTwoLevel", 0);
    }
    #endregion
    void CanInteract() // Method to handle placing objects on the ground
    {
        RaycastHit hit; // Declare a RaycastHit variable to store information about what the ray hits

        // Cast rays and store whether either hit an object
        bool isHitPos = Physics.Raycast(interactionCastPos.position, interactionCastPos.forward, out hit);
        bool isHitGroundPos = Physics.Raycast(interactionCastGroundPos.position, interactionCastGroundPos.forward, out hit);

        // Draw the debug rays for both positions
        Debug.DrawRay(interactionCastPos.position, interactionCastPos.forward, Color.red, 15);
        Debug.DrawRay(interactionCastGroundPos.position, interactionCastGroundPos.forward, Color.red, 20);

        GameObject hitObject = hit.collider.gameObject;
        // If either ray hits something, check for "Interactable" tag
        if (isHitPos || isHitGroundPos)
        {
            if (hit.collider != null && hit.collider.CompareTag("Interactable"))
            {
                Debug.Log(hit);
                gameManager.InteractableTag.SetActive(true); // Activate the interactable tag
                if(Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Interacting");
                    hitObject.GetComponent<Interactable>().Interact();
                }
                return; // Exit the function once we find an interactable object
            }
        }

        // If no hit or no "Interactable" tag, deactivate the interactable tag
        gameManager.InteractableTag.SetActive(false);
    }


}


