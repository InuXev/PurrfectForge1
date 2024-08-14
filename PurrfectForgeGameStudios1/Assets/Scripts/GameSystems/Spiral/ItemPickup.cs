using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ScriptableItems item; // Ensure this is private and serialized to adjust from the inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Check if the collider is the player
        {
            Debug.Log(item.itemName + " Being Added");
            InventorySystem.Instance.AddToInventory(item);
            Destroy(gameObject); // Destroy the item after adding it to the inventory
        }
    }
}
