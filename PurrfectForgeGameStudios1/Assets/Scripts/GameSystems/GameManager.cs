using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] public GameObject pauseMenu;
    public GameObject activeMenu;
    public bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
    public void statePaused()
    {
        //
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
}
