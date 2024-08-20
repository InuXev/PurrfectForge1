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

    // Update is called once per frame
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
                            PrefabList.PrefabData data = new PrefabList.PrefabData
                            {
                                type = item.type,
                                name = item.itemName,
                                position = hit.point,
                                rotation = transform.rotation,
                                scriptableItemName = item.itemName, // Save the name of the ScriptableItem
                                eshesBuildObjectName = item.eshesBuildObject ? item.eshesBuildObject.name : "" // Save the name of the GameObject
                            };
                            //AddPrefabToGame(data);
                            gameManager.prefabList.items.Add(data);
                            saveLoadManager.Save(prefabList);
                            Debug.Log(data.name + " Added to Prefab List");
                            Debug.Log(data.position + " Added to Prefab List");
                            Debug.Log(data.rotation + " Added to Prefab List");
                            foreach (PrefabList.PrefabData listItem in prefabList.items)
                            {
                                Debug.Log($"Name: {listItem.name}, Type: {listItem.type}, Position: {listItem.position}, Rotation: {listItem.rotation}, GameObjectName: {listItem.eshesBuildObjectName}");
                            }
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
                            // Assuming you want to manage the picked-up item data:
                            Debug.Log("Picked up object: " + item.itemName);

                            // If you need to add the item to a player's inventory or similar:
                            // Example: Add the item to the player's inventory or update relevant UI
                            item.amountHeld += 1;
                            int indexToRemove = -1; // Default to an invalid index

                            for (int i = 0; i < prefabList.items.Count; i++)
                            {
                                var prefab = prefabList.items[i];
                                if (prefab.name == item.itemName && prefab.position == hit.collider.transform.position)
                                {
                                    indexToRemove = i;
                                    break;
                                }
                            }

                            if (indexToRemove >= 0)
                            {
                                // Store the name of the prefab before removing it
                                string removedPrefabName = prefabList.items[indexToRemove].name;

                                // Remove the prefab from the list
                                prefabList.items.RemoveAt(indexToRemove);

                                // Log the removal after successfully removing it from the list
                                Debug.Log("Removed prefab: " + removedPrefabName);
                            }
                            else
                            {
                                Debug.LogError("Prefab not found for removal.");
                            }
                            foreach (var prefab in prefabList.items)
                            {
                                Debug.Log(prefab.name + " still in Prefab List.");
                            }

                            // Destroy the object
                            Destroy(hit.collider.gameObject);
                        }
                        else
                        {
                            Debug.LogError("ScriptableItems component is missing from the hit object!");
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
            else
            {
                Debug.LogError("ItemData component is missing from chosenObject!");
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
    //void AddPrefabToGame(PrefabList.PrefabData prefabData)
    //{
    //    // Assume prefabList is already initialized somewhere in your GameManager or relevant script

    //    // Create a new PrefabData item
    //    prefabData = new PrefabList.PrefabData(
    //        "Building",                  // Type of the prefab
    //        "HousePrefab",               // Name or identifier
    //        new Vector3(0, 0, 0),        // Position
    //        Quaternion.identity,         // Rotation
    //        "HouseScriptableItem",       // Scriptable Object Name
    //        ""                           // Leave eshesBuildObjectName empty for now
    //    );

    //    // Set the correct path for the prefab within the Resources folder
    //    prefabData.SetEshesBuildObjectName("Prefabs/Buildings", "HousePrefab");

    //    // Add the prefab data to the list
    //    prefabList.items.Add(prefabData);

    //    // Save the list using your SaveLoadManager (assuming it's already initialized)
    //    saveLoadManager.Save(prefabList);
    //}
}
