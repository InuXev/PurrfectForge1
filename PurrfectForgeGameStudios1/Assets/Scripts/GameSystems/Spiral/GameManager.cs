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
    public Image playerHPBar;
    public TMP_Text playerHPText;
    public Image playerHP;
    public static GameManager Instance;
    public GameObject activeMenu;
    public bool isPaused;

    #endregion

    #region Processes
    void Start()
    {
        
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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        //allow time to pass again
        Time.timeScale = 1;
        activeMenu.SetActive(isPaused);
        activeMenu = null;
    }
    public void youDead()
    {
        isPaused = true;
        //keep cursor in the window
        Cursor.lockState = CursorLockMode.Confined;
        //hide cursor
        Cursor.visible = true;
        //reset time passed to zero
        //Time.timeScale = 0;
        activeMenu = loseMenu;
        loseMenu.SetActive(true);
    }
    #endregion

}
