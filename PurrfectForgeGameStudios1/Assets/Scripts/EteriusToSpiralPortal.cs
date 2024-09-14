using UnityEngine.SceneManagement;
using UnityEngine;

public class EteriusToSpiralPortal : MonoBehaviour
{

    [SerializeField] SaveLoadManager saveLoadManager;
        
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            saveLoadManager.SaveEshesWorld();
            SceneManager.LoadScene("1"); //load the scene
        }
    }
}
