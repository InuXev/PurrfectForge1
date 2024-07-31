using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EshesPlayerEye : MonoBehaviour
{

    [SerializeField] CharacterController characterControl;
    [SerializeField] Transform cameraEye;
    [SerializeField] GameObject chosenObject;
    private Vector3 moveDirection;
    public float moveSpeed;
    private float moveSpeedOriginal = 8;
    public float dashMult;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        Dash();
        GroundSearch();
    }

    public void Walk()
    {
        moveDirection = (Input.GetAxis("Horizontal") * transform.right) +
            (Input.GetAxis("Vertical") * transform.forward).normalized;
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

    void GroundSearch()
    {
        RaycastHit hit; // Create a raycast hit variable
        Debug.DrawRay(cameraEye.position, Vector3.down, Color.clear, 10000);
        if (Physics.Raycast(cameraEye.position, Vector3.down, out hit)) // Check if the raycast hits something
        {
            if (hit.collider.CompareTag("EshesGround")) // Check if the object hit is the ground
            {
                //can build an object at this hit
                if (Input.GetKeyDown(KeyCode.E))
                {

                    //places the object you have chosen from the build menu
                    //this needs a confirm
                    Instantiate(chosenObject, hit.point, transform.rotation);

                }
            }
            else
            {
                if (hit.collider.CompareTag("WorldObject")) // Check if the object hit is the ground
                {
                    //can build an object at this hit
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        //destroy object but increase the corresponding amount in inventory
                        //this needs a confirm
                        Destroy(hit.collider.gameObject);

                    }
                }
            }
        }
    }
}
