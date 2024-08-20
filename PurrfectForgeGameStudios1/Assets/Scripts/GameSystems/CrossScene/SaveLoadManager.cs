using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    // Define the file name and directory
    public const string SaveDirectory = "/SaveData/";
    public const string FileName = "SaveGame.save";

    public void Save(PrefabList prefabList)
    {
        foreach (var item in prefabList.items)
        {
            Debug.Log($"Saving Prefab: {item.name}, Position: {item.position}, Rotation: {item.rotation}, ScriptableItemName: {item.scriptableItemName}, EshesBuildObjectName: {item.eshesBuildObjectName}");
        }

        // Ensure the directory exists
        string directoryPath = Application.persistentDataPath + SaveDirectory;
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Set the full path including the file name
        string savePath = Path.Combine(directoryPath, FileName);

        // Serialize the prefabList to a JSON string
        string json = JsonUtility.ToJson(prefabList, true);
        GUIUtility.systemCopyBuffer = savePath;

        // Write the JSON string to a file
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
        if (prefabList == null || prefabList.items.Count == 0)
        {
            Debug.LogWarning("Prefab list is empty or null. No prefabs to replace.");
            return;
        }

        foreach (var item in prefabList.items)
        {
            Debug.Log($"Attempting to load prefab: {item.eshesBuildObjectName}");

            // Load the prefab using the eshesBuildObjectName field
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/{item.eshesBuildObjectName}");
            if (prefab != null)
            {
                // Instantiate the prefab at the saved position and rotation
                GameObject instantiatedObject = Instantiate(prefab, item.position, item.rotation);
                Debug.Log($"Prefab instantiated: {item.eshesBuildObjectName} at {item.position}");
            }
            else
            {
                Debug.LogWarning($"Prefab not found in Resources: {item.eshesBuildObjectName}");
            }
        }
    }
}