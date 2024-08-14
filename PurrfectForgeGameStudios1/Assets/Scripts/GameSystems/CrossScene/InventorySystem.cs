using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private List<ScriptableItems> playerInventory; // Better to keep it private
    public static InventorySystem Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Make sure the inventory persists across scenes
        }
        else
        {
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
        else
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
            if (item.amountHeld == 0)
            {
                playerInventory.Remove(item);
            }
        }
    }
}