using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabList
{
    public List<PrefabData> items = new List<PrefabData>();

    [System.Serializable]
    public class PrefabData
    {
        public string type;                   // The type of the prefab (optional for categorization)
        public string name;                   // The name or identifier of the prefab instance
        public Vector3 position;              // The position where the prefab should be instantiated
        public Quaternion rotation;           // The rotation of the instantiated prefab
        public string scriptableItemName;     // The name of the associated ScriptableObject (if any)
        public string eshesBuildObjectName;   // The path to the prefab within the Resources folder

        // Constructor to initialize PrefabData with all necessary information
        public PrefabData(string type, string name, Vector3 position, Quaternion rotation, string scriptableItemName, string eshesBuildObjectName)
        {
            this.type = type;
            this.name = name;
            this.position = position;
            this.rotation = rotation;
            this.scriptableItemName = scriptableItemName;
            this.eshesBuildObjectName = eshesBuildObjectName; // Ensure this path is correct relative to Resources folder
        }

        // Default constructor required for serialization
        public PrefabData() { }

        // Optional method to help set eshesBuildObjectName based on known paths
        public void SetEshesBuildObjectName(string folderName, string prefabName)
        {
            // This method can help ensure the path is consistent and correct
            this.eshesBuildObjectName = $"{folderName}/{prefabName}";
        }
    }
}
