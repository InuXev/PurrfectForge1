using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextFloorPort : MonoBehaviour
{
    public string levelToLoad; //provides the next level

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerManager.Instance.SavePlayerPrefs(); //save player stats
            SceneManager.LoadScene(levelToLoad); //load the scene
        }
    }
}
