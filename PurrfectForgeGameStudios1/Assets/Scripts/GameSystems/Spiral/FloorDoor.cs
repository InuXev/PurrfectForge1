using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDoor : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] GameObject LeftDoor;
    [SerializeField] GameObject RightDoor;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerManager.HasFloorKey)
        {
            //check for key here
            LeftDoor.transform.Rotate(0, 100, 0);
            RightDoor.transform.Rotate(0, -100, 0);
            playerManager.HasFloorKey = false;
        }
    }
}
