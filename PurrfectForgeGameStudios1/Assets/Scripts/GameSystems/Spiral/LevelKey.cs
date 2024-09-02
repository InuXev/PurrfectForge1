using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelKey : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerManager playerManager = other.GetComponent<PlayerManager>(); //grab the players manager
            playerManager.HasFloorKey = true; //set the player to have key for current floor
            Destroy(gameObject); //destory key on pick up
        }
    }
}
