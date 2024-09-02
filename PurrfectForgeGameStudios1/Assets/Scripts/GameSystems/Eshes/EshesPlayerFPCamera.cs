using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class EshesPlayerFPCamera : MonoBehaviour
{
    #region Fields/Objects

    private float ZoomSpeed = 5f; // Speed at which the camera zooms.
    private float[] ZoomBounds = new float[] { 10f, 85f }; // Minimum and maximum values for the camera's field of view.
    private Camera Camera; // Reference to the Camera component.
    [SerializeField] EshesGameManager gameManager; // Reference to the EshesGameManager, set in the inspector.
    [SerializeField] CharacterController eyeCharacterControl; // Reference to the CharacterController for eye movement, set in the inspector.
    [SerializeField] Camera FPersonCam; // Reference to the first-person camera, set in the inspector.
    float panSpeed = 6f; // Speed at which the camera pans.

    private Vector3 originalPosition; // Stores the camera's original position.
    private Quaternion originalRotation; // Stores the camera's original rotation.

    private float currentXRotation = 0.0f; // Tracks the current vertical rotation of the first-person camera.

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
        VerticalLook(); // Handles vertical camera rotation based on mouse input.
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
        if (gameManager.FPActive) // Checks if first-person mode is active.
        {
            VerticalLook(); // Handles vertical camera rotation based on mouse input if in first-person mode.
        }
    }

    void HandleOverheadMovement() // Handles camera movement with the middle mouse button.
    {
        if (gameManager.FPActive) // Checks if first-person mode is active.
        {
            return; // Does nothing if in first-person mode.
        }
        else if (Input.GetMouseButton(2)) // Checks if the middle mouse button is pressed.
        {
            float h = Input.GetAxis("Mouse X") * panSpeed * Time.deltaTime; // Gets horizontal mouse movement.
            float v = Input.GetAxis("Mouse Y") * panSpeed * Time.deltaTime; // Gets vertical mouse movement.

            transform.Translate(-h, -v, 0); // Moves the camera based on mouse input.
        }
    }

    private void VerticalLook() // Handles vertical camera rotation based on mouse input.
    {
        float y = panSpeed * Input.GetAxis("Mouse Y"); // Gets vertical mouse movement.
        currentXRotation -= y; // Updates the current vertical rotation.
        currentXRotation = Mathf.Clamp(currentXRotation, -45, 45); // Clamps the rotation to avoid extreme angles.
        FPersonCam.transform.localRotation = Quaternion.Euler(currentXRotation, 0, 0); // Applies the rotation to the first-person camera.
    }
    #endregion
}

