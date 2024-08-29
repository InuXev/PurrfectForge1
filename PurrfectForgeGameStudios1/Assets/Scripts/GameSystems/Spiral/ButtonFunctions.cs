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
    public void SaveGame()
    {
        gameManager.SaveGame();
    }
    public void LoadGame()
    {
        gameManager.LoadGame();
    }
    public void NewGame()
    {
        gameManager.NewGame();
    }
    public void restart()
    {
        //reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
        gameManager.stateUnPaused();

    }

    public void eshesPort()
    {
        gameManager.SaveGame();
        SceneManager.LoadScene("Eshes");
        gameManager.stateUnPaused();
    }
       public void FrontQuit()
    {
        gameManager.quitConfirm();
    }
    public void completeQuit()
    {
        SceneManager.LoadScene("Eshes");
    }
    public void resume2()
    {
        gameManager.stateUnPaused();
        gameManager.confirmMenu.SetActive(false);
    }
    #endregion
}
