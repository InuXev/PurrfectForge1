using System.Collections; // Import the System.Collections namespace (not used in this script)
using System.Collections.Generic; // Import the System.Collections.Generic namespace for using lists
using UnityEditor.Animations;
using UnityEngine; // Import the UnityEngine namespace for Unity-specific features like MonoBehaviour, GameObject, etc.

public class EshesPlayerEye : MonoBehaviour // Define the EshesPlayerEye class, inheriting from MonoBehaviour
{
    [SerializeField] SaveLoadManager saveLoadManager; // Reference to the SaveLoadManager component, assigned via the Unity Editor
    [SerializeField] EshesGameManager gameManager; // Reference to the EshesGameManager component, assigned via the Unity Editor
    [SerializeField] CharacterController characterControl; // Reference to the CharacterController component, assigned via the Unity Editor
    [SerializeField] Transform cameraEye; // Reference to the camera's Transform, assigned via the Unity Editor
    [SerializeField] public GameObject chosenObject; // Public reference to the currently selected object, assigned via the Unity Editor
    [SerializeField] public Camera eshesCamera; // Public reference to the Camera component, assigned via the Unity Editor
    private GameObject currentPreviewObject; // Private variable to store the currently displayed preview object
    [SerializeField] public Transform objectPreviewPOS; // Public reference to the position for preview objects, assigned via the Unity Editor
    GameObject previewObject; // Private variable to store the object being previewed
    GameObject placedObject; // Private variable to store the last placed object
    private Vector3 moveDirection; // Private variable to store the player's movement direction
    public float moveSpeed; // Public variable to set the player's movement speed
    private Vector3 originalPosition; // Private variable to store the player's original position
    private Quaternion originalRotation; // Private variable to store the player's original rotation
    private Vector3 eyeOriginalPosition; // Private variable to store the camera's original position
    private Quaternion eyeOriginalRotation; // Private variable to store the camera's original rotation
    public PrefabList prefabList; // Public variable to reference a list of prefabs
    public Quaternion currentRotation;
    private Vector3 fixedPreviewPosition; // Private variable to store the fixed preview position
    public Material previewMaterial;
    private void Awake() // Awake is called when the script instance is being loaded
    {
        fixedPreviewPosition = objectPreviewPOS.position; // Store the initial position of the preview object
        originalPosition = transform.position; // Store the OverHeadEye's original position
        originalRotation = transform.rotation; // Store the OverHeadEye's original rotation
        eyeOriginalPosition = cameraEye.position; // Store the camera's original position
        eyeOriginalRotation = cameraEye.rotation; // Store the camera's original rotation
    }
    public void Start()
    {

    }
    void Update() // Update is called once per frame
    {
        Walk(); // Call the Walk method to handle player movement
        GroundSearchPlace(); // Call the GroundSearchPlace method to handle placing objects
        GroundSearchPickup(); // Call the GroundSearchPickup method to handle picking up objects
        ObjectPreview(); // Call the ObjectPreview method to handle displaying object previews
        PreviewRotate(); //Rotates the preview before placement
    }
    public void ResetPosition() // Method to reset the OverHeadEye's position and rotation
    {
        transform.position = originalPosition; // Reset the OverHeadEye's position to the original position
        transform.rotation = originalRotation; // Reset the OverHeadEye's rotation to the original rotation
        cameraEye.position = eyeOriginalPosition; // Reset the camera's position to the original position
        cameraEye.rotation = eyeOriginalRotation; // Reset the camera's rotation to the original rotation
        RemovePreview(); // Call the RemovePreview method to remove the object preview
    }
    public void Walk() // Method to handle player movement
    {
        moveDirection = (Input.GetAxis("HorizontalMove") * transform.right) + (Input.GetAxis("VerticalMove") * transform.forward).normalized; // Calculate the movement direction based on player input
        characterControl.Move(moveDirection * moveSpeed * Time.deltaTime); // Move the player using the CharacterController component
    }

