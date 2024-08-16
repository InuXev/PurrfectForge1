using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private List<ScriptableItems> playerInventory; // Keep it private
    public static InventorySystem Instance { get; private set; }

    [SerializeField] GameManager gameManager;
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

        BuildInventory();
    }

    private void BuildInventory()
    {
        playerInventory = new List<ScriptableItems>(); // Initialize inventory
    }

    public void AddToInventory(ScriptableItems item)
    {
        if (!playerInventory.Contains(item))
        {
            playerInventory.Add(item);
            item.amountHeld = 1; // Set initial amount held when a new item is added
            Debug.Log(item.itemName + " New Item Added");
        }
        if (playerInventory.Contains(item))
        {
            item.amountHeld += 1;
            Debug.Log(item.itemName + " Incremented");
        }
    }

    public void RemoveFromInventory(ScriptableItems item)
    {
        if (item.amountHeld > 0)
        {
            item.amountHeld -= 1;
            Debug.Log(item.itemName + " Decremented");
            if (item.amountHeld == 0)
            {
                playerInventory.Remove(item);
            }
        }
    }

    public ScriptableItems GetScriptableItem(GameObject obj)
    {
        foreach (var item in playerInventory)
        {
            if (item.eshesBuildObject == obj)
            {
                return item;
            }
        }
        return null;
    }
    public void InventoryDisplay()
    {
        string itemList = string.Empty;
        if (playerInventory != null)
        {
            foreach (var item in playerInventory)
            {
                if (item == null)
                {
                    Debug.LogWarning("Found a null item in playerInventory.");
                    continue;
                }

                if (item.itemName == null)
                {
                    Debug.LogWarning("Item name is null for one of the items.");
                    continue;
                }
                string stringAdder = item.itemName;
                if (itemList == string.Empty)
                {
                    itemList = stringAdder;
                }
                else
                {
                    itemList = itemList + ", " + stringAdder; // Added comma for separation
                }
            }
        }
        Debug.Log("Item List: " + itemList);
        if (GameManager.Instance != null && GameManager.Instance.inventoryList != null)
        {
            GameManager.Instance.inventoryList.text = itemList;
        }

    }

}