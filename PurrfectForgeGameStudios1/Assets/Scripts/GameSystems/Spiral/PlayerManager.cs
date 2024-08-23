using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
//using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour, PDamage
{
    #region Fields/Objects

    [SerializeField] CharacterController characterControl;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject currentWeapon;
    [SerializeField] GameObject currentShield;
    [SerializeField] Animator Anim;

    public static PlayerManager Instance;


    //player info
    public int playerLevel = 1;
    public int playerCoin;
    int playerLevelMax = 50;
    public float HP;
    public float HPOriginal = 5;
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

    #endregion

    #region Processes
    void Awake()
    {
        AwakenProcesses();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        StartUpProcesses();
    }
    void Update()
    {
        UpdateProcesses();
    }
    #endregion

    #region Movement Systems

    public void Turn()
    {
        float x = panSpeed * Input.GetAxis("Mouse X");
        float y = panSpeed * Input.GetAxis("Mouse Y");
        transform.Rotate(0, x, 0);
    }
    public void Walk()
    {
        if (characterControl.isGrounded)
        {
            jumpCounter = 0;
            playerVelocity = Vector3.zero;
        }

        moveDirection = (Input.GetAxis("Horizontal") * transform.right) +
            (Input.GetAxis("Vertical") * transform.forward)/*.normalized*/;
        characterControl.Move(moveDirection * MoveSpeed * Time.deltaTime);
    }
    public void Dash()
    {
        if (Input.GetButtonDown("Dash") && !dashing && Stamina > 0)
        {
            dashing = true;
            MoveSpeed *= dashMult;

            // Start draining stamina
            if (staminaDrainCoroutine != null)
            {
                StopCoroutine(staminaDrainCoroutine);
            }
            staminaDrainCoroutine = StartCoroutine(StaminaDrain());

            // Stop refill coroutine if it's running
            if (staminaRefillCoroutine != null)
            {
                StopCoroutine(staminaRefillCoroutine);
                staminaRefillCoroutine = null;
            }
        }

        if (Input.GetButtonUp("Dash") && dashing)
        {
            dashing = false;
            MoveSpeed = MoveSpeedOriginal;

            if (staminaDrainCoroutine != null)
            {
                StopCoroutine(staminaDrainCoroutine);
                staminaDrainCoroutine = null;
            }

            // Start the refill coroutine if not already running
            if (staminaRefillCoroutine == null)
            {
                staminaRefillCoroutine = StartCoroutine(StaminaRefill());
            }
        }
    }


    public void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCounter < maxJumps)
        {
            jumpCounter++;
            playerVelocity.y = jumpSpeed;
            Stamina -= 1;
            if (staminaDrainCoroutine != null)
            {
                StopCoroutine(staminaDrainCoroutine);
                staminaDrainCoroutine = null;
            }

            // Start the refill coroutine if not already running
            if (staminaRefillCoroutine == null)
            {
                staminaRefillCoroutine = StartCoroutine(StaminaRefill());
            }
        }
        playerVelocity.y -= gravity * Time.deltaTime;
        characterControl.Move(playerVelocity * Time.deltaTime);

    }

    #endregion

    #region Combat / Health Systems

    public void Melee()
    {
        if (currentWeapon != null && !shieldUp)
        {
            if (Input.GetButtonDown("Left Mouse"))
            {
                StartCoroutine(Swing());
            }
        }
    }
    public void Defend()
    {
        if (currentShield != null)
        {
            if (Input.GetButtonDown("Right Mouse"))
            {
                currentShield.SetActive(true);
                shieldUp = true;
                playerShieldMod = .5F;
            }
            if (Input.GetButtonUp("Right Mouse"))
            {
                currentShield.SetActive(false);
                shieldUp = false;
                playerShieldMod = 0;
            }
        }
    }
    public void DeathCheck()
    {
        if (HP <= 0)
        {
            UpdatePlayerUI();
            //gameManager.statePaused();
            gameManager.youDead();
        }
    }

    public void takeDamage(float damage)
    {
        if (shieldUp)
        {
            float damageTaken = DamageShieldUPCalc(damage);
            HP -= damageTaken;
            Stamina -= .1F;
            if (staminaDrainCoroutine != null)
            {
                StopCoroutine(staminaDrainCoroutine);
                staminaDrainCoroutine = null;
            }

            // Start the refill coroutine if not already running
            if (staminaRefillCoroutine == null)
            {
                staminaRefillCoroutine = StartCoroutine(StaminaRefill());
            }
            StartCoroutine(HitFlash());
            DeathCheck();
        }
        else
        {
            float damageTaken = DamageShieldDownCalc(damage);
            HP -= damageTaken;
            StartCoroutine(HitFlash());
            DeathCheck();
        }
    }
    IEnumerator HitFlash()
    {
        if (HP > 0)
        {
            gameManager.playerHitFlash.SetActive(true);
            yield return new WaitForSeconds(.1F);
            gameManager.playerHitFlash.SetActive(false);
        }
    }
    IEnumerator Swing()
    {
        if (Stamina >= SwingCostCalc())
        {
            float swingCost = SwingCostCalc();
            Stamina -= swingCost;
            currentWeapon.SetActive(true);
            Anim.SetTrigger("Attacking");
            yield return new WaitForSeconds(0.2F);
            currentWeapon.SetActive(false);

            // Ensure the refill coroutine starts if needed
            if (staminaRefillCoroutine != null)
            {
                StopCoroutine(staminaRefillCoroutine);
                staminaRefillCoroutine = null;
            }

            // Restart the refill coroutine after the swing action if dashing isn't active
            if (!dashing)
            {
                staminaRefillCoroutine = StartCoroutine(StaminaRefill());
            }
        }
    }


    public float DamageShieldUPCalc(float damage)
    {
        float damageOut;
        float damageReduction = damage * playerShieldMod;
        damageOut = damage - damageReduction;
        return damageOut;
    }
    public float DamageShieldDownCalc(float damage)
    {
        float damageOut;
        float damageReduction = damage * (Def * .025F);
        damageOut = damage - damageReduction;
        return damageOut;
    }
    public float SwingCostCalc()
    {

        float cost;
        float costMod = Dex * .0025F;
        float baseCost = 3F - costMod;
        cost = baseCost;
        return cost;
    }


    #endregion

    #region UI Systems

    public void UpdatePlayerUI()
    {
        //HP updater
        float hpFillAmount = HP / HPOriginal;
        //convert hpFillAmount to a percentage for display
        int hpPercentage = (int)(hpFillAmount * 100);
        gameManager.playerHP.fillAmount = hpFillAmount;
        gameManager.playerHPText.text = (hpPercentage.ToString() + "%");

        //XP updater
        float xpFillAmount = playerXP / (100 * playerLevel);
        int xpPercentage = (int)(xpFillAmount * 100);
        gameManager.playerXPBar.fillAmount = xpFillAmount;

        //StaminaUpdater
        float stamFillAmount = Stamina / StaminaOriginal;
        int stamPercentage = (int)(stamFillAmount * 100);
        gameManager.playerStamBar.fillAmount = stamFillAmount;

        playerCoin = gameManager.coinPurse.amountHeld;

        gameManager.playerLvLText.text = playerLevel.ToString();
        gameManager.playerHPStat.text = HPOriginal.ToString();
        gameManager.playerAttStat.text = AttackOriginal.ToString();
        gameManager.playerDefStat.text = DefOriginal.ToString();
        gameManager.playerDexStat.text = DexOriginal.ToString();
        gameManager.playerStamStat.text = StaminaOriginal.ToString();
        gameManager.playerCoins.text = playerCoin.ToString();
    }
    private void playerLevelUp()
    {
        if (playerXP >= (100 * playerLevel) && playerLevel < playerLevelMax)
        {
            //show the level up screen
            //increase stats by random number
            int randBoostHP = UnityEngine.Random.Range(1, 10);
            int randBoostAttack = UnityEngine.Random.Range(1, 5);
            int randBoostDef = UnityEngine.Random.Range(1, 3);
            float randBoostMoveSpeed = UnityEngine.Random.Range(.01F, .05F);
            int randBoostStamina = UnityEngine.Random.Range(1, 4);
            int randBoostDex = UnityEngine.Random.Range(1, 3);

            HP += randBoostHP;
            HPOriginal += randBoostHP;
            Attack += randBoostAttack;
            AttackOriginal += randBoostAttack;
            Def += randBoostDef;
            DefOriginal += randBoostDef;
            Dex += randBoostDex;
            DexOriginal += randBoostDex;
            MoveSpeed += randBoostMoveSpeed;
            MoveSpeedOriginal += randBoostMoveSpeed;
            Stamina += randBoostStamina;
            StaminaOriginal += randBoostStamina;
            playerLevel++;
            UpdatePlayerUI();

            playerXP = 0;
            //reset playerXp to zero
        }
    }

    IEnumerator StaminaDrain()
    {
        while (dashing && Stamina > 0)
        {
            yield return new WaitForSeconds(0.1F); // Adjust time to control the drain rate
            Stamina -= 0.1F; // Adjust the amount drained per second
            UpdatePlayerUI();
            if (Stamina <= 0)
            {
                Stamina = 0;
                dashing = false;
                MoveSpeed = MoveSpeedOriginal; // Reset speed if stamina is depleted

                // Stop the drain coroutine
                if (staminaDrainCoroutine != null)
                {
                    StopCoroutine(staminaDrainCoroutine);
                    staminaDrainCoroutine = null;
                }

                // Start the refill coroutine if not already running
                if (staminaRefillCoroutine == null)
                {
                    staminaRefillCoroutine = StartCoroutine(StaminaRefill());
                }

                yield break; // Exit the coroutine as the player has no stamina left
            }
        }
    }

    IEnumerator StaminaRefill()
    {
        Debug.Log("Starting StaminaRefill coroutine.");

        // Continue refilling as long as stamina is below the maximum and not dashing
        while (Stamina < StaminaOriginal)
        {
            // Wait for a short period before refilling
            yield return new WaitForSeconds(0.1F); // Adjust this value to control the refill delay

            // Gradually refill stamina
            Stamina += 0.1F + (Dex * .005F); // Adjust this value for desired refill speed
            Stamina = Mathf.Clamp(Stamina, 0, StaminaOriginal); // Ensure stamina does not exceed the original amount
            UpdatePlayerUI();

            // Debug log to check refill progress
            Debug.Log($"Refilling stamina. Current stamina: {Stamina}");

            // Stop refilling if the player starts dashing or attacking
            //if (dashing || (staminaDrainCoroutine != null && Stamina <= 0))
            //{
            //    Debug.Log("Stopping refill coroutine due to dashing or attacking.");
            //    yield break; // Exit the coroutine
            //}
        }

        // Ensure stamina is capped at maximum
        Stamina = StaminaOriginal;
        Debug.Log("Stamina refill complete.");

        staminaRefillCoroutine = null; // Reset coroutine reference
    }


    #endregion

    #region Organizational Systems

    private void StartUpProcesses()
    {
        GetPlayerPrefs();
    }
    private void AwakenProcesses()
    {
        playerPOS = transform.position;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void UpdateProcesses()
    {
        if (!gameManager.isPaused)
        {
            Melee();

            Defend();

            Walk();

            Turn();

            Jump();

            Dash();

            DeathCheck();

            playerLevelUp();

            UpdatePlayerUI();
        }
    }
    public void GetPlayerPrefs()
    {
        HPOriginal = PlayerPrefs.GetFloat("HPOringal", HPOriginal);
        HP = HPOriginal;
        AttackOriginal = PlayerPrefs.GetFloat("AttackOriginal", AttackOriginal);
        Attack = AttackOriginal;
        DefOriginal = PlayerPrefs.GetFloat("DefOriginal", DefOriginal);
        Def = DefOriginal;
        MoveSpeedOriginal = PlayerPrefs.GetFloat("MoveSpeedOriginal", MoveSpeedOriginal);
        MoveSpeed = MoveSpeedOriginal;
        StaminaOriginal = PlayerPrefs.GetFloat("StaminaOriginal", StaminaOriginal);
        Stamina = StaminaOriginal;
        DexOriginal = PlayerPrefs.GetFloat("DexOriginal", DexOriginal);
        Dex = DexOriginal;
        playerXP = PlayerPrefs.GetFloat("playerXP", playerXP);
        playerLevel = PlayerPrefs.GetInt("playerLevel", playerLevel);
        maxJumps = PlayerPrefs.GetInt("maxJumps", maxJumps);
        playerCoin = PlayerPrefs.GetInt("playerCoin", playerCoin);
    }
    public void SavePlayerPrefs()
    {

        PlayerPrefs.SetFloat("HPOringal", HPOriginal);
        PlayerPrefs.SetFloat("AttackOriginal", AttackOriginal);
        PlayerPrefs.SetFloat("DefOriginal", DefOriginal);
        PlayerPrefs.SetFloat("DexOriginal", DexOriginal);
        PlayerPrefs.SetFloat("MoveSpeedOriginal", MoveSpeedOriginal);
        PlayerPrefs.SetFloat("StaminaOriginal", StaminaOriginal);
        PlayerPrefs.SetFloat("playerXP", playerXP);
        PlayerPrefs.SetInt("playerLevel", playerLevel);
        PlayerPrefs.SetInt("maxJumps", maxJumps);
        PlayerPrefs.SetInt("playerCoin", playerCoin);
    }
    public void ResetSetPlayerPrefs()
    {

        PlayerPrefs.SetFloat("HPOringal", 5);
        PlayerPrefs.SetFloat("AttackOriginal", 5);
        PlayerPrefs.SetFloat("DefOriginal", 1);
        PlayerPrefs.SetFloat("DexOriginal", 1);
        PlayerPrefs.SetFloat("MoveSpeedOriginal", 4);
        PlayerPrefs.SetFloat("StaminaOriginal", 5);
        PlayerPrefs.SetFloat("playerXP", 0);
        PlayerPrefs.SetInt("playerLevel", 1);
        PlayerPrefs.SetInt("maxJumps", 1);
        PlayerPrefs.SetInt("playerCoin", playerCoin);

    }
    #endregion

}


