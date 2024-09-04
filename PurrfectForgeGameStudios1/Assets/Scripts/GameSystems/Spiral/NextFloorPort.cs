using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextFloorPort : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] ScriptableLevelCompleted highestLevelCompleted;
    public int ThisLevel;
    public string levelToLoad; //provides the next level
    //unlock completed level in Spiral Floor Selector
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (highestLevelCompleted.highestLevelComplete < ThisLevel)
            {
                highestLevelCompleted.highestLevelComplete = ThisLevel;
                Debug.Log(playerManager.highestFloorCompleted);
            }
            PlayerManager.Instance.SavePlayerPrefs(); //save player stats
            SceneManager.LoadScene(levelToLoad); //load the scene



        }
    }
}