    void GroundSearchPlace() // Method to handle placing objects on the ground
    {
        RaycastHit hit; // Declare a RaycastHit variable to store information about what the ray hits
        Debug.DrawRay(cameraEye.position, Vector3.down, Color.red, 10000); // Draw a debug ray downwards from the camera's position

        if (Physics.Raycast(cameraEye.position, Vector3.down, out hit)) // Perform a raycast downwards from the camera's position
        {
            if (hit.collider.CompareTag("EshesGround")) // Check if the ray hit an object tagged as "EshesGround"
            {
                if (chosenObject != null) // Check if a chosen object is selected
                {
                    ItemData itemData = chosenObject.GetComponent<ItemData>(); // Get the ItemData component from the chosen object
                    ScriptableItems item = itemData.scriptableItems; // Get the ScriptableItems reference from the ItemData component

                    if (Input.GetKeyDown(KeyCode.E) && item.amountHeld > 0) // Check if the "E" key is pressed and if the item is available
                    {
                        Debug.Log("placing");
                        if (gameManager.buildON) // Check if building mode is enabled in the game manager
                        {
                            GameObject placedObject = Instantiate(chosenObject, hit.point, currentPreviewObject.transform.rotation); // Instantiate the chosen object at the hit point
                            item.amountHeld -= 1; // Decrease the amount held for the item
                            gameManager.UpdateItemCounts(); // Update the item counts in the game manager

                            if (item.amountHeld == 0) // Check if the item is depleted
                            {
                                RemovePreview(); // Call the RemovePreview method to remove the object preview
                            }
                        }
                        else if (chosenObject == null) // If no object is selected
                        {
                            gameManager.selectSomethingToBuild(); // Prompt the player to select something to build
                        }
                    }
                }
            }
        }
    }

