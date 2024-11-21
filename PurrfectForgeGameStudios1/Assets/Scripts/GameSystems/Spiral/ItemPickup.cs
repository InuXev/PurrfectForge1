using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] public ScriptableItems item; // Ensure this is private and serialized to adjust from the inspector
    [SerializeField] public ItemData itemData;
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
            if (item.type == "Coin") //coin
            {
                Debug.Log("Coins being Added to purse");
                PlayerManager.Instance.playerCoin += itemData.value;
            }
            if (item.type == "Weapon") //coin
            {
                Debug.Log("PickUp Weapon");
                ItemData itemData = gameObject.GetComponent<ItemData>() ?? gameObject.GetComponentInChildren<ItemData>();
                if (itemData != null)
                {
                    PlayerManager.Instance.inventorySystem.AddToInventory(itemData); // Add the item to the list
                }
                else
                {
                    Debug.LogError("ItemData not found on the GameObject.");
                }
            }
            if (transform.parent != null)
            {
                transform.parent.gameObject.SetActive(false); // Deactivate the parent object
            }
            else
            {
                gameObject.SetActive(false); // Destroy the item after adding it to the inventory
            }
        }
    }
}
