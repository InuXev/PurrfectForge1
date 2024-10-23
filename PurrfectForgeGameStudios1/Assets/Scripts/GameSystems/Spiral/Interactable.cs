using System;
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
    [SerializeField] GameObject Effect1;
    [SerializeField] GameObject Effect2;
    [SerializeField] GameObject Effect3;
    [SerializeField] GameObject Loot;
    [SerializeField] GameObject[] LootPool;
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

            // Get the current rotation progress as a percentage (0 = closed, 1 = fully open)
            float currentAngle = Hinge.localEulerAngles.x;
            if (currentAngle > 180) currentAngle -= 360;  // Correct for the way Euler angles wrap

            // Normalize the progress based on the target angle
            float openPercentage = Mathf.Clamp01(Mathf.Abs(currentAngle) / Mathf.Abs(chestAngle));

            // Use Mathf.SmoothStep to slow down the orb movement (makes it more gradual)
            float smoothOpenPercentage = Mathf.SmoothStep(0f, 1f, (openPercentage * .4f));

            // Use this percentage to move Effect1 smoothly from StartPOS to EndPOS
            Effect1.transform.position = Vector3.Lerp(StartPOS.position, EndPOS.position, smoothOpenPercentage);
            Loot.transform.position = Vector3.Lerp(StartPOS.position, EndPOS.position, smoothOpenPercentage);
            // If Effect1 has reached the end position, disable it and enable Effect2
            if (openPercentage >= 1f)
            {
                Effect1.SetActive(false);
                Loot.SetActive(false);
                Effect2.SetActive(true);
            }

            // Check if the chest lid has reached the target rotation
            if (Mathf.Abs(currentAngle) >= Mathf.Abs(chestAngle))
            {
                // Snap to the final position
                Hinge.localEulerAngles = new Vector3(-chestAngle, 0, 0);
                open = true;
                // Stop moving
                isMoving = false;
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

            // Get the current rotation progress as a percentage (0 = closed, 1 = fully open)
            float currentAngle = Hinge.localEulerAngles.x;
            if (currentAngle > 180) currentAngle -= 360;  // Correct for the way Euler angles wrap

            // Normalize the progress based on the target angle
            float openPercentage = Mathf.Clamp01(Mathf.Abs(currentAngle) / Mathf.Abs(chestAngle));

            // Use Mathf.SmoothStep to slow down the orb movement (makes it more gradual)
            float smoothOpenPercentage = Mathf.SmoothStep(0f, 1f, (openPercentage * .4f));

            // Use this percentage to move Effect1 smoothly from StartPOS to EndPOS
            Effect1.transform.position = Vector3.Lerp(StartPOS.position, EndPOS.position, smoothOpenPercentage);
            Loot.transform.position = Vector3.Lerp(StartPOS.position, EndPOS.position, smoothOpenPercentage);
            // If Effect1 has reached the end position, disable it and enable Effect2
            if (openPercentage >= 1f)
            {
                Effect1.SetActive(false);
                Loot.SetActive(false);
                Effect2.SetActive(true);
            }

            // Check if the chest lid has reached the target rotation
            if (Mathf.Abs(currentAngle) >= Mathf.Abs(chestAngle))
            {
                // Snap to the final position
                Hinge.localEulerAngles = new Vector3(-chestAngle, 0, 0);
                open = true;
                // Stop moving
                isMoving = false;
                playerManager.HasBossKey = true;

            }
        }

        if (isMoving && type == Type.Door)
        {
            float rotationStep = (rotationSpeed * 5) * Time.deltaTime;

            // Gradually rotate the door's hinge towards the target angle
            Hinge.localRotation = Quaternion.RotateTowards( Hinge.localRotation, Quaternion.Euler(0, chestAngle, 0), rotationStep);
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
        if (!open && type != Type.BossDoor)  // Check if the chest is not already open
        {
            if (!isMoving)
            {
                // Start the movement process
                isMoving = true;
                if (type == Type.Chest || type == Type.BossKeyChest)
                {
                    LootSelector();
                    Quaternion lootRotation = Quaternion.Euler(0, -90, 0); // Adjust the axis as needed
                    Loot = Instantiate(Loot, StartPOS.position, lootRotation);
                }
            }
        }
        if(!open && type == Type.BossDoor && PlayerManager.Instance.HasBossKey)
        {
            isMoving = true;
        }
    }

    public void LootSelector()
    {
        if (type == Type.Chest)
        {
            int randomIndex = UnityEngine.Random.Range(0, LootPool.Length); // Random.Range for Unity
            Loot = LootPool[randomIndex];
        }
    }

}