    void GroundSearchPickup() // Method to handle picking up objects from the ground
    {
        RaycastHit hit; // Declare a RaycastHit variable to store information about what the ray hits
        Debug.DrawRay(cameraEye.position, Vector3.down, Color.clear, 10000); // Draw a debug ray downwards from the camera's position

        if (Physics.Raycast(cameraEye.position, Vector3.down, out hit)) // Perform a raycast downwards from the camera's position
        {
            Debug.Log("Hit Object: " + hit.collider.name);
            if (hit.collider.CompareTag("WorldObject")) // Check if the ray hit an object tagged as "WorldObject"
            {
                if (Input.GetKeyDown(KeyCode.R)) // Check if the "R" key is pressed
                {
                    // Get the parent of the hit object
                    Transform parentTransform = hit.collider.transform.parent;

                    // Check if the parent exists and has the ItemData component
                    if (parentTransform != null)
                    {
                        ItemData itemData = parentTransform.GetComponent<ItemData>(); // Get the ItemData component from the parent

                        if (itemData != null) // Check if the parent object has an ItemData component
                        {
                            ScriptableItems item = itemData.scriptableItems; // Get the ScriptableItems reference from the ItemData component
                            item.amountHeld += 1; // Increase the amount held for the item

                            Destroy(parentTransform.gameObject); // Destroy the parent object and all its children
                        }
                        else
                        {
                            Debug.LogWarning("No ItemData component found on parent object.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Object has no parent or ItemData on the parent.");
                    }
                }
            }
        }
    }
    public void ChangePreview() // Method to change the object preview
    {
        if (chosenObject != previewObject) // Check if the selected object is different from the current preview object
        {
            previewObject = chosenObject; // Update the preview object to the selected object

            if (currentPreviewObject != null) // If there is a current preview object
            {
                Destroy(currentPreviewObject); // Destroy the existing preview object
            }

            // Instantiate a new preview object
            currentPreviewObject = Instantiate(previewObject, objectPreviewPOS.position, Quaternion.identity);

            // Change the material for all child MeshRenderer components
            MeshRenderer[] childRenderers = currentPreviewObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in childRenderers)
            {
                // Handle multiple material elements (e.g., trunk and leaves)
                Material[] materials = renderer.materials;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = previewMaterial; // Assign the preview material to each element
                }
                renderer.materials = materials; // Reassign the updated materials array back to the renderer
            }

            // Handle LODGroups and change materials for LOD levels (trees or objects with LODs)
            LODGroup[] lodGroups = currentPreviewObject.GetComponentsInChildren<LODGroup>();
            foreach (LODGroup lodGroup in lodGroups)
            {
                LOD[] lods = lodGroup.GetLODs(); // Get all LOD levels
                foreach (LOD lod in lods)
                {
                    foreach (Renderer lodRenderer in lod.renderers) // Loop through renderers in each LOD
                    {
                        if (lodRenderer is MeshRenderer meshRenderer) // Ensure it's a MeshRenderer
                        {
                            // Apply the preview material to each material element of the LOD renderer
                            Material[] lodMaterials = meshRenderer.materials;
                            for (int i = 0; i < lodMaterials.Length; i++)
                            {
                                lodMaterials[i] = previewMaterial; // Assign the preview material
                            }
                            meshRenderer.materials = lodMaterials; // Reassign the updated materials array back to the LOD renderer
                        }
                    }
                }
            }

            // Disable colliders for the preview object
            MeshCollider[] childColliders = currentPreviewObject.GetComponentsInChildren<MeshCollider>();
            foreach (MeshCollider colliders in childColliders)
            {
                colliders.GetComponent<MeshCollider>().enabled = false; // Reassign the updated materials array back to the renderer
            }
        }
    }

    public void ObjectPreview() // Method to handle the object preview display
    {
        if (chosenObject != null && gameManager.buildON) // Check if an object is selected and building mode is enabled
        {
            ItemData itemData = chosenObject.GetComponent<ItemData>(); // Get the ItemData component from the chosen object

            if (itemData != null) // Check if the chosen object has an ItemData component
            {
                ScriptableItems item = itemData.scriptableItems; // Get the ScriptableItems reference from the ItemData component

                if (item.amountHeld > 0) // Check if the item is available
                {
                    ChangePreview(); // Call the ChangePreview method to update the object preview

                    if (currentPreviewObject == null) // If there is no current preview object
                    {
                        currentPreviewObject = Instantiate(previewObject, objectPreviewPOS.position, Quaternion.identity); // Instantiate a new preview object at the preview position

                        MeshRenderer[] childRenderers = currentPreviewObject.GetComponentsInChildren<MeshRenderer>();
                        foreach (MeshRenderer renderer in childRenderers)
                        {
                            // Handle multiple material elements (e.g., trunk and leaves)
                            Material[] materials = renderer.materials;
                            for (int i = 0; i < materials.Length; i++)
                            {
                                materials[i] = previewMaterial; // Assign the preview material to each element
                            }
                            renderer.materials = materials; // Reassign the updated materials array back to the renderer
                        }

                        // Handle LODGroups and change materials for LOD levels (trees or objects with LODs)
                        LODGroup[] lodGroups = currentPreviewObject.GetComponentsInChildren<LODGroup>();
                        foreach (LODGroup lodGroup in lodGroups)
                        {
                            LOD[] lods = lodGroup.GetLODs(); // Get all LOD levels
                            foreach (LOD lod in lods)
                            {
                                foreach (Renderer lodRenderer in lod.renderers) // Loop through renderers in each LOD
                                {
                                    if (lodRenderer is MeshRenderer meshRenderer) // Ensure it's a MeshRenderer
                                    {
                                        // Apply the preview material to each material element of the LOD renderer
                                        Material[] lodMaterials = meshRenderer.materials;
                                        for (int i = 0; i < lodMaterials.Length; i++)
                                        {
                                            lodMaterials[i] = previewMaterial; // Assign the preview material
                                        }
                                        meshRenderer.materials = lodMaterials; // Reassign the updated materials array back to the LOD renderer
                                    }
                                }
                            }
                        }

                        // Disable colliders for the preview object
                        MeshCollider[] childColliders = currentPreviewObject.GetComponentsInChildren<MeshCollider>();
                        foreach (MeshCollider colliders in childColliders)
                        {
                            colliders.GetComponent<MeshCollider>().enabled = false; // Reassign the updated materials array back to the renderer
                        }
                    }
                    else // If there is an existing preview object
                    {
                        currentPreviewObject.transform.position = objectPreviewPOS.position; // Update the preview object's position
                    }
                }
                else // If the item is not available
                {
                    RemovePreview(); // Call the RemovePreview method to remove the object preview
                }
            }
        }
        else if (!gameManager.buildON) // If building mode is not enabled
        {
            RemovePreview(); // Call the RemovePreview method to remove the object preview
        }
    }

    public void UpdatePreviewAfterReset() // Method to update the object preview after resetting the position
    {
        if (chosenObject != null && gameManager.buildON) // Check if an object is selected and building mode is enabled
        {
            if (currentPreviewObject != null) // If there is a current preview object
            {
                currentPreviewObject.transform.position = objectPreviewPOS.position; // Update the preview object's position
                currentPreviewObject.transform.rotation = Quaternion.identity; // Reset the preview object's rotation
            }
            else // If there is no current preview object
            {
                ObjectPreview(); // Call the ObjectPreview method to create a new preview object
            }
        }
    }

    public void RemovePreview() // Method to remove the object preview
    {
        if (currentPreviewObject != null) // If there is a current preview object
        {
            Destroy(currentPreviewObject); // Destroy the preview object
            currentPreviewObject = null; // Clear the reference to the preview object
        }
    }

    public void PreviewRotate()
    {
        if (currentPreviewObject != null) // If there is a current preview object
        {
            if (Input.GetKey(KeyCode.X)) //checks continuously for Q to be down
            {
                currentPreviewObject.transform.Rotate(0f, 30f * Time.deltaTime, 0f, Space.Self); //rotates preview and provides the BUILT object with its rotation
            }
            if (Input.GetKey(KeyCode.Z)) //checks continuously for Q to be down
            {
                currentPreviewObject.transform.Rotate(0f, -30f * Time.deltaTime, 0f, Space.Self); //rotates preview and provides the BUILT object with its rotation
            }
        }
    }

}
