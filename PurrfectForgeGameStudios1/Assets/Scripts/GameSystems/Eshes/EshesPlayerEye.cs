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
    [SerializeField] SaveLoadManager saveLoadManager;
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

    public PrefabList prefabList;
    void Start()
    {

    }
    void Update()
    {
        Walk();
        Dash();
        GroundSearchPlace();
        GroundSearchPickup();
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
    void GroundSearchPlace()
    {
        RaycastHit hit; // Create a raycast hit variable
        Debug.DrawRay(cameraEye.position, Vector3.down, Color.clear, 10000);
        if (Physics.Raycast(cameraEye.position, Vector3.down, out hit)) // Check if the raycast hits something
        {
            if (hit.collider.CompareTag("EshesGround")) // Check if the object hit is the ground
            {
                if (chosenObject != null)
                {
                    ItemData itemData = chosenObject.GetComponent<ItemData>();
                    ScriptableItems item = itemData.scriptableItems;
                    // Can build an object at this hit
                    if (Input.GetKeyDown(KeyCode.E) && item.amountHeld > 0)
                    {
                        if (gameManager.buildON)
                        {
                            // Instantiate the chosen object
                            GameObject placedObject = Instantiate(chosenObject, hit.point, transform.rotation);
                           
                            // Get the ItemData component from the chosenObject
                            if (itemData != null)
                            {
                                // Access the ScriptableItems instance
                                if (item != null)
                                {
                                    // Directly update the amountHeld property
                                    item.amountHeld -= 1;
                                    gameManager.UpdateItemCounts();
                                    Debug.Log($"{item.itemName} amountHeld decreased to {item.amountHeld}");
                                    if (item.amountHeld == 0)
                                    {
                                        RemovePreview();
                                    }
                                }
                            }
                            placedObject.GetComponent<MeshCollider>().enabled = true;
                        }
                        else if (chosenObject == null)
                        {
                            gameManager.selectSomethingToBuild();
                        }
                    }
                }
            }
        }
    }
    void GroundSearchPickup()
    {
        RaycastHit hit; // Create a raycast hit variable
        Debug.DrawRay(cameraEye.position, Vector3.down, Color.clear, 10000);

        if (Physics.Raycast(cameraEye.position, Vector3.down, out hit)) // Check if the raycast hits something
        {
            if (hit.collider.CompareTag("WorldObject")) // Check if the object hit is a world object
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    // Get the ItemData component from the object that the raycast hit
                    ItemData itemData = hit.collider.GetComponent<ItemData>();

                    if (itemData != null)
                    {
                        ScriptableItems item = itemData.scriptableItems;

                        if (item != null)
                        {
                            // Log initial state
                            Debug.Log("Before removal:");
                            foreach (var prefab in prefabList.items)
                            {
                                Debug.Log("Prefab name: " + prefab.name + ", Position: " + prefab.position);
                            }

                            item.amountHeld += 1;
                            Destroy(hit.collider.gameObject);
                        }
                    }
                    else
                    {
                        Debug.LogError("ItemData component is missing from the hit object!");
                    }
                }
            }
        }
    }



    void ObjectPreview() // Creates or updates the preview object at the chosen position
    {
        if (chosenObject != null && gameManager.buildON)
        {
            ItemData itemData = chosenObject.GetComponent<ItemData>();
            if (itemData != null)
            {
                ScriptableItems item = itemData.scriptableItems;

                // Only show preview if amountHeld is greater than zero
                if (item.amountHeld > 0)
                {
                    ChangePreview();

                    // Check if there's an existing preview object
                    if (currentPreviewObject != null)
                    {
                        // Destroy the existing preview object if any
                        Destroy(currentPreviewObject);
                    }
                    // Instantiate the new preview object and set its properties
                    currentPreviewObject = Instantiate(previewObject, objectPreviewPOS.position, transform.rotation, objectPreviewPOS);
                    currentPreviewObject.GetComponent<MeshCollider>().enabled = false;
                }
                else
                {
                    // If amountHeld is zero, remove the preview
                    RemovePreview();
                }
            }
        }
        else if (!gameManager.buildON)
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
        // If a preview object already exists, it needs to be updated or destroyed and recreated
        if (currentPreviewObject != null)
        {
            Destroy(currentPreviewObject); // Destroy existing preview object
            currentPreviewObject = null;   // Reset the reference
        }
    }
}
