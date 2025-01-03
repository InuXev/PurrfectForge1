using System.Collections; // Import the System.Collections namespace for working with non-generic collections (not used in this script)
using System.Collections.Generic; // Import the System.Collections.Generic namespace for working with generic collections (not used in this script)
using UnityEngine; // Import UnityEngine namespace to access Unity-specific features like MonoBehaviour, Vector3, etc.
using UnityEngine.EventSystems; // Import UnityEngine.EventSystems namespace for handling events (not used in this script)

public class PlayerBasics : MonoBehaviour // Define the PlayerBasics class inheriting from MonoBehaviour
{
    // Variables for player movement and control
    private float gravity = 20; // Gravity force applied to the player
    private Vector3 playerVelocity; // Vector to store the player's current velocity
    private Vector3 moveDirection; // Vector to store the player's movement direction
    private float moveSpeed = 8; // Speed at which the player moves
    private float dashMult = 2; // Multiplier for dashing speed
    float panSpeed = 6f; // Speed at which the player rotates with mouse movement
    [SerializeField] CharacterController characterControl; // Reference to the CharacterController component (serialized for assignment in the Unity Editor)
    public int jumpCounter; // Counter to track the number of jumps performed
    private float jumpSpeed = 8F; // Speed at which the player jumps
    private int maxJumps = 1; // Maximum number of jumps the player can perform

    void Start() // Start is called before the first frame update
    {
        // Initialization logic (currently empty)
    }

    void Update() // Update is called once per frame
    {
        Dash(); // Call the Dash method to handle dashing logic
        Movement(); // Call the Movement method to handle player movement
        Jump(); // Call the Jump method to handle jumping logic

        if (characterControl.isGrounded) // Check if the player is grounded
        {
            jumpCounter = 0; // Reset the jump counter when the player is on the ground
        }
    }

    void Dash() // Method to handle dashing logic
    {
        if (Input.GetKeyDown("left shift")) // Check if the left shift key is pressed down
        {
            moveSpeed *= dashMult; // Increase the movement speed by the dash multiplier
        }
        if (Input.GetKeyUp("left shift")) // Check if the left shift key is released
        {
            moveSpeed /= dashMult; // Reset the movement speed to its original value
        }
    }

    void Movement() // Method to handle player movement
    {
        float x = panSpeed * Input.GetAxis("Mouse X"); // Get the mouse movement on the X-axis and multiply by pan speed
        transform.Rotate(0, x, 0); // Rotate the player around the Y-axis based on the mouse movement

        moveDirection = (Input.GetAxis("HorizontalMove") * transform.right).normalized + // Calculate movement direction on the horizontal axis
                        (Input.GetAxis("VerticalMove") * transform.forward).normalized; // Calculate movement direction on the vertical axis
        characterControl.Move(moveDirection * moveSpeed * Time.deltaTime); // Move the player based on direction and speed

        playerVelocity.y -= gravity * Time.deltaTime; // Apply gravity to the player's vertical velocity
        characterControl.Move(playerVelocity * Time.deltaTime); // Move the player vertically based on the updated velocity
    }

    void Jump() // Method to handle jumping logic
    {
        if (Input.GetButtonDown("Jump") && jumpCounter < maxJumps) // Check if the jump button is pressed and jumps are available
        {
            jumpCounter++; // Increment the jump counter
            playerVelocity.y = jumpSpeed; // Set the player's vertical velocity to jump speed
        }
        playerVelocity.y -= gravity * Time.deltaTime; // Apply gravity to the player's vertical velocity
        characterControl.Move(playerVelocity * Time.deltaTime); // Move the player vertically based on the updated velocity
    }
}
