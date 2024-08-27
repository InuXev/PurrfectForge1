using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EshesPlayerEye : MonoBehaviour
{
    [SerializeField] SaveLoadManager saveLoadManager;
    [SerializeField] EshesGameManager gameManager;
    [SerializeField] CharacterController characterControl;
    [SerializeField] Transform cameraEye;
    [SerializeField] public GameObject chosenObject;
    [SerializeField] public Camera eshesCamera;
    private GameObject currentPreviewObject;
    [SerializeField] public Transform objectPreviewPOS;
    GameObject previewObject;
    GameObject placedObject;
    private Vector3 moveDirection;
    public float moveSpeed;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 eyeOriginalPosition;
    private Quaternion eyeOriginalRotation;
    public PrefabList prefabList;
    private Vector3 fixedPreviewPosition;
    private void Awake()
    {
        fixedPreviewPosition = objectPreviewPOS.position;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        eyeOriginalPosition = cameraEye.position;
        eyeOriginalRotation = cameraEye.rotation;
    }

    void Update()
    {
        Walk();
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
    void GroundSearchPlace()
    {
        RaycastHit hit;
        Debug.DrawRay(cameraEye.position, Vector3.down, Color.clear, 10000);
        if (Physics.Raycast(cameraEye.position, Vector3.down, out hit))
        {
            if (hit.collider.CompareTag("EshesGround"))
            {
                if (chosenObject != null)
                {
                    ItemData itemData = chosenObject.GetComponent<ItemData>();
                    ScriptableItems item = itemData.scriptableItems;
                    if (Input.GetKeyDown(KeyCode.E) && item.amountHeld > 0)
                    {
                        if (gameManager.buildON)
                        {
                            GameObject placedObject = Instantiate(chosenObject, hit.point, transform.rotation);
                            item.amountHeld -= 1;
                            gameManager.UpdateItemCounts();
                            if (item.amountHeld == 0)
                            {
                                RemovePreview();
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
        RaycastHit hit;
        Debug.DrawRay(cameraEye.position, Vector3.down, Color.clear, 10000);

        if (Physics.Raycast(cameraEye.position, Vector3.down, out hit))
        {
            if (hit.collider.CompareTag("WorldObject"))
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    ItemData itemData = hit.collider.GetComponent<ItemData>();

                    if (itemData != null)
                    {
                        ScriptableItems item = itemData.scriptableItems;
                        item.amountHeld += 1;
                        Destroy(hit.collider.gameObject);
                    }
                }
            }
        }
    }

    public void ObjectPreview()
    {
        if (chosenObject != null && gameManager.buildON)
        {
            ItemData itemData = chosenObject.GetComponent<ItemData>();
            if (itemData != null)
            {
                ScriptableItems item = itemData.scriptableItems;

                if (item.amountHeld > 0)
                {
                    ChangePreview();

                    if (currentPreviewObject == null)
                    {
                        currentPreviewObject = Instantiate(previewObject, objectPreviewPOS.position, Quaternion.identity);
                        currentPreviewObject.GetComponent<MeshCollider>().enabled = false;
                    }
                    else
                    {
                        currentPreviewObject.transform.position = objectPreviewPOS.position;
                    }
                }
                else
                {
                    RemovePreview();
                }
            }
        }
        else if (!gameManager.buildON)
        {
            RemovePreview();
        }
    }

    public void UpdatePreviewAfterReset()
    {
        if (chosenObject != null && gameManager.buildON)
        {
            if (currentPreviewObject != null)
            {
                currentPreviewObject.transform.position = objectPreviewPOS.position;
                currentPreviewObject.transform.rotation = Quaternion.identity;
            }
            else
            {
                ObjectPreview();
            }
        }
    }

    public void ChangePreview()
    {
        if (chosenObject != previewObject)
        {
            previewObject = chosenObject;

            if (currentPreviewObject != null)
            {
                Destroy(currentPreviewObject);
                currentPreviewObject = Instantiate(previewObject, objectPreviewPOS.position, Quaternion.identity);
                currentPreviewObject.GetComponent<MeshCollider>().enabled = false;
            }
        }
    }

    public void RemovePreview()
    {
        if (currentPreviewObject != null)
        {
            Destroy(currentPreviewObject);
            currentPreviewObject = null;
        }
    }

    public void ResetPosition()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        cameraEye.position = eyeOriginalPosition;
        cameraEye.rotation = eyeOriginalRotation;
        RemovePreview();
    }
}
