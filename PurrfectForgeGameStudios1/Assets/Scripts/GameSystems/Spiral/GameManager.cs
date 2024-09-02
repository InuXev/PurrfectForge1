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

public class GameManager : MonoBehaviour
{

    #region Fields/Objects

    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public GameObject loseMenu;
    [SerializeField] public GameObject playerHitFlash;
    [SerializeField] public GameObject statMenu;
    [SerializeField] public GameObject inventoryMenu;
    [SerializeField] public GameObject confirmMenu;
    [SerializeField] public TMP_Text inventoryList;
    [SerializeField] InventorySystem inventorySystem;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] GameObject playerDeathQuitConfirm;
    public Image playerHP;
    public Image playerHPBar;
    public TMP_Text playerHPText;
    public TMP_Text playerLvLText;
    public TMP_Text playerHPStat;
    public TMP_Text playerAttStat;
    public TMP_Text playerDefStat;
    public TMP_Text playerDexStat;
    public TMP_Text playerStamStat;
    [SerializeField] public ScriptableItems coinPurse;
    public TMP_Text playerCoins;
    public Image playerXP;
    public Image playerXPBar;
    public Image playerStam;
    public Image playerStamBar;
    public static GameManager Instance { get; private set; }
    public GameObject activeMenu;
    public bool isPaused;
    int spiralLevel;
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
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) //pause menu
        {
            if(activeMenu == null) //if active menu null
            {
                statePaused(); //call paused
                activeMenu = pauseMenu; //set menu
                pauseMenu.SetActive(true); //turn on menu
            }
            else if(activeMenu != null) //if a menu is up
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
                inventoryMenu.SetActive(true); //turn it on
                inventorySystem.InventoryDisplay(); //display inventory items
            }
            else if (activeMenu != null) //if something in active
            {
                stateUnPaused(); //state unpaused
            }
        }
    }

    #endregion

    #region Game States

    public void statePaused() //pause
    {
        isPaused = true; //pause flag 
        Cursor.lockState = CursorLockMode.Confined; //keep cursor in the window
        Cursor.visible = true; //hide cursor
        Time.timeScale = 0; //reset time passed to zero
    }
    public void stateUnPaused()
    {
        isPaused = false; //pause flag
        Cursor.visible = true; //cursor on
        Cursor.lockState = CursorLockMode.Confined; //lock cursor
        Time.timeScale = 1; //allow time to pass again
        if (activeMenu != null && activeMenu != pauseMenu) //if something other than pause menu is active
        {
            if(activeMenu == statMenu) //if its the statmenu
            {
                activeMenu.SetActive(isPaused); //turn it off
                activeMenu = null; //set it to null
            }
            else
            {
                activeMenu.SetActive(false); //turn off a menu
                activeMenu = pauseMenu; //set it to pause
                activeMenu.SetActive(true); //turn it on
            }
        }
        else
        {
            activeMenu.SetActive(isPaused);//turn off
            activeMenu = null; //null active menu
        }
    }
    public void youDead() //prompt player death
    {
        isPaused = true; //pause flag
        //Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined; //confine cursor
        Cursor.visible = true; //hide cursor
        activeMenu = loseMenu; //active to lose
        loseMenu.SetActive(true); //turn it on
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

}
