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
            if(item.type == "Orb")
            {
                if (item.itemName == "HealthOrb")
                {
                    PlayerManager.Instance.HP += PlayerManager.Instance.HPOriginal * .1F;
                }
                if (item.itemName == "StaminaOrb")
                {
                    PlayerManager.Instance.Stamina += PlayerManager.Instance.StaminaOriginal * .1F;
                }
            }
            if (item.type == "Essence")
            {
                Debug.Log(item.itemName + " Being Added");
                item.amountHeld += 1;
            }
            if (item.type == "InventoryItem")
            {
                InventorySystem inventorySystem = other.GetComponent<InventorySystem>();
                if (inventorySystem != null)
                {
                    inventorySystem.AddToInventory(item);
                }
            }
            Destroy(gameObject); // Destroy the item after adding it to the inventory
        }
    }
}
