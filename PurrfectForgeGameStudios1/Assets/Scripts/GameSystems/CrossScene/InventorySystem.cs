using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<ItemData> playerInventory; // Declare a list to store the player's inventory items
    public static InventorySystem Instance; // Singleton instance of InventorySystem
    public const string FileName = "InventorySave.save"; // Name of the save file
    [SerializeField] GameManager gameManager; // Serialize the GameManager reference for assignment in the Unity Editor

    [System.Serializable]
    public class InventoryDataWrapper
    {
        public List<ItemDataSerializable> items;
    }

    [System.Serializable]
    public class ItemDataSerializable
    {
        public string type;
        public string eshesBuildObjectName;
        public string description;
        public string itemName;
        //public int amountHeld;
        public float HpPow;
        public float AttPow;
        public float DefPow;
        public float DexPow;
        public float StaminaPow;
        public int value;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Another instance of InventorySystem found, destroying this instance.");
            Destroy(gameObject);
        }

        //BuildInventory();
    }

    //private void BuildInventory()
    //{
    //    playerInventory = new List<ItemData>();
    //}
    public void AddToInventory(GameObject itemObject)
    {
        Debug.Log("Adding item to inventory...");
        ItemData data = itemObject.GetComponent<ItemData>() ?? itemObject.GetComponentInChildren<ItemData>();

        if (data != null)
        {
            if (!playerInventory.Contains(data))
            {
                playerInventory.Add(data);
                data.amountHeld = 1;
                Debug.Log(data.itemName + " added as new item.");
            }
            else
            {
                data.amountHeld += 1;
                Debug.Log(data.itemName + " incremented. Amount: " + data.amountHeld);
            }

            gameManager.InventoryUpdate();
            Debug.Log("Inventory updated.");
        }
        else
        {
            Debug.LogError("ItemData not found.");
        }
    }

    public void RemoveFromInventory(ItemData item)
    {
        if (item.amountHeld > 0)
        {
            item.amountHeld -= 1;
            Debug.Log(item.itemName + " Decremented");
            if (item.amountHeld == 0)
            {
                playerInventory.Remove(item);
            }
            for (int i = 0; i < playerInventory.Count; i++)
            {
                Debug.Log(playerInventory[i].itemName);
            }
        }
    }

    public ItemData GetItemData(string name)
    {
        return playerInventory.Find(item => item.itemName == name);
    }

    public void SaveInventoryToJson()
    {
        string directoryPath = Application.persistentDataPath + SaveLoadManager.SaveDirectory;
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string savePath = Path.Combine(directoryPath, FileName);

        InventoryDataWrapper inventoryWrapper = new InventoryDataWrapper();
        inventoryWrapper.items = new List<ItemDataSerializable>();

        foreach (var item in playerInventory)
        {
            ItemDataSerializable serializableData = new ItemDataSerializable
            {
                type = item.type,
                eshesBuildObjectName = item.eshesBuildObjectName,
                description = item.description,
                itemName = item.itemName,
                //amountHeld = item.amountHeld,
                HpPow = item.HpPow,
                AttPow = item.AttPow,
                DefPow = item.DefPow,
                DexPow = item.DexPow,
                StaminaPow = item.StaminaPow,
                value = item.value
            };

            inventoryWrapper.items.Add(serializableData);
        }

        string json = JsonUtility.ToJson(inventoryWrapper, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Inventory saved to " + savePath);
        GUIUtility.systemCopyBuffer = savePath;
        Debug.Log("Save path copied to clipboard: " + savePath);
    }

    public void LoadInventoryFromJson()
    {
        string directoryPath = Application.persistentDataPath + SaveLoadManager.SaveDirectory;
        string savePath = Path.Combine(directoryPath, FileName);

        if (File.Exists(savePath))
        {
            Debug.Log("Loading inventory from: " + savePath);
            string json = File.ReadAllText(savePath, Encoding.UTF8);
            Debug.Log("JSON content: " + json);

            InventoryDataWrapper inventoryWrapper = JsonUtility.FromJson<InventoryDataWrapper>(json);
            if (inventoryWrapper == null || inventoryWrapper.items == null)
            {
                Debug.LogError("Failed to deserialize JSON or inventoryWrapper.items is null");
                return;
            }

            playerInventory.Clear();

            foreach (var data in inventoryWrapper.items)
            {
                GameObject itemObject = Resources.Load<GameObject>($"Weaponry/{data.itemName}");
                if (itemObject != null)
                {
                    ItemData itemData = itemObject.GetComponent<ItemData>() ?? itemObject.GetComponentInChildren<ItemData>();
                    if (itemData != null)
                    {
                        //itemData.type = data.type;
                        //itemData.eshesBuildObjectName = data.eshesBuildObjectName;
                        //itemData.description = data.description;
                        //itemData.itemName = data.itemName;
                        //itemData.amountHeld = data.amountHeld;
                        //itemData.HpPow = data.HpPow;
                        //itemData.AttPow = data.AttPow;
                        //itemData.DefPow = data.DefPow;
                        //itemData.DexPow = data.DexPow;
                        //itemData.StaminaPow = data.StaminaPow;
                        //itemData.value = data.value;

                        playerInventory.Add(itemData);
                    }
                }
                else
                {
                    Debug.LogWarning("GameObject not found in Resources: " + data.itemName);
                }
            }

            Debug.Log("Inventory loaded successfully. Items count: " + playerInventory.Count);
        }
    }
}
