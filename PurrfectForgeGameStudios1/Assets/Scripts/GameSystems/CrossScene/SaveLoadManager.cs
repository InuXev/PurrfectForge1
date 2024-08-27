using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static PrefabList;
using static UnityEditor.Progress;

public class SaveLoadManager : MonoBehaviour
{
    // Define the file name and directory
    public const string SaveDirectory = "/SaveData/";
    public const string FileName = "SaveGame.save";

    public void SaveEshesWorld()
    {
        // Ensure the directory exists
        string directoryPath = Application.persistentDataPath + SaveDirectory;
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Set the full path including the file name
        string savePath = Path.Combine(directoryPath, FileName);

        List<PrefabData> prefabDataList = new List<PrefabData>();

        // Find all objects with the tag "WorldObject"
        GameObject[] eshesObjects = GameObject.FindGameObjectsWithTag("WorldObject"); // Adjust the tag as needed

        // Collect data from each object
        foreach (GameObject obj in eshesObjects)
        {
            // Get the ItemData component attached to the GameObject
            ItemData itemData = obj.GetComponent<ItemData>();

            if (itemData != null)
            {
                ScriptableItems item = itemData.scriptableItems;

                // Retrieve data from ScriptableItems if available
                string type = item != null ? item.type : "Unknown"; // Adjust based on actual fields
                string scriptableItemName = item != null ? item.itemName : "Unknown"; // Adjust based on actual fields
                string eshesBuildObjectName = item != null && item.eshesBuildObject != null ? item.eshesBuildObject.name : "Unknown"; // Adjust based on actual fields

                // Create PrefabData instance using values from ItemData and ScriptableItems
                PrefabData data = new PrefabData(
                    type: type,
                    name: obj.name,
                    position: obj.transform.position,
                    rotation: obj.transform.rotation,
                    scriptableItemName: scriptableItemName,
                    eshesBuildObjectName: eshesBuildObjectName
                );

                prefabDataList.Add(data);
            }
            else
            {
                Debug.LogWarning("ItemData component missing on GameObject: " + obj.name);
            }
        }

        // Create a new PrefabList with the collected data
        PrefabList newPrefabList = new PrefabList();
        newPrefabList.items = prefabDataList;

        // Serialize the newPrefabList to a JSON string
        string json = JsonUtility.ToJson(newPrefabList, true);

        // Write the JSON string to a file, overwriting old data
        File.WriteAllText(savePath, json);

        Debug.Log("Data saved to " + savePath);
    }
    public PrefabList Load()
    {
        // Set the full path including the file name
        string fullPath = Path.Combine(Application.persistentDataPath + SaveDirectory, FileName);

        // Check if the file exists
        if (File.Exists(fullPath))
        {
            // Read the JSON string from the file
            string json = File.ReadAllText(fullPath);
            Debug.Log("Loaded Game");

            // Deserialize the JSON string back into a PrefabList object
            PrefabList prefabList = JsonUtility.FromJson<PrefabList>(json);

            // Return the loaded PrefabList
            return prefabList;
        }
        else
        {
            Debug.LogError("Save file not found at " + fullPath);
            return null;
        }
    }
    public void ReplacePrefabs(PrefabList prefabList)
    {
        if (prefabList == null)
        {
            Debug.LogError("PrefabList is null. Cannot replace prefabs.");
            return;
        }

        if (prefabList.items == null)
        {
            Debug.LogError("PrefabList items are null. Cannot replace prefabs.");
            return;
        }

        if (prefabList.items.Count == 0)
        {
            return;
        }

        // Optionally clear existing prefabs before adding new ones
        ClearAllInstantiatedPrefabs();

        Debug.Log($"Replacing prefabs with {prefabList.items.Count} items.");

        // Create a new list to track instantiated prefabs
        List<PrefabData> instantiatedPrefabs = new List<PrefabData>();

        foreach (var item in prefabList.items)
        {
            Debug.Log($"Attempting to load prefab: {item.eshesBuildObjectName}");

            // Load the prefab using the eshesBuildObjectName field
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/{item.eshesBuildObjectName}");
            if (prefab != null)
            {
                // Instantiate the prefab at the saved position and rotation
                GameObject instantiatedObject = Instantiate(prefab, item.position, item.rotation);
                instantiatedObject.name = $"Prefab_{item.name}"; // Optional: Set a unique name for management
                Debug.Log($"Prefab instantiated: {item.eshesBuildObjectName} at {item.position}");

                // Create a new PrefabData to track the instantiated prefab
                PrefabData newPrefabData = new PrefabData(
                    type: item.type,
                    name: instantiatedObject.name,
                    position: instantiatedObject.transform.position,
                    rotation: instantiatedObject.transform.rotation,
                    scriptableItemName: item.scriptableItemName,
                    eshesBuildObjectName: item.eshesBuildObjectName
                );

                // Add the new PrefabData to the list
                instantiatedPrefabs.Add(newPrefabData);
            }
            else
            {
                Debug.LogWarning($"Prefab not found in Resources: {item.eshesBuildObjectName}");
            }
        }

        // Update the prefabList with the newly instantiated prefabs
        prefabList.items = instantiatedPrefabs;

        Debug.Log("Prefab list updated with new instances.");
    }
    void ClearAllInstantiatedPrefabs()
    {
        var instantiatedPrefabs = GameObject.FindObjectsOfType<GameObject>();
        foreach (var prefab in instantiatedPrefabs)
        {
            if (prefab.name.StartsWith("Prefab_")) // Use a naming convention or other criteria to identify
            {
                Destroy(prefab);
                Debug.Log($"Destroyed prefab: {prefab.name}");
            }
        }
    }
    public void ClearSaveData()
    {
        // Ensure the directory exists
        string directoryPath = Application.persistentDataPath + SaveDirectory;
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Set the full path including the file name
        string savePath = Path.Combine(directoryPath, FileName);

        // Create an empty PrefabList
        PrefabList emptyPrefabList = new PrefabList();
        emptyPrefabList.items = new List<PrefabList.PrefabData>();

        // Serialize the empty list to a JSON string
        string emptyJson = JsonUtility.ToJson(emptyPrefabList, true);

        // Write the empty JSON string to the file (overwrites the file)
        File.WriteAllText(savePath, emptyJson);

        Debug.Log("Save data cleared.");
    }
}