using System.Collections.Generic; // Import necessary libraries for working with lists
using System.IO; // Import necessary libraries for file input/output operations
using UnityEngine; // Import Unity engine functionalities
using static PrefabList; // Allow direct access to members of PrefabList without needing to qualify it with the type name
using static UnityEditor.Progress; // (Unused) Might be intended for progress display in Unity's editor

public class SaveLoadManager : MonoBehaviour // Define a class inheriting from MonoBehaviour to manage save/load functionality
{
    // Define the file name and directory
    public const string SaveDirectory = "/SaveData/"; // Directory path where save data will be stored
    public const string FileName = "SaveGame.save"; // Name of the save file

    public void SaveEshesWorld() // Method to save the game world
    {
        // Ensure the directory exists
        string directoryPath = Application.persistentDataPath + SaveDirectory; // Full path to the save directory
        if (!Directory.Exists(directoryPath)) // Check if the directory exists
        {
            Directory.CreateDirectory(directoryPath); // Create the directory if it doesn't exist
        }

        // Set the full path including the file name
        string savePath = Path.Combine(directoryPath, FileName); // Combine directory path and file name

        List<PrefabData> prefabDataList = new List<PrefabData>(); // Create a list to hold prefab data

        // Find all objects with the tag "WorldObject"
        GameObject[] eshesObjects = GameObject.FindGameObjectsWithTag("WorldObject"); // Find all game objects with the specified tag

        // Collect data from each object
        foreach (GameObject obj in eshesObjects) // Loop through each object found
        {
            // Get the ItemData component attached to the GameObject
            ItemData itemData = obj.GetComponent<ItemData>(); // Try to get the ItemData component

            if (itemData != null) // Proceed if the component exists
            {
                ScriptableItems item = itemData.scriptableItems; // Get the scriptableItems from ItemData

                // Retrieve data from ScriptableItems if available
                string type = item != null ? item.type : "Unknown"; // Get type or default to "Unknown"
                string scriptableItemName = item != null ? item.itemName : "Unknown"; // Get item name or default to "Unknown"
                string eshesBuildObjectName = item != null && item.eshesBuildObject != null ? item.eshesBuildObject.name : "Unknown"; // Get object name or default to "Unknown"

                // Create PrefabData instance using values from ItemData and ScriptableItems
                PrefabData data = new PrefabData(
                    type: type, // Set type field
                    name: obj.name, // Set object name field
                    position: obj.transform.position, // Set object position
                    rotation: obj.transform.rotation, // Set object rotation
                    scriptableItemName: scriptableItemName, // Set scriptable item name
                    eshesBuildObjectName: eshesBuildObjectName // Set eshes build object name
                );

                prefabDataList.Add(data); // Add the created data to the list
            }
            //else
            //{
            //    Debug.LogWarning("ItemData component missing on GameObject: " + obj.name); // Log warning if ItemData is missing
            //}
        }

        // Create a new PrefabList with the collected data
        PrefabList newPrefabList = new PrefabList(); // Create an instance of PrefabList
        newPrefabList.items = prefabDataList; // Assign collected data to the items list

        // Serialize the newPrefabList to a JSON string
        string json = JsonUtility.ToJson(newPrefabList, true); // Convert PrefabList to JSON

        // Write the JSON string to a file, overwriting old data
        File.WriteAllText(savePath, json); // Write JSON to file

        Debug.Log("Data saved to " + savePath); // Log the save path
    }

    public PrefabList Load() // Method to load the saved game data
    {
        // Set the full path including the file name
        string fullPath = Path.Combine(Application.persistentDataPath + SaveDirectory, FileName); // Combine directory path and file name

        // Check if the file exists
        if (File.Exists(fullPath)) // Verify if the file exists
        {
            // Read the JSON string from the file
            string json = File.ReadAllText(fullPath); // Read the file content as a JSON string
            Debug.Log("Loaded Game"); // Log that the game has been loaded

            // Deserialize the JSON string back into a PrefabList object
            PrefabList prefabList = JsonUtility.FromJson<PrefabList>(json); // Convert JSON string back to PrefabList

            // Return the loaded PrefabList
            return prefabList; // Return the deserialized PrefabList
        }
        else
        {
            Debug.LogError("Save file not found at " + fullPath); // Log an error if the file doesn't exist
            return null; // Return null if loading fails
        }
    }

