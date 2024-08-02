using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class EshesPlayerEye : MonoBehaviour
{
    [SerializeField] EshesGameManager gameManager;
    [SerializeField] CharacterController characterControl;
    [SerializeField] Transform cameraEye;
    [SerializeField] public GameObject chosenObject;
    private GameObject currentPreviewObject;
    [SerializeField] public Transform objectPreviewPOS;
    GameObject previewObject;
    GameObject placedObject;
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
        ObjectPreview();
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
                    if (chosenObject != null && gameManager.buildON)
                    {
                        Instantiate(chosenObject, hit.point, transform.rotation);
                        chosenObject.GetComponent<MeshCollider>().enabled = true;
                    }
                    if (chosenObject == null)
                    {
                        gameManager.selectSomethingToBuild();
                    }
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



    void ObjectPreview() // Creates or updates the preview object at the chosen position
    {

        if (chosenObject != null && gameManager.buildON)
        {
            ChangePreview();

            // Check if there's an existing preview object
            if (currentPreviewObject != null)
            {
                // Destroy the existing preview object if any
                Destroy(currentPreviewObject);
            }
            Debug.Log("Creating Preview");
            // Instantiate the new preview object and set its properties
            currentPreviewObject = Instantiate(previewObject, objectPreviewPOS.position, transform.rotation, objectPreviewPOS);
            currentPreviewObject.GetComponent<MeshCollider>().enabled = false;
        }
        if(!gameManager.buildON)
        { 
            RemovePreview();
        }

    }

    public void ChangePreview()
    {
        if (chosenObject != previewObject)
        {
            previewObject = chosenObject;

            // If a preview object already exists, it needs to be updated or destroyed and recreated
            if (currentPreviewObject != null)
            {
                Destroy(currentPreviewObject); // Destroy existing preview object
                currentPreviewObject = null;   // Reset the reference
            }
        }
    }
    public void RemovePreview()
    {
        Debug.Log("Removing Preview");

            // If a preview object already exists, it needs to be updated or destroyed and recreated
        if (currentPreviewObject != null)
        {
            Destroy(currentPreviewObject); // Destroy existing preview object
            currentPreviewObject = null;   // Reset the reference
        }
    }
}
