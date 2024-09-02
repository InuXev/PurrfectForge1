using System.Collections; // Import the System.Collections namespace (not used in this script)
using System.Collections.Generic; // Import the System.Collections.Generic namespace for using lists
using UnityEngine; // Import the UnityEngine namespace for Unity-specific features like MonoBehaviour, GameObject, etc.

public class InventorySystem : MonoBehaviour // Define the InventorySystem class, inheriting from MonoBehaviour
{
    private List<ScriptableItems> playerInventory; // Declare a private list to store the player's inventory items
    public static InventorySystem Instance { get; private set; } // Singleton instance of InventorySystem with a private setter

    [SerializeField] GameManager gameManager; // Serialize the GameManager reference for assignment in the Unity Editor

    void Awake() // Awake is called when the script instance is being loaded
    {
        if (Instance == null) // Check if there is no existing instance of InventorySystem
        {
            Instance = this; // Assign this instance to the static Instance property
        }
        else // If an instance already exists
        {
            Debug.Log("Another instance of InventorySystem found, destroying this instance."); // Log a warning
            Destroy(gameObject); // Destroy this game object to enforce the singleton pattern
        }

        BuildInventory(); // Call BuildInventory to initialize the player's inventory
    }

    private void BuildInventory() // Method to initialize the inventory
    {
        playerInventory = new List<ScriptableItems>(); // Instantiate the playerInventory list
    }

    public void AddToInventory(ScriptableItems item) // Method to add an item to the inventory
    {
        if (!playerInventory.Contains(item)) // Check if the item is not already in the inventory
        {
            playerInventory.Add(item); // Add the item to the inventory
            item.amountHeld = 1; // Set the initial amount held for the item to 1
            Debug.Log(item.itemName + " New Item Added"); // Log that a new item was added
        }
        if (playerInventory.Contains(item)) // Check if the item is already in the inventory
        {
            item.amountHeld += 1; // Increment the amount held for that item
            Debug.Log(item.itemName + " Incremented"); // Log that the item's count was incremented
        }
    }

    public void RemoveFromInventory(ScriptableItems item) // Method to remove an item from the inventory
    {
        if (item.amountHeld > 0) // Check if the item is held in the inventory
        {
            item.amountHeld -= 1; // Decrement the amount held for that item
            Debug.Log(item.itemName + " Decremented"); // Log that the item's count was decremented
            if (item.amountHeld == 0) // If the amount held is now zero
            {
                playerInventory.Remove(item); // Remove the item from the inventory
            }
        }
    }

    public ScriptableItems GetScriptableItem(GameObject obj) // Method to retrieve an item from the inventory based on a GameObject
    {
        foreach (var item in playerInventory) // Iterate through all items in the inventory
        {
            if (item.eshesBuildObject == obj) // Check if the item's associated GameObject matches the given GameObject
            {
                return item; // Return the matching item
            }
        }
        return null; // Return null if no matching item is found
    }

    public void InventoryDisplay() // Method to display the inventory items
    {
        string itemList = string.Empty; // Initialize an empty string to build the item list

        if (playerInventory != null) // Check if the inventory list is not null
        {
            foreach (var item in playerInventory) // Iterate through each item in the inventory
            {
                if (item == null) // Check if the current item is null
                {
                    Debug.LogWarning("Found a null item in playerInventory."); // Log a warning for null items
                    continue; // Skip to the next item
                }

                if (item.itemName == null) // Check if the item's name is null
                {
                    Debug.LogWarning("Item name is null for one of the items."); // Log a warning for null item names
                    continue; // Skip to the next item
                }

                string stringAdder = item.itemName; // Assign the item's name to a temporary string
                if (itemList == string.Empty) // If the item list is currently empty
                {
                    itemList = stringAdder; // Set the item list to the current item's name
                }
                else // If the item list already has items
                {
                    itemList = itemList + ", " + stringAdder; // Append the current item's name to the list, separated by a comma
                }
            }
        }

        Debug.Log("Item List: " + itemList); // Log the complete item list

        if (GameManager.Instance != null && GameManager.Instance.inventoryList != null) // Check if the GameManager instance and its inventory list UI element exist
        {
            GameManager.Instance.inventoryList.text = itemList; // Update the inventory list UI with the item list
        }
    }
}
