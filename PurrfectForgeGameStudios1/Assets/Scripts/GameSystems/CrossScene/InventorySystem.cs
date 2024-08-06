using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventorySystem : MonoBehaviour
{
    Scene currentScene;
    // Start is called before the first frame update
    void Start()
    {
        //identify which scene im in
        currentScene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        string sceneName = currentScene.name;
        if(sceneName == "Eshse")
        {

        }
        if (sceneName == "Spiral")
        {

        }
    }
}
