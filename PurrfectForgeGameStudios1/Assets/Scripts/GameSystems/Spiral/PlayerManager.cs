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
    int playerLevel;
    int playerLevelMax = 100;
    public float HP;
    public float HPOriginal = 10;
    public float Attack;
    public float AttackOriginal = 5;
    public float Def;
    public float DefOriginal = 5;
    public float Dex;
    public float DexOriginal = 5;
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
    public float playerXPMax = 100;

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

            if (staminaRefillCoroutine != null)
            {
                StopCoroutine(staminaRefillCoroutine);
            }
            staminaRefillCoroutine = StartCoroutine(StaminaRefill());
        }
    }
    public void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCounter < maxJumps)
        {
            jumpCounter++;
            playerVelocity.y = jumpSpeed;

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
                playerShieldMod = .75F;
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
            float damageReduction = damage * playerShieldMod;
            float damageTaken = damage - damageReduction;

            HP -= damageTaken;
            StartCoroutine(HitFlash());
            DeathCheck();
        }
        else
        {
            float defReduction = Def * .1F;
            HP -= (damage - defReduction);
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
        Stamina -= 1;
        currentWeapon.SetActive(true);
        Anim.SetTrigger("Attacking");
        yield return new WaitForSeconds(.2F);
        currentWeapon.SetActive(false);
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
        float xpFillAmount = playerXP / playerXPMax;
        int xpPercentage = (int)(xpFillAmount * 100);
        gameManager.playerXPBar.fillAmount = xpFillAmount;

        //StaminaUpdater
        float stamFillAmount = Stamina / StaminaOriginal;
        int stamPercentage = (int)(stamFillAmount * 100);
        gameManager.playerStamBar.fillAmount = stamFillAmount;
    }
    private void playerLevelUp()
    {
        if (playerXP >= 100 && playerLevel < playerLevelMax)
        {
            //show the level up screen
            //increase stats by random number
            float randBoostHP = UnityEngine.Random.Range(1F, 10F);
            float randBoostAttack = UnityEngine.Random.Range(1F, 3F);
            float randBoostDef = UnityEngine.Random.Range(1F, 3F);
            float randBoostMoveSpeed = UnityEngine.Random.Range(.01F, .05F);
            float randBoostStamina = UnityEngine.Random.Range(1F, 3F);
            float randBoostDex = UnityEngine.Random.Range(1F, 3F);

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

                // Stop the drain coroutine and start the refill coroutine
                if (staminaDrainCoroutine != null)
                {
                    StopCoroutine(staminaDrainCoroutine);
                    staminaDrainCoroutine = null;
                }

                // Start the stamina refill coroutine
                if (staminaRefillCoroutine != null)
                {
                    StopCoroutine(staminaRefillCoroutine);
                }
                staminaRefillCoroutine = StartCoroutine(StaminaRefill());
            }
        }
    }
    IEnumerator StaminaRefill()
    {
        while (Stamina < StaminaOriginal)
        {
            // Gradually refill stamina
            Stamina += 0.1F; // Adjust this value for desired refill speed
            Stamina = Mathf.Clamp(Stamina, 0, StaminaOriginal); // Ensure stamina does not exceed the original amount
            UpdatePlayerUI();
            yield return new WaitForSeconds(0.2F); // Adjust this value to control the refill delay
        }

        // Ensure stamina is capped at maximum
        Stamina = StaminaOriginal;
    }
    #endregion

    #region Organizational Systems

    private void StartUpProcesses()
    {
        HP = HPOriginal;
        Attack = AttackOriginal;
        Def = DefOriginal;
        MoveSpeed = MoveSpeedOriginal;
        Stamina = StaminaOriginal;
        Dex = DexOriginal;
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

    #endregion

}


