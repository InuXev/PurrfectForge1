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
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour, IDamage
{

    [SerializeField] CharacterController characterControl;
    public int HP;
    public int HPOriginal = 10;
    public int gravity;
    public int moveSpeed;
    public int jumpCounter;
    public int jumpSpeed;
    public int maxJumps;
    public Vector3 playerVelocity;
    public bool grounded;

    private Vector3 moveDirection;

    void Start()
    {

        HP = HPOriginal;

    }
    void Update()
    {

        Movement();

        GravCheck();

        Jump();

    }

    #region Movement Systems


    public void Movement()
    {
        if (characterControl.isGrounded)
        {
            jumpCounter = 0;
            playerVelocity = Vector3.zero;
        }

        moveDirection = (Input.GetAxis("Horizontal") * transform.right) +
            (Input.GetAxis("Vertical") * transform.forward).normalized;
        characterControl.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
    void Jump()
    {

        if (Input.GetButtonDown("Jump") && jumpCounter < maxJumps)
        {
            jumpCounter++;
            playerVelocity.y = jumpSpeed;

        }
        playerVelocity.y -= gravity * Time.deltaTime;
        characterControl.Move(playerVelocity * Time.deltaTime);

    }

    public void GravCheck()
    {
        if (characterControl.isGrounded)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    #endregion


    #region Combat Systems
    public void takeDamage(int damage)
    {

    }

    #endregion


}


