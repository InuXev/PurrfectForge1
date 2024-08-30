using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBasics : MonoBehaviour
{
    // Start is called before the first frame update
    private float gravity = 20;
    private Vector3 playerVelocity;
    private Vector3 moveDirection;
    private float moveSpeed = 8;
    private float dashMult = 2;
    float panSpeed = 6f;
    [SerializeField] CharacterController characterControl;
    public int jumpCounter;
    private float jumpSpeed = 8F;
    private int maxJumps = 1;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Dash();
        Movement();
        Jump();
        if (characterControl.isGrounded)
        {
            jumpCounter = 0;
        }
    }
    void Dash()
    {
        if (Input.GetKeyDown("left shift"))
        {
            moveSpeed *= dashMult;
        }
        if (Input.GetKeyUp("left shift"))
        {
            moveSpeed /= dashMult;
        }
    }
    void Movement()
    {
        float x = panSpeed * Input.GetAxis("Mouse X");
        transform.Rotate(0, x, 0);

        moveDirection = (Input.GetAxis("Horizontal") * transform.right) +
        (Input.GetAxis("Vertical") * transform.forward);
        characterControl.Move(moveDirection * moveSpeed * Time.deltaTime);

        playerVelocity.y -= gravity * Time.deltaTime;
        characterControl.Move(playerVelocity * Time.deltaTime);
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
}
