using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
//using UnityEditor.Experimental.GraphView;
using System.Linq;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{

    #region Fields/Objects
    [Header("Menus")]
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public GameObject loseMenu;
    [SerializeField] public GameObject playerHitFlash;
    [SerializeField] public GameObject statMenu;
    [SerializeField] public GameObject inventoryMenu;
    [SerializeField] public GameObject confirmMenu;
    [SerializeField] public GameObject confirmEquipMenu;
    [SerializeField] public TMP_Text inventoryList;
    [SerializeField] InventorySystem inventorySystem;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] GameObject playerDeathQuitConfirm;
    [SerializeField] public GameObject inspector;

    [Header("Basic Information")]
    public Image playerHP;
    public Image playerHPBar;
    public TMP_Text playerHPText;
    public TMP_Text playerLvLText;
    public TMP_Text playerHPStat;
    public TMP_Text playerAttStat;
    public TMP_Text playerDefStat;
    public TMP_Text playerDexStat;
    public TMP_Text playerStamStat;
    public TMP_Text playerAvailableSkillPts;
    public GameObject levelUpNote;
    public TMP_Text playerCoins;
    public Image playerXP;
    public Image playerXPBar;
    public Image playerStam;
    public Image playerStamBar;
    public static GameManager Instance { get; private set; }
    public GameObject activeMenu;
    public bool isPaused;
    int spiralLevel;

    public int inspectorPageCount = 0;

    [Header("Skill Information")]
    [SerializeField] public GameObject FireNotAvailable;
    [SerializeField] public GameObject IceNotAvailable;
    [SerializeField] public GameObject LightningNotAvailable;
    [SerializeField] public GameObject FireLockTierTwo;
    [SerializeField] public GameObject FireLockTierThree;
    [SerializeField] public GameObject IceLockTierTwo;
    [SerializeField] public GameObject IceLockTierThree;
    [SerializeField] public GameObject LightningLockTierTwo;
    [SerializeField] public GameObject LightningLockTierThree;
    [SerializeField] public Image SkillSlotOneImage;
    [SerializeField] public Image SkillSlotTwoImage;
    [SerializeField] public Image SkillSlotThreeImage;
    [SerializeField] public GameObject InteractableTag;


    [Header("Inventory Information")]
    [SerializeField] public TMP_Text InventorySlotOne;
    [SerializeField] public TMP_Text InventorySlotTwo;
    [SerializeField] public TMP_Text InventorySlotThree;
    [SerializeField] public TMP_Text InventorySlotFour;
    [SerializeField] public TMP_Text InventorySlotFive;
    [SerializeField] public TMP_Text InventorySlotSix;
    [SerializeField] public TMP_Text InventorySlotSeven;
    [SerializeField] public TMP_Text InventorySlotEight;
    [SerializeField] public TMP_Text InventorySlotNine;
    [SerializeField] public TMP_Text InventorySlotTen;
    [SerializeField] public TMP_Text InventorySlotOneCount;
    [SerializeField] public TMP_Text InventorySlotTwoCount;
    [SerializeField] public TMP_Text InventorySlotThreeCount;
    [SerializeField] public TMP_Text InventorySlotFourCount;
    [SerializeField] public TMP_Text InventorySlotFiveCount;
    [SerializeField] public TMP_Text InventorySlotSixCount;
    [SerializeField] public TMP_Text InventorySlotSevenCount;
    [SerializeField] public TMP_Text InventorySlotEightCount;
    [SerializeField] public TMP_Text InventorySlotNineCount;
    [SerializeField] public TMP_Text InventorySlotTenCount;
    [SerializeField] public TMP_Text InspectDescription;
    [SerializeField] public TMP_Text InspectedName;
    [SerializeField] public TMP_Text InspectedHPStat;
    [SerializeField] public TMP_Text InspectedAttStat;
    [SerializeField] public TMP_Text InspectedDefStat;
    [SerializeField] public TMP_Text InspectedDexStat;
    [SerializeField] public TMP_Text InspectedStamStat;
    [SerializeField] public TMP_Text InspectedValue;

    [Header("Inventory Information")]
    [SerializeField] public TMP_Text inHeadSlot;
    [SerializeField] public TMP_Text inChestSlot;
    [SerializeField] public TMP_Text inHandSlot;
    [SerializeField] public TMP_Text inLegSlot;
    [SerializeField] public TMP_Text inFootSlot;
    [SerializeField] public TMP_Text MainHand;
    [SerializeField] public TMP_Text inOffHandSlot;
    [SerializeField] public TMP_Text inAccOneSlot;
    [SerializeField] public TMP_Text inAccTwoSlot;

    #endregion

    #region Processes
    void Awake()
    {
        if (Instance == null) //instance GM is null
        {
            Instance = this; //this GM
        }
        else
        {
            Destroy(gameObject); //Destroy the old one
        }
    }
    void Start()
    {
        if (inventoryList == null) //check for inventory
        {
            Debug.LogError("InventoryList is not assigned!");
        }
        if (SceneManager.GetActiveScene().name == "Spiral") //if im in the spiral
        {
            Cursor.visible = false; //cursor off
        }
        inventorySystem.LoadInventoryFromJson();
        InventoryUpdate();
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape)) //pause menu
        {
            if (activeMenu == null) //if active menu null
            {
                statePaused(); //call paused
                activeMenu = pauseMenu; //set menu
                pauseMenu.SetActive(true); //turn on menu
            }
            else if (activeMenu != null) //if a menu is up
            {
                stateUnPaused(); //unpause the game
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab)) //player stat window on tab
        {
            if (activeMenu == null) //no window active
            {
                statePaused(); //pause
                activeMenu = statMenu; //asign stat to active
                statMenu.SetActive(true); //turn it on
            }
            else if (activeMenu != null) //is something is in active on tab
            {
                stateUnPaused(); //unpause
            }
        }
        if (Input.GetKeyDown(KeyCode.I)) //invnetory window
        {
            if (activeMenu == null) //no active menu
            {
                statePaused(); //pause
                activeMenu = inventoryMenu; //inv menu active
                activeMenu.SetActive(true); //turn it on
                InventoryUpdate();
            }
            else if (activeMenu != null) //if something in active
            {
                activeMenu.SetActive(false);
                inspector.SetActive(false);
                stateUnPaused(); //state unpaused
            }
        }

        if (activeMenu == null)
        {
            Cursor.visible = false;
        }
        if (playerManager.chosenElement == null)
        {
            IceNotAvailable.SetActive(false);
            FireNotAvailable.SetActive(false);
            LightningNotAvailable.SetActive(false);
            FireLockTierTwo.SetActive(true);
            FireLockTierThree.SetActive(true);
            IceLockTierTwo.SetActive(true);
            IceLockTierThree.SetActive(true);
            LightningLockTierTwo.SetActive(true);
            LightningLockTierThree.SetActive(true);
        }
        if (playerManager.chosenElement != null)
        {
            if (playerManager.chosenElement == "Fire")
            {
                IceNotAvailable.SetActive(true);
                FireNotAvailable.SetActive(false);
                LightningNotAvailable.SetActive(true);
                if (playerManager.tierTwoUnlocked)
                {
                    //turn off tier two fire lock
                    FireLockTierTwo.SetActive(false);
                }
                else if (!playerManager.tierTwoUnlocked)
                {
                    FireLockTierTwo.SetActive(true);
                    //turn on tier two fire lock
                }
                if (playerManager.tierThreeUnlocked)
                {
                    FireLockTierThree.SetActive(false);
                    //turn off tier three fire lock
                }
                else if (!playerManager.tierThreeUnlocked)
                {
                    FireLockTierThree.SetActive(true);
                    //turn on tier three fire lock
                }

            }
            else if (playerManager.chosenElement == "Ice")
            {
                IceNotAvailable.SetActive(false);
                FireNotAvailable.SetActive(true);
                LightningNotAvailable.SetActive(true);
                if (playerManager.tierTwoUnlocked)
                {
                    //turn off tier two fire lock
                    IceLockTierTwo.SetActive(false);
                }
                else if (!playerManager.tierTwoUnlocked)
                {
                    IceLockTierTwo.SetActive(true);
                    //turn on tier two fire lock
                }
                if (playerManager.tierThreeUnlocked)
                {
                    IceLockTierThree.SetActive(false);
                    //turn off tier three fire lock
                }
                else if (!playerManager.tierThreeUnlocked)
                {
                    IceLockTierThree.SetActive(true);
                    //turn on tier three fire lock
                }
            }
            else if (playerManager.chosenElement == "Lightning")
            {
                IceNotAvailable.SetActive(true);
                FireNotAvailable.SetActive(true);
                LightningNotAvailable.SetActive(false);
                if (playerManager.tierTwoUnlocked)
                {
                    //turn off tier two fire lock
                    LightningLockTierTwo.SetActive(false);
                }
                else if (!playerManager.tierTwoUnlocked)
                {
                    LightningLockTierTwo.SetActive(true);
                    //turn on tier two fire lock
                }
                if (playerManager.tierThreeUnlocked)
                {
                    LightningLockTierThree.SetActive(false);
                    //turn off tier three fire lock
                }
                else if (!playerManager.tierThreeUnlocked)
                {
                    LightningLockTierThree.SetActive(true);
                    //turn on tier three fire lock
                }
            }
        }
    }
    public void OnInspectButtonClick(string itemName)
    {
        ItemData item = playerManager.inventorySystem.GetItemData(itemName);
        Debug.Log("Item: " + item.itemName);
        if (item != null)
        {
            Debug.Log("Item found in inventory: " + itemName);
            InspectDescription.text = item.description.ToString();
            InspectedName.text = item.itemName.ToString();
            InspectedHPStat.text = item.HpPow.ToString();
            InspectedAttStat.text = item.AttPow.ToString();
            InspectedDefStat.text = item.DefPow.ToString();
            InspectedDexStat.text = item.DexPow.ToString();
            InspectedStamStat.text = item.StaminaPow.ToString();
            InspectedValue.text = item.value.ToString();
        }
        else
        {
            Debug.LogError("Item not found in inventory: " + itemName);
        }
    }
    public void InventoryUpdate()
    {
        Debug.Log("Updating inventory display...");
        TMP_Text[] inventoryDisplaySlots = {
        InventorySlotOne, InventorySlotTwo, InventorySlotThree, InventorySlotFour,
        InventorySlotFive, InventorySlotSix, InventorySlotSeven, InventorySlotEight,
        InventorySlotNine, InventorySlotTen
    };
        TMP_Text[] inventoryDisplaySlotsCount = {
        InventorySlotOneCount, InventorySlotTwoCount, InventorySlotThreeCount, InventorySlotFourCount,
        InventorySlotFiveCount, InventorySlotSixCount, InventorySlotSevenCount, InventorySlotEightCount,
        InventorySlotNineCount, InventorySlotTenCount
    };

        // Clear all slots initially
        for (int j = 0; j < inventoryDisplaySlots.Length; j++)
        {
            inventoryDisplaySlots[j].text = string.Empty;
            inventoryDisplaySlotsCount[j].text = string.Empty;
        }

        // Display items for the current page
        for (int i = 0; i < inventoryDisplaySlots.Length; i++)
        {
            int inventoryIndex = i + inspectorPageCount; // Offset by current page

            if (inventoryIndex < playerManager.inventorySystem.playerInventory.Count)
            {
                ItemData currentItem = playerManager.inventorySystem.playerInventory[inventoryIndex];
                inventoryDisplaySlots[i].text = currentItem.itemName;
                //inventoryDisplaySlotsCount[i].text = currentItem.amountHeld.ToString();
                Debug.Log($"Slot {i}: {currentItem.itemName} x{currentItem.amountHeld}");
            }
        }
    }

    public void updateEquipment()
    {
        //inHeadSlot.text = playerManager.inventorySystem.GetEquipData("Head").itemName;
        //inChestSlot.text = playerManager.inventorySystem.GetEquipData("Chest").itemName;
        //inHandSlot.text = playerManager.inventorySystem.GetEquipData("Hand").itemName;
        //inLegSlot.text = playerManager.inventorySystem.GetEquipData("Leg").itemName;
        //inFootSlot.text = playerManager.inventorySystem.GetEquipData("Foot").itemName;
        //MainHand.text = playerManager.inventorySystem.GetEquipData("MainHand").itemName;
        //inOffHandSlot.text = playerManager.inventorySystem.GetEquipData("OffHand").itemName;
        //inAccOneSlot.text = playerManager.inventorySystem.GetEquipData("Accesory1").itemName;
        //inAccTwoSlot.text = playerManager.inventorySystem.GetEquipData("Accesory2").itemName;
    }

    #endregion

    #region Game States

    public void statePaused() //pause
    {
        isPaused = true; //pause flag 
        Cursor.lockState = CursorLockMode.Confined; //keep cursor in the window
        Cursor.visible = true; //hide cursor
        //Time.timeScale = 0; //reset time passed to zero
    }
    public void stateUnPaused()
    {
        isPaused = false; //pause flag
        Cursor.visible = false; //cursor on
        Cursor.lockState = CursorLockMode.Confined; //lock cursor
        Time.timeScale = 1; //allow time to pass again
        if (activeMenu != null && activeMenu != pauseMenu) //if something other than pause menu is active
        {
            if (activeMenu == statMenu) //if its the statmenu
            {
                activeMenu.SetActive(isPaused); //turn it off
                Cursor.visible = false; //cursor on
                activeMenu = null; //set it to null
            }
        }
        else
        {
            activeMenu.SetActive(isPaused);//turn off
            Cursor.visible = false; //cursor on
            activeMenu = null; //null active menu
        }
    }
    public void youDead() //prompt player death
    {
        isPaused = true; //pause flag
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined; //confine cursor
        Cursor.visible = true; //hide cursor
        activeMenu = loseMenu; //active to lose
        loseMenu.SetActive(true); //turn it on
    }
    public void youAlive() //prompt player death
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Confined; //confine cursor
        Cursor.visible = false; //hide cursor
        activeMenu = null; //active to lose
    }
    public void SaveGame()
    {
        playerManager.HasFloorKey = false;//reset floor key
        playerManager.SavePlayerPrefs(); //set save data
    }
    public void LoadGame()
    {
        playerManager.GetPlayerPrefs(); //grab save data
        SceneManager.LoadScene("Spiral"); //load main gameplay scene
    }
    public void NewGame()
    {
        playerManager.ResetSetPlayerPrefs(); //reset save data
        playerManager.SavePlayerPrefs(); //save reset save data
        SceneManager.LoadScene("Spiral"); //load main game play scene
    }
    public void quitConfirm() //quit confirm menu
    {
        statePaused(); //pause
        activeMenu = confirmMenu; //active to confirm menu
        confirmMenu.SetActive(true); //turn it on

    }
    public void DeathQuitConfirm() //confirm death quit
    {
        statePaused(); //paused
        activeMenu = playerDeathQuitConfirm; //asign active
        playerDeathQuitConfirm.SetActive(true); //turn on

    }
    public void DeathQuitCancel() //cancel death quit
    {
        stateUnPaused(); //unpause
        playerDeathQuitConfirm.SetActive(false);//turn off death menu
        activeMenu = null; //null active
        activeMenu = loseMenu;//active deathmenu
        activeMenu.SetActive(true);//turn on
    }
    #endregion
    public int SkillPointCheck()
    {
        return playerManager.playerSkillPoints;
    }
    public void SkillPointUse()
    {
        playerManager.playerSkillPoints -= 1;
        UpdatePlayerUI();
    }
    public void AssignElement(string element)
    {
        playerManager.chosenElement = element;
    }
    public void SkillPointReturn()
    {
        playerManager.playerSkillPoints += 1;
        UpdatePlayerUI();
    }
    public int TierOneCheck()
    {
        int whichSkill = 0;
        if (playerManager.tierOne == 1)
        {
            whichSkill = 1;
        }
        else if (playerManager.tierOne == 2)
        {
            whichSkill = 2;
        }
        return whichSkill;
    }
    public int TierThreeCheck()
    {
        int whichSkill = 0;
        if (playerManager.tierThree == 1)
        {
            whichSkill = 1;
        }
        else if (playerManager.tierThree == 2)
        {
            whichSkill = 2;
        }
        return whichSkill;
    }
    public void AssignTierOne(int tier)
    {
        playerManager.tierOne = tier;
    }
    public void AssignTierTwo()
    {
        playerManager.tierTwo = 1;
    }
    public void AssignTierThree(int tier)
    {
        playerManager.tierThree = tier;
    }

    public int SkillTierOneLevelCheck()
    {
        return playerManager.skillOneLevel;
    }
    public int SkillTierTwoLevelCheck()
    {
        return playerManager.skillTwoLevel;
    }
    public void SkillOneLevelUp()
    {
        playerManager.skillOneLevel += 1;
    }
    public void SkillTwoLevelUp()
    {
        playerManager.skillTwoLevel += 1;
    }
    public bool TierTwoUnlocked()
    {
        return playerManager.tierTwoUnlocked;
    }
    public bool TierThreeUnlocked()
    {
        return playerManager.tierThreeUnlocked;
    }
    public string AssignedElementCheck()
    {
        return playerManager.chosenElement;
    }
    public void UpdatePlayerUI()
    {
        playerManager.UpdatePlayerUI();
    }
    public void AssignSkillImageOne(ScriptableSkill skill)
    {
        SkillSlotOneImage.sprite = skill.SkillSlotImage;
    }
    public void AssignSkillImageTwo(ScriptableSkill skill)
    {
        SkillSlotTwoImage.sprite = skill.SkillSlotImage;
    }
    public void AssignSkillImageThree(ScriptableSkill skill)
    {
        SkillSlotThreeImage.sprite = skill.SkillSlotImage;
    }

    public void ClearInventory()
    {
        
    }
}
