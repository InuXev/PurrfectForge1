using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
    #region Fields/Objects

    [SerializeField] GameManager gameManager;
    public Button targetButton;

    #endregion

    #region Buttons
    public void resume()
    {
        gameManager.stateUnPaused();
    }

    public void restart()
    {
        //reload scene
        
        SceneManager.LoadScene("MainWorld");
        gameManager.stateUnPaused();

    }
       public void quit2()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion
}
