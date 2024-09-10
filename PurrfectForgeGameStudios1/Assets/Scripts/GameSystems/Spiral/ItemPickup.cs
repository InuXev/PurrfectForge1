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
            if(item.type == "Orb") //if its an Orb type
            {
                if (item.itemName == "HealthOrb") //health orb
                {
                    float amountToIncrease = PlayerManager.Instance.HPOriginal * .1F;
                    if (PlayerManager.Instance.HP + amountToIncrease >= PlayerManager.Instance.HPOriginal)
                    {
                        PlayerManager.Instance.HP = PlayerManager.Instance.HPOriginal;
                    }
                    else
                    {
                        PlayerManager.Instance.HP += amountToIncrease; //increase HP by 10%
                    }

                }
                if (item.itemName == "StaminaOrb") //stamina orb
                {
                    float amountToIncrease = PlayerManager.Instance.StaminaOriginal * .1F;
                    if(PlayerManager.Instance.StaminaOriginal + amountToIncrease >= PlayerManager.Instance.StaminaOriginal)
                    {
                        PlayerManager.Instance.Stamina = PlayerManager.Instance.StaminaOriginal;
                    }
                    else
                    {
                        PlayerManager.Instance.Stamina += amountToIncrease; //increase stamina by 10%
                    }

                }
            }
            if (item.type == "Essence") //essence type
            {
                Debug.Log(item.itemName + " Being Added");
                item.amountHeld += 1; //increase its scriptable amountHeld by one
            }
            if (item.type == "InventoryItem") //Inventory Item 
            {
                InventorySystem inventorySystem = other.GetComponent<InventorySystem>(); //create a list
                if (inventorySystem != null) //if the invsys is not null
                {
                    inventorySystem.AddToInventory(item); //add the item to the list
                }
            }
            if (item.type == "Coin") //coin
            {
                Debug.Log("Coins being Added to purse");
                item.amountHeld += 1; //increase its scriptable amountHeld by one
            }
            Destroy(gameObject); // Destroy the item after adding it to the inventory
        }
    }
}
