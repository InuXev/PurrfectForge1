using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
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

    private float panSpeed = 7.5F;
    public float HP;
    public float HPOriginal = 10;
    public float moveSpeed;
    private float moveSpeedOriginal = 4;
    public float dashMult;
    private float gravity = 20;
    public int jumpCounter;
    public float jumpSpeed;
    public int maxJumps;
    private Vector3 playerVelocity;
    private Vector3 moveDirection;
    private Vector3 playerPOS;

    #endregion

    #region Processes
    void Awake()
    {
        AwakenProcesses();
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
        characterControl.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    public void Dash()
    {
        if (Input.GetButtonDown("Dash"))
        {
            moveSpeed = moveSpeed * dashMult;
        }
        if (Input.GetButtonUp("Dash"))
        {
            moveSpeed = moveSpeedOriginal;
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
        if (currentWeapon != null)
        {
            if (Input.GetButtonDown("Left Mouse"))
            {
                StartCoroutine(Swing());
            }
        }
    }
    public void Defend()
    {
        //if(currentShield != null)
        //{
        if (Input.GetButtonDown("Right Mouse"))
        {
            currentShield.SetActive(true);
        }
        if (Input.GetButtonUp("Right Mouse"))
        {
            currentShield.SetActive(false);
        }
        //}
    }
    public void DeathCheck()
    {
        if (HP <= 0)
        {
            UpdatePlayerUI();
            gameManager.statePaused();
            gameManager.youDead();
        }
    }

    public void takeDamage(float damage)
    {
        HP -= damage;
        DeathCheck();
    }

    IEnumerator Swing()
    {
        currentWeapon.SetActive(true);
        Anim.SetTrigger("Attacking");
        yield return new WaitForSeconds(.3F);
        currentWeapon.SetActive(false);
    }



    #endregion

    #region UI Systems

    public void UpdatePlayerUI()
    {
        float hpFillAmount = HP / HPOriginal;

        gameManager.playerHP.fillAmount = hpFillAmount;
        gameManager.playerHPText.text = HP.ToString();
    }

    #endregion

    #region Organizational Systems

    private void StartUpProcesses()
    {
        moveSpeed = moveSpeedOriginal;
        HP = HPOriginal;
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

            UpdatePlayerUI();
        }
    }

    #endregion

}


