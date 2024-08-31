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
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (inventoryList == null)
        {
            Debug.LogError("InventoryList is not assigned!");
        }
        if (SceneManager.GetActiveScene().name == "Spiral")
        {
            Cursor.visible = false;
        }
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(activeMenu == null)
            {

                statePaused();
                activeMenu = pauseMenu;
                pauseMenu.SetActive(true);

            }
            else if(activeMenu != null)
            {
                stateUnPaused();
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (activeMenu == null)
            {

                statePaused();
                activeMenu = statMenu;
                statMenu.SetActive(true);

            }
            else if (activeMenu != null)
            {
                stateUnPaused();
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (activeMenu == null)
            {

                statePaused();
                activeMenu = inventoryMenu;
                inventoryMenu.SetActive(true);
                inventorySystem.InventoryDisplay();

            }
            else if (activeMenu != null)
            {
                stateUnPaused();
            }
        }
    }

    #endregion

    #region Game States

    public void statePaused()
    {
        isPaused = true;
        //keep cursor in the window
        Cursor.lockState = CursorLockMode.Confined;
        //hide cursor
        Cursor.visible = true;
        //reset time passed to zero
        Time.timeScale = 0;
    }
    public void stateUnPaused()
    {
        isPaused = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        //allow time to pass again
        Time.timeScale = 1;
        if (activeMenu != null && activeMenu != pauseMenu)
        {
            activeMenu.SetActive(false);
            activeMenu = pauseMenu;
            activeMenu.SetActive(true);

        }
        else
        {
            activeMenu.SetActive(isPaused);
            activeMenu = null;
        }
    }
    public void youDead()
    {
        isPaused = true;
        //keep cursor in the window
        //Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        //hide cursor
        Cursor.visible = true;
        //reset time passed to zero
        //Time.timeScale = 0;
        activeMenu = loseMenu;
        loseMenu.SetActive(true);
    }
    public void SaveGame()
    {
        playerManager.HasFloorKey = false;
        playerManager.SavePlayerPrefs();
    }
    public void LoadGame()
    {
        playerManager.GetPlayerPrefs();
        SceneManager.LoadScene("Spiral");
    }
    public void NewGame()
    {
        playerManager.ResetSetPlayerPrefs();
        playerManager.SavePlayerPrefs();
        SceneManager.LoadScene("Spiral");
    }
    public void quitConfirm()
    {
        statePaused();
        activeMenu = confirmMenu;
        confirmMenu.SetActive(true);

    }
    public void DeathQuitConfirm()
    {
        statePaused();
        playerDeathQuitConfirm.SetActive(true);
        activeMenu = playerDeathQuitConfirm;
    }
    public void DeathQuitCancel()
    {
        stateUnPaused();
        playerDeathQuitConfirm.SetActive(false);
        activeMenu = null;
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }
    #endregion

}
