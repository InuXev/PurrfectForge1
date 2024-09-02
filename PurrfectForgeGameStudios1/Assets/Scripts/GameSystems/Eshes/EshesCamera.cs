using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class EshesCamera : MonoBehaviour
{
    #region Fields/Objects

    private float ZoomSpeed = 5f; // Speed at which the camera zooms.
    private float[] ZoomBounds = new float[] { 10f, 85f }; // Minimum and maximum values for camera field of view.
    private Camera Camera; // Reference to the Camera component.
    [SerializeField] EshesGameManager gameManager; // Reference to the EshesGameManager, set in the inspector.
    [SerializeField] CharacterController eyeCharacterControl; // Reference to the CharacterController for eye movement, set in the inspector.
    [SerializeField] EshesPlayerEye eshesPlayerEye; // Reference to the EshesPlayerEye, set in the inspector.
    private Vector3 moveDirection; // Vector for storing movement direction.
    float panSpeed = 6f; // Speed at which the camera pans.

    private Vector3 originalPosition; // Stores the camera's original position.
    private Quaternion originalRotation; // Stores the camera's original rotation.

    #endregion

    #region Processes
    void Awake() // Called when the script instance is being loaded.
    {
        AwakenProcesses(); // Initializes variables and states.
    }

    void Update() // Called once per frame.
    {
        UpdateProcesses(); // Updates processes like handling input.
    }

    #endregion

    #region Organizational Systems

    private void AwakenProcesses() // Initializes or sets up components and variables.
    {
        Camera = GetComponent<Camera>(); // Gets the Camera component attached to the GameObject.

        originalPosition = transform.position; // Records the initial position of the camera.
        originalRotation = transform.rotation; // Records the initial rotation of the camera.
    }

    private void UpdateProcesses() // Manages processes to be updated every frame.
    {
        HandleMouse(); // Handles mouse input for camera zoom.
    }

    #endregion

    #region Camera Systems

    void HandleMouse() // Handles mouse input for camera operations.
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel"); // Gets the scroll wheel input.
        ZoomCamera(scroll, ZoomSpeed); // Applies zoom based on scroll input and zoom speed.
    }

    void ZoomCamera(float offset, float speed) // Zooms the camera in or out.
    {
        if (offset == 0) // If there's no scroll input, do nothing.
        {
            return;
        }

        Camera.fieldOfView = Mathf.Clamp(Camera.fieldOfView - (offset * speed), ZoomBounds[0], ZoomBounds[1]); // Adjusts the field of view with bounds.
    }

    public void Movement() // Public method to trigger camera movement.
    {
        HandleOverheadMovement(); // Handles movement based on input.
    }

    IEnumerator PerformReset() // Coroutine to reset camera position and rotation.
    {
        transform.position = originalPosition; // Resets camera position.
        transform.rotation = originalRotation; // Resets camera rotation.

        eshesPlayerEye.ResetPosition(); // Resets the player's eye position.

        yield return null; // Waits for the end of the frame.

        for (int i = 0; i < 5; i++) // Loops to repeatedly reset position and rotation.
        {
            transform.position = originalPosition; // Resets camera position.
            transform.rotation = originalRotation; // Resets camera rotation.
            eshesPlayerEye.ResetPosition(); // Resets the player's eye position.
            yield return new WaitForEndOfFrame(); // Waits for the end of the frame.
        }
        eshesPlayerEye.UpdatePreviewAfterReset(); // Updates the preview after resetting.
    }

    void HandleOverheadMovement() // Handles camera movement with the middle mouse button.
    {
        if (Input.GetMouseButton(2)) // Checks if the middle mouse button is pressed.
        {
            float h = Input.GetAxis("Mouse X") * panSpeed * Time.deltaTime; // Gets horizontal mouse movement.
            float v = Input.GetAxis("Mouse Y") * panSpeed * Time.deltaTime; // Gets vertical mouse movement.

            transform.Translate(-h, -v, 0); // Moves the camera based on mouse input.
        }
    }
    #endregion
}