    public void ReplacePrefabs(PrefabList prefabList) // Method to replace prefabs with those from a saved list
    {
        if (prefabList == null) // Check if the prefab list is null
        {
            Debug.LogError("PrefabList is null. Cannot replace prefabs."); // Log an error if null
            return; // Exit the method
        }

        if (prefabList.items == null) // Check if the items list is null
        {
            Debug.LogError("PrefabList items are null. Cannot replace prefabs."); // Log an error if null
            return; // Exit the method
        }

        if (prefabList.items.Count == 0) // Check if the list is empty
        {
            return; // Exit the method without doing anything
        }

        // Optionally clear existing prefabs before adding new ones
        ClearAllInstantiatedPrefabs(); // Clear any existing prefabs

        Debug.Log($"Replacing prefabs with {prefabList.items.Count} items."); // Log the number of prefabs being replaced

        // Create a new list to track instantiated prefabs
        List<PrefabData> instantiatedPrefabs = new List<PrefabData>(); // Create a list to store newly instantiated prefabs

        foreach (var item in prefabList.items) // Loop through each item in the saved prefab list
        {
            Debug.Log($"Attempting to load prefab: {item.eshesBuildObjectName}"); // Log which prefab is being attempted to load

            // Load the prefab using the eshesBuildObjectName field
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/{item.eshesBuildObjectName}"); // Load the prefab from Resources
            if (prefab != null) // Check if the prefab was loaded successfully
            {
                // Instantiate the prefab at the saved position and rotation
                GameObject instantiatedObject = Instantiate(prefab, item.position, item.rotation); // Instantiate the prefab at the saved location
                instantiatedObject.name = $"Prefab_{item.name}"; // Optional: Set a unique name for management
                Debug.Log($"Prefab instantiated: {item.eshesBuildObjectName} at {item.position}"); // Log the instantiation

                // Create a new PrefabData to track the instantiated prefab
                PrefabData newPrefabData = new PrefabData(
                    type: item.type, // Set type
                    name: instantiatedObject.name, // Set the instantiated object's name
                    position: instantiatedObject.transform.position, // Set the position
                    rotation: instantiatedObject.transform.rotation, // Set the rotation
                    scriptableItemName: item.scriptableItemName, // Set the scriptable item name
                    eshesBuildObjectName: item.eshesBuildObjectName // Set the eshes build object name
                );

                // Add the new PrefabData to the list
                instantiatedPrefabs.Add(newPrefabData); // Add the newly created prefab data to the list
            }
            else
            {
                Debug.LogWarning($"Prefab not found in Resources: {item.eshesBuildObjectName}"); // Log a warning if prefab not found
            }
        }

        // Update the prefabList with the newly instantiated prefabs
        prefabList.items = instantiatedPrefabs; // Replace the original list with the newly instantiated list

        Debug.Log("Prefab list updated with new instances."); // Log that the prefab list has been updated
    }

    void ClearAllInstantiatedPrefabs() // Method to clear all instantiated prefabs
    {
        var instantiatedPrefabs = GameObject.FindObjectsOfType<GameObject>(); // Find all game objects in the scene
        foreach (var prefab in instantiatedPrefabs) // Loop through each game object
        {
            if (prefab.name.StartsWith("Prefab_")) // Use a naming convention or other criteria to identify
            {
                Destroy(prefab); // Destroy the identified prefab
                Debug.Log($"Destroyed prefab: {prefab.name}"); // Log that the prefab was destroyed
            }
        }
    }

    public void ClearSaveData() // Method to clear the save data
    {
        // Ensure the directory exists
        string directoryPath = Application.persistentDataPath + SaveDirectory; // Full path to the save directory
        if (!Directory.Exists(directoryPath)) // Check if the directory exists
        {
            Directory.CreateDirectory(directoryPath); // Create the directory if it doesn't exist
        }

        // Set the full path including the file name
        string savePath = Path.Combine(directoryPath, FileName); // Combine directory path and file name

        // Create an empty PrefabList
        PrefabList emptyPrefabList = new PrefabList(); // Create an empty instance of PrefabList
        emptyPrefabList.items = new List<PrefabList.PrefabData>(); // Initialize an empty list of PrefabData

        // Serialize the empty list to a JSON string
        string emptyJson = JsonUtility.ToJson(emptyPrefabList, true); // Convert empty PrefabList to JSON

        // Write the empty JSON string to the file (overwrites the file)
        File.WriteAllText(savePath, emptyJson); // Write the empty JSON to the save file

        Debug.Log("Save data cleared."); // Log that the save data was cleared
    }
}
