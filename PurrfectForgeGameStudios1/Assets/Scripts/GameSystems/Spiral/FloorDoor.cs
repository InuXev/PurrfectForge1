using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDoor : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager; //to check key flag
    [SerializeField] GameObject Door; // Reference to the Door, only Door will move
    [SerializeField] Transform StartPOS; // Start position of the door (where the door is initially)
    [SerializeField] Transform EndPOS; // End position of the door (where it moves to)
    public float timeToEnd = 10.0f; // Total time it takes to reach the end position

    private bool isMoving = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerManager.HasFloorKey)
        {
            if (!isMoving)
            {
                // Start the movement process
                isMoving = true;
            }

            playerManager.HasFloorKey = false; // Remove key flag from player after triggering
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            // Continue moving the door over time
            float t = Time.deltaTime;

            // Smoothly move the Door between StartPOS and EndPOS
            Door.transform.position = Vector3.Lerp(StartPOS.position, EndPOS.position, t);

            if (t >= 9.0f)
            {
                // When movement is done, snap the door to the end position and stop moving
                Door.transform.position = EndPOS.position;
                isMoving = false; // Stop further updates
            }
        }
    }
}

