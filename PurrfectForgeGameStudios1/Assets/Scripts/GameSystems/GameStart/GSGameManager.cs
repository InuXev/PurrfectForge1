using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GSGameManager : MonoBehaviour
{
    public void LoadGame()
    {
        //playerManager.GetPlayerPrefs();
        SceneManager.LoadScene("Spiral");
        Time.timeScale = 1;
    }
    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Spiral");
        Time.timeScale = 1;
    }
}
