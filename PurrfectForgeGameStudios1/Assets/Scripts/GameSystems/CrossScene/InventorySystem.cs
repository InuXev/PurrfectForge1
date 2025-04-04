using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{

    //MANAGES THE INVENTORY SYSTEMS//


    public List<ItemData> playerInventory; // Declare a list to store the player's inventory items
    public List<EquipmentSlot> playerEquipment; // Declare a list to store the player's inventory items
    public static InventorySystem Instance; // Singleton instance of InventorySystem
    public const string FileName = "InventorySave.save"; // Name of the save file
    public const string EquipmentFile = "EquipmentSave.save"; // Name of the save file
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
        public string slotType;
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
    [System.Serializable]
    public class EquipmentSlot
    {
        public string slotName;
        public ItemData itemData;

        public EquipmentSlot(string name)
        {
            slotName = name;
            itemData = null;
        }
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

        playerEquipment = new List<EquipmentSlot>
        {
            new EquipmentSlot("Head"),
            new EquipmentSlot("Chest"),
            new EquipmentSlot("Legs"),
            new EquipmentSlot("Feet"),
            new EquipmentSlot("Hands"),
            new EquipmentSlot("MainHand"),
            new EquipmentSlot("OffHand"),
            new EquipmentSlot("Accessory1"),
            new EquipmentSlot("Accessory2")
        };

    }
    public ItemData GetItemData(string name)
    {
        return playerInventory.Find(item => item.itemName == name);
    }
    public void AddToInventory(ItemData itemData)
    {
        Debug.Log("Adding item to inventory...");

        if (itemData != null)
        {
            if (!playerInventory.Contains(itemData))
            {
                playerInventory.Add(itemData);
                itemData.amountHeld = 1;
                Debug.Log(itemData.itemName + " added as new item.");
            }
            else
            {
                itemData.amountHeld += 1;
                Debug.Log(itemData.itemName + " incremented. Amount: " + itemData.amountHeld);
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
    public ItemData GetEquipData(string name)
    {
        foreach (var slot in playerEquipment)
        {
            if (slot.itemData != null && slot.itemData.itemName == name)
            {
                return slot.itemData;
            }
        }
        return null;
    }

    public bool AddToEquipment(ItemData itemData)
    {
        //Debug.Log("Current Equipment Slots:");
        //foreach (var slot in playerEquipment)
        //{
        //    if (slot.itemData != null)
        //    {
        //        Debug.Log(slot.slotName + ": " + slot.itemData.itemName);
        //    }
        //    else
        //    {
        //        Debug.Log(slot.slotName + ": Empty");
        //    }
        //}

        Debug.Log("Adding item to equipment...");

        if (itemData != null)
        {
            Debug.Log("ItemData is not null. Item: " + itemData.itemName + ", SlotType: " + itemData.slotType);

            // Find the appropriate slot for the item type
            foreach (var slot in playerEquipment)
            {
                Debug.Log("Checking slot: " + slot.slotName);

                if (slot.slotName == itemData.slotType)
                {
                    if (slot.itemData == null)
                    {
                        slot.itemData = itemData;
                        Debug.Log(itemData.itemName + " added to " + slot.slotName + " slot.");
                        PlayerManager.Instance.currentWeapon = itemData.itemGameObject;
                        gameManager.InventoryUpdate();
                        return true;
                    }
                    else
                    {
                        Debug.LogWarning("Slot " + slot.slotName + " is already occupied.");
                    }
                }
            }
        }
        else
        {
            Debug.LogError("ItemData not found.");
        }

        return false;
    }

    public void RemoveFromEquipment(ItemData item)
    {
        foreach (var slot in playerEquipment)
        {
            if (slot.itemData == item)
            {
                slot.itemData = null;
                Debug.Log(item.itemName + " removed from " + slot.slotName + " slot.");
                break;
            }
        }
    }

    public void SaveEquipmentToJson()
    {
        string directoryPath = Application.persistentDataPath + SaveLoadManager.SaveDirectory;
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string savePath = Path.Combine(directoryPath, EquipmentFile);

        InventoryDataWrapper inventoryWrapper = new InventoryDataWrapper();
        inventoryWrapper.items = new List<ItemDataSerializable>();

        foreach (var slot in playerEquipment)
        {
            if (slot.itemData != null)
            {
                ItemDataSerializable serializableData = new ItemDataSerializable
                {
                    type = slot.itemData.type,
                    eshesBuildObjectName = slot.itemData.eshesBuildObjectName,
                    description = slot.itemData.description,
                    itemName = slot.itemData.itemName,
                    HpPow = slot.itemData.HpPow,
                    AttPow = slot.itemData.AttPow,
                    DefPow = slot.itemData.DefPow,
                    DexPow = slot.itemData.DexPow,
                    StaminaPow = slot.itemData.StaminaPow,
                    value = slot.itemData.value
                };

                inventoryWrapper.items.Add(serializableData);
            }
        }

        string json = JsonUtility.ToJson(inventoryWrapper, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Equipment saved to " + savePath);
        GUIUtility.systemCopyBuffer = savePath;
        Debug.Log("Save path copied to clipboard: " + savePath);
    }

    public void LoadEquipmentFromJson()
    {
        string directoryPath = Application.persistentDataPath + SaveLoadManager.SaveDirectory;
        string savePath = Path.Combine(directoryPath, EquipmentFile);

        if (File.Exists(savePath))
        {
            Debug.Log("Loading equipment from: " + savePath);
            string json = File.ReadAllText(savePath, Encoding.UTF8);
            Debug.Log("JSON content: " + json);

            InventoryDataWrapper inventoryWrapper = JsonUtility.FromJson<InventoryDataWrapper>(json);
            if (inventoryWrapper == null || inventoryWrapper.items == null)
            {
                Debug.LogError("Failed to deserialize JSON or inventoryWrapper.items is null");
                return;
            }

            // Clear current equipment slots
            foreach (var slot in playerEquipment)
            {
                slot.itemData = null;
            }

            // Load equipment from JSON
            foreach (var data in inventoryWrapper.items)
            {
                GameObject itemObject = Resources.Load<GameObject>($"Weaponry/{data.itemName}");
                if (itemObject != null)
                {
                    ItemData itemData = itemObject.GetComponent<ItemData>() ?? itemObject.GetComponentInChildren<ItemData>();
                    if (itemData != null)
                    {
                        // Find the appropriate slot for the item type
                        foreach (var slot in playerEquipment)
                        {
                            if (slot.slotName == data.slotType && slot.itemData == null)
                            {
                                slot.itemData = itemData;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("GameObject not found in Resources: " + data.itemName);
                }
            }

            Debug.Log("Equipment loaded successfully.");
        }
        else
        {
            Debug.LogWarning("Save file not found: " + savePath);
        }
    }

}
