using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
    #region Fields/Objects

    [SerializeField] GameManager gameManager;
    public Button targetButton;
    public string clickedButton;
    public int pageCount = 0;
    public int inquireIndex;
    #endregion

    #region Buttons
    public void resume() //unpause game from main menu
    {
        gameManager.stateUnPaused(); //call from GM
    }
    public void restart() //on death
    {
        //reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name.ToString()); //grab surrent scene
        gameManager.stateUnPaused();//call from GM
    }
    public void EteriusPort() //port the Eterius
    {
        PlayerManager.Instance.HasFloorKey = false;
        PlayerManager.Instance.HasBossKey = false;
        PlayerManager.Instance.inventorySystem.SaveInventoryToJson();
        PlayerManager.Instance.inventorySystem.SaveEquipmentToJson();
        gameManager.SaveGame();//call from GM
        SceneManager.LoadScene("Eshes"); //load eshes
        gameManager.stateUnPaused();//call from GM
    }
    public void InspectorOff()
    {
        GameManager.Instance.inspector.SetActive(false);
    }
    public void InspectorOn()
    {
        GameObject clickButton = EventSystem.current.currentSelectedGameObject;
        string clickedButton = clickButton.GetComponentInChildren<Transform>().name;
        int index = int.Parse(clickedButton) + pageCount;

        // Check if the slot is empty
        if (index < 0 || index >= PlayerManager.Instance.inventorySystem.playerInventory.Count ||
            PlayerManager.Instance.inventorySystem.playerInventory[index] == null)
        {
            return;
        }

        GameManager.Instance.stateUnPaused();
        GameManager.Instance.inspector.SetActive(true);
        GameManager.Instance.OnInspectButtonClick(PlayerManager.Instance.inventorySystem.playerInventory[index].itemName);
        GameManager.Instance.statePaused();
    }

    public void InspectorNext()
    {
        pageCount += 10;
        gameManager.inspectorPageCount += 10;

        if (pageCount > 40)
        {
            pageCount = 0;
            gameManager.inspectorPageCount = 0;
        }

        gameManager.InventoryUpdate();
    }

    public void InspectorBack()
    {
        pageCount -= 10;
        gameManager.inspectorPageCount -= 10;

        if (pageCount < 0)
        {
            pageCount = 40;
            gameManager.inspectorPageCount = 40;
        }

        gameManager.InventoryUpdate();
    }
    public void EquipCancel()
    {
        gameManager.confirmEquipMenu.SetActive(false);
    }
    public void EquipItem()
    {
        gameManager.confirmEquipMenu.SetActive(true);
        GameObject clickButton = EventSystem.current.currentSelectedGameObject;
        string clickedButton = clickButton.GetComponentInChildren<Transform>().name;

        // Try to parse the clickedButton name to an integer
        if (!int.TryParse(clickedButton, out this.inquireIndex))
        {
            Debug.LogError("Button name is not a valid integer: " + clickedButton);
            return;
        }
    }



    public void ConfirmEquipItem()
    {
        // Check if the slot is empty
        if (inquireIndex < 0 || inquireIndex >= PlayerManager.Instance.inventorySystem.playerInventory.Count ||
            PlayerManager.Instance.inventorySystem.playerInventory[inquireIndex] == null)
        {
            return;
        }

        // Get the item to be equipped
        ItemData itemToEquip = PlayerManager.Instance.inventorySystem.playerInventory[inquireIndex];

        // Add the item to equipment and check if it was successful
        if (PlayerManager.Instance.inventorySystem.AddToEquipment(itemToEquip))
        {
            // Remove the item from inventory
            PlayerManager.Instance.inventorySystem.playerInventory.RemoveAt(inquireIndex);
            Debug.Log(itemToEquip.itemName + " removed from inventory.");
            gameManager.updateEquipment();
        }
        else
        {
            Debug.LogWarning("Failed to equip item: " + itemToEquip.itemName);
        }

        // Update the inventory and close the confirm equip menu
        gameManager.InventoryUpdate();
        gameManager.confirmEquipMenu.SetActive(false);
        inquireIndex = 0;
    }


    public void FrontQuit() //main quit button
    {
        gameManager.quitConfirm();//call from GM
    }
    public void DeathQuit() //on death quit
    {
        gameManager.DeathQuitConfirm();//call from GM
    }
    public void DeathQuitCancel() //cancel death quit
    {
        gameManager.DeathQuitCancel();//call from GM
    }
    public void completeQuit() //after quit confirmation in pause menu
    {
        SceneManager.LoadScene("Eshes"); //load Eterius
    }
    public void EteriusPortConfirmCancel() //eterius port confirm cancel
    {
        gameManager.stateUnPaused();//call from GM
        gameManager.confirmMenu.SetActive(false);//call from GM turn off confirm menu
    }

    #endregion

    #region Skill Buttons


    #region Fire
    public void Fire1()
    {
        if (gameManager.TierOneCheck() != 2 && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() != "Ice" && gameManager.AssignedElementCheck() != "Lightning")
        {
            gameManager.AssignTierOne(1);
            gameManager.AssignElement("Fire");
            switch (gameManager.SkillTierOneLevelCheck())
            {
                case 0:
                    //unlock level to one
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 1:
                    //level up to two
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 2:
                    //level up to three
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                default:
                    break;
            }

        }
    }
    public void Fire2()
    {
        if (gameManager.TierOneCheck() != 1 && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() != "Ice" && gameManager.AssignedElementCheck() != "Lightning")
        {
            gameManager.AssignTierOne(2);
            gameManager.AssignElement("Fire");
            switch (gameManager.SkillTierOneLevelCheck())
            {
                case 0:
                    //unlock level to one
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 1:
                    //level up to two
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 2:
                    //level up to three
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                default:
                    break;
            }
        }
    }
    public void Fire3()
    {
        if (gameManager.SkillPointCheck() >= 1 && gameManager.TierTwoUnlocked() && gameManager.AssignedElementCheck() != "Ice" && gameManager.AssignedElementCheck() == "Fire")
        {
            gameManager.AssignTierTwo();
            switch (gameManager.SkillTierTwoLevelCheck())
            {
                case 0:
                    //unlock level to one
                    gameManager.SkillTwoLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 1:
                    //level up to two
                    gameManager.SkillTwoLevelUp();
                    gameManager.SkillPointUse();
                    break;
                default:
                    break;
            }
        }
    }
    public void Fire4()
    {
        if (gameManager.TierThreeCheck() != 2 && gameManager.TierThreeUnlocked() && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() == "Fire")
        {
            gameManager.AssignTierThree(1);
            gameManager.SkillPointUse();
        }
    }
    public void Fire5()
    {
        if (gameManager.TierThreeCheck() != 1 && gameManager.TierThreeUnlocked() && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() == "Fire")
        {
            gameManager.AssignTierThree(2);
            gameManager.SkillPointUse();
        }
    }
    #endregion

    #region Fire
    public void Ice1()
    {
        if (gameManager.TierOneCheck() != 2 && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() != "Fire" && gameManager.AssignedElementCheck() != "Lightning")
        {
            gameManager.AssignTierOne(1);
            gameManager.AssignElement("Ice");
            switch (gameManager.SkillTierOneLevelCheck())
            {
                case 0:
                    //unlock level to one
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 1:
                    //level up to two
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 2:
                    //level up to three
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                default:
                    break;
            }

        }
    }
    public void Ice2()
    {
        if (gameManager.TierOneCheck() != 1 && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() != "Fire" && gameManager.AssignedElementCheck() != "Lightning")
        {
            gameManager.AssignTierOne(2);
            gameManager.AssignElement("Ice");
            switch (gameManager.SkillTierOneLevelCheck())
            {
                case 0:
                    //unlock level to one
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 1:
                    //level up to two
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 2:
                    //level up to three
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                default:
                    break;
            }
        }
    }
    public void Ice3()
    {
        if (gameManager.SkillPointCheck() >= 1 && gameManager.TierTwoUnlocked() && gameManager.AssignedElementCheck() == "Ice")
        {
            gameManager.AssignTierTwo();
            switch (gameManager.SkillTierTwoLevelCheck())
            {
                case 0:
                    //unlock level to one
                    gameManager.SkillTwoLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 1:
                    //level up to two
                    gameManager.SkillTwoLevelUp();
                    gameManager.SkillPointUse();
                    break;
                default:
                    break;
            }
        }
    }
    public void Ice4()
    {
        if (gameManager.TierThreeCheck() != 2 && gameManager.TierThreeUnlocked() && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() == "Ice")
        {
            gameManager.AssignTierThree(1);
            gameManager.SkillPointUse();
        }
    }
    public void Ice5()
    {
        if (gameManager.TierThreeCheck() != 1 && gameManager.TierThreeUnlocked() && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() == "Ice")
        {
            gameManager.AssignTierThree(2);
            gameManager.SkillPointUse();
        }
    }
    #endregion
    #region Fire
    public void Lightning1()
    {
        if (gameManager.TierOneCheck() != 2 && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() != "Fire" && gameManager.AssignedElementCheck() != "Ice")
        {
            gameManager.AssignTierOne(1);
            gameManager.AssignElement("Lightning");
            switch (gameManager.SkillTierOneLevelCheck())
            {
                case 0:
                    //unlock level to one
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 1:
                    //level up to two
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 2:
                    //level up to three
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                default:
                    break;
            }

        }
    }
    public void Lightning2()
    {
        if (gameManager.TierOneCheck() != 1 && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() != "Fire" && gameManager.AssignedElementCheck() != "Ice")
        {
            gameManager.AssignTierOne(2);
            gameManager.AssignElement("Lighning");
            switch (gameManager.SkillTierOneLevelCheck())
            {
                case 0:
                    //unlock level to one
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 1:
                    //level up to two
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 2:
                    //level up to three
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                default:
                    break;
            }
        }
    }
    public void Lightning3()
    {
        if (gameManager.SkillPointCheck() >= 1 && gameManager.TierTwoUnlocked() && gameManager.AssignedElementCheck() == "Lightning")
        {
            gameManager.AssignTierTwo();
            switch (gameManager.SkillTierTwoLevelCheck())
            {
                case 0:
                    //unlock level to one
                    gameManager.SkillTwoLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 1:
                    //level up to two
                    gameManager.SkillTwoLevelUp();
                    gameManager.SkillPointUse();
                    break;
                default:
                    break;
            }
        }
    }
    public void Lightning4()
    {
        if (gameManager.TierThreeCheck() != 2 && gameManager.TierThreeUnlocked() && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() == "Lightning")
        {
            gameManager.AssignTierThree(1);
            gameManager.SkillPointUse();
        }
    }
    public void Lightning5()
    {
        if (gameManager.TierThreeCheck() != 1 && gameManager.TierThreeUnlocked() && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() == "Lightning")
        {
            gameManager.AssignTierThree(2);
            gameManager.SkillPointUse();
        }
    }
    #endregion
    #endregion
}
//PlayerManager.Instance.inventorySystem.AddToInventory(clickButton); //add the item to the list
//GameManager.Instance.InventoryUpdate();