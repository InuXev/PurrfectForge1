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
        gameManager.SaveGame();//call from GM
        SceneManager.LoadScene("Eshes"); //load eshes
        gameManager.stateUnPaused();//call from GM
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
}
