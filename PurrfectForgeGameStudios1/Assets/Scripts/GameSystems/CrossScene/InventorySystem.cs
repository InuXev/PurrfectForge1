using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private List<ScriptableItems> playerInventory; // Keep it private
    public static InventorySystem Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Make sure the inventory persists across scenes
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
        foreach (var item in playerInventory)
        {
            Debug.Log(item);
        }
    }
}