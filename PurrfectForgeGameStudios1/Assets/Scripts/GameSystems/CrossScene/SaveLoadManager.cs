using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static PrefabList;

public class SaveLoadManager : MonoBehaviour
{
    // Define the file name and directory
    public const string SaveDirectory = "/SaveData/";
    public const string FileName = "SaveGame.save";

    public void Save(PrefabList prefabList)
    {
        // Ensure the directory exists
        string directoryPath = Application.persistentDataPath + SaveDirectory;
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Set the full path including the file name
        string savePath = Path.Combine(directoryPath, FileName);

        List<PrefabList.PrefabData> prefabDataList = new List<PrefabList.PrefabData>();

        // Load existing data if the file exists
        if (File.Exists(savePath))
        {
            string existingJson = File.ReadAllText(savePath);
            PrefabList existingPrefabList = JsonUtility.FromJson<PrefabList>(existingJson);

            // Add existing data to the list
            prefabDataList.AddRange(existingPrefabList.items);
        }

        // Add new data to the list
        prefabDataList.AddRange(prefabList.items);

        // Create a new PrefabList with the combined data
        PrefabList combinedPrefabList = new PrefabList();
        combinedPrefabList.items = prefabDataList;

        // Serialize the combinedPrefabList to a JSON string
        string json = JsonUtility.ToJson(combinedPrefabList, true);
        GUIUtility.systemCopyBuffer = savePath;

        // Write the JSON string to a file
        File.WriteAllText(savePath, json);

        Debug.Log("Data appended and saved to " + savePath);
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
            Debug.Log("Loaded JSON: " + json); // Log loaded JSON to verify

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
            Debug.LogWarning("Prefab list is empty. No prefabs to replace.");
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
    //public void ReplacePrefabs(PrefabList prefabList)
    //{
    //    if (prefabList == null || prefabList.items.Count == 0)
    //    {
    //        Debug.LogWarning("Prefab list is empty or null. No prefabs to replace.");
    //        return;
    //    }

    //    // Optionally clear existing prefabs before adding new ones
    //    ClearAllInstantiatedPrefabs();

    //    Debug.Log($"Replacing prefabs with {prefabList.items.Count} items.");

    //    // Create a new list to track instantiated prefabs
    //    List<PrefabData> instantiatedPrefabs = new List<PrefabData>();

    //    foreach (var item in prefabList.items)
    //    {
    //        Debug.Log($"Attempting to load prefab: {item.eshesBuildObjectName}");

    //        // Load the prefab using the eshesBuildObjectName field
    //        GameObject prefab = Resources.Load<GameObject>($"Prefabs/{item.eshesBuildObjectName}");
    //        if (prefab != null)
    //        {
    //            // Instantiate the prefab at the saved position and rotation
    //            GameObject instantiatedObject = Instantiate(prefab, item.position, item.rotation);
    //            instantiatedObject.name = $"Prefab_{item.name}"; // Optional: Set a unique name for management
    //            Debug.Log($"Prefab instantiated: {item.eshesBuildObjectName} at {item.position}");

    //            // Create a new PrefabData to track the instantiated prefab
    //            PrefabData newPrefabData = new PrefabData(
    //                type: item.type,
    //                name: instantiatedObject.name,
    //                position: instantiatedObject.transform.position,
    //                rotation: instantiatedObject.transform.rotation,
    //                scriptableItemName: item.scriptableItemName,
    //                eshesBuildObjectName: item.eshesBuildObjectName
    //            );

    //            // Add the new PrefabData to the list
    //            instantiatedPrefabs.Add(newPrefabData);
    //        }
    //        else
    //        {
    //            Debug.LogWarning($"Prefab not found in Resources: {item.eshesBuildObjectName}");
    //        }
    //    }

    //    // Update the prefabList with the newly instantiated prefabs
    //    prefabList.items = instantiatedPrefabs;

    //    Debug.Log("Prefab list updated with new instances.");
    //}


    // Optional: Method to clear all previously instantiated prefabs
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