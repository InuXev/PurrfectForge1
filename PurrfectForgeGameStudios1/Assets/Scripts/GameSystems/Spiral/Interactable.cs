using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] public Transform Hinge;
    [SerializeField] public PlayerManager playerManager;
    [SerializeField] GameObject Door; // Reference to the Door, only Door will move
    [SerializeField] Transform StartPOS; // Start position of the door (where the door is initially)
    [SerializeField] Transform EndPOS; // End position of the door (where it moves to)
    public float timeToEnd = 10.0f; // Total time it takes to reach the end position
    private bool isMoving = false;
    private float chestAngle = 90f; // Target rotation in degrees
    private float rotationSpeed = 30f; // Degrees per second
    public enum Type { Chest, Door, BossKeyChest, BossDoor }
    public Type type;
    bool open;

    public void Update()
    {
        if (isMoving && type == Type.Chest)
        {
            // Calculate the amount to rotate this frame
            float rotationStep = rotationSpeed * Time.deltaTime;

            // Gradually rotate the door's hinge by -90 degrees around the X-axis
            Hinge.localRotation = Quaternion.RotateTowards(
                Hinge.localRotation,
                Quaternion.Euler(-chestAngle, 0, 0),
                rotationStep
            );

            // Check if the door has reached or passed the target rotation
            if (Hinge.localEulerAngles.x <= Mathf.Abs(-chestAngle))
            {
                Hinge.localEulerAngles = new Vector3(-chestAngle, 0, 0); // Snap to the exact final angle
                isMoving = false; // Stop moving once the rotation is complete
                //logic to provide items
            }
        }
        if (isMoving && type == Type.BossKeyChest)
        {
            // Calculate the amount to rotate this frame
            float rotationStep = rotationSpeed * Time.deltaTime;

            // Gradually rotate the door's hinge by -90 degrees around the X-axis
            Hinge.localRotation = Quaternion.RotateTowards(
                Hinge.localRotation,
                Quaternion.Euler(-chestAngle, 0, 0),
                rotationStep
            );

            // Check if the door has reached or passed the target rotation
            if (Hinge.localEulerAngles.x <= Mathf.Abs(-chestAngle))
            {
                Hinge.localEulerAngles = new Vector3(-chestAngle, 0, 0); // Snap to the exact final angle
                isMoving = false; // Stop moving once the rotation is complete
                playerManager.HasBossKey = true;
            }
        }
        if (isMoving && type == Type.Door)
        {
            float rotationStep = (rotationSpeed * 5) * Time.deltaTime;

            // Gradually rotate the door's hinge towards the target angle
            Hinge.localRotation = Quaternion.RotateTowards(
                Hinge.localRotation,
                Quaternion.Euler(0, chestAngle, 0),
                rotationStep
            );
            if (Mathf.Abs(Hinge.localEulerAngles.x - chestAngle) < 0.01f)
            {
                Hinge.localEulerAngles = new Vector3(0, chestAngle, 0); // Snap to the exact final angle
                isMoving = false; // Stop moving once the rotation is complete
            }
        }
        if (isMoving && type == Type.BossDoor)
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
    public void Interact()
    {
        if (!open)
        {
            // Implement chest opening logic, such as showing loot or animation
            if (!isMoving)
            {
                // Start the movement process
                isMoving = true;
            }
        }
    }
}

