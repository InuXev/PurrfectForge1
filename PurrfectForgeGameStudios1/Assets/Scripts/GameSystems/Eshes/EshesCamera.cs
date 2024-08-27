using System.Collections;
using UnityEngine;

public class EshesCamera : MonoBehaviour
{
    #region Fields/Objects

    private float ZoomSpeed = 5f;
    private float[] ZoomBounds = new float[] { 10f, 85f };
    private Camera Camera;
    [SerializeField] EshesGameManager gameManager;
    [SerializeField] CharacterController eyeCharacterControl;
    [SerializeField] CharacterController eshesCharacterControl;
    [SerializeField] EshesPlayerEye eshesPlayerEye;
    private Vector3 moveDirection;
    public float moveSpeed;
    public float dashMult;
    float panSpeed = 6f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private bool resetOverheadCamera = false;

    private float gravity = 20;
    private Vector3 playerVelocity;

    #endregion

    #region Processes
    void Awake()
    {
        AwakenProcesses();
    }

    void Update()
    {
        UpdateProcesses();
    }

    #endregion

    #region Organizational Systems

    private void AwakenProcesses()
    {
        Camera = GetComponent<Camera>();

        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void UpdateProcesses()
    {
        HandleMouse();
        Turn();
    }

    #endregion

    #region Camera Systems

    void HandleMouse()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, ZoomSpeed);
    }

    void ZoomCamera(float offset, float speed)
    {
        if (offset == 0)
        {
            return;
        }

        Camera.fieldOfView = Mathf.Clamp(Camera.fieldOfView - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
    }

    public void Turn() //turns player left or right
    {
        if (gameManager.FPActive)
        {
            resetOverheadCamera = false;

            float x = panSpeed * Input.GetAxis("Mouse X");
            float y = panSpeed * Input.GetAxis("Mouse Y");
            transform.Rotate(0, x, 0);

            moveDirection = (Input.GetAxis("Horizontal") * transform.right) +
            (Input.GetAxis("Vertical") * transform.forward).normalized;
            eshesCharacterControl.Move(moveDirection * moveSpeed * Time.deltaTime);

            playerVelocity.y -= gravity * Time.deltaTime;
            eshesCharacterControl.Move(playerVelocity * Time.deltaTime);
        }
        else
        {
            if (!resetOverheadCamera)
            {
                ResetOverheadCamera();
                resetOverheadCamera = true;
            }


            HandleOverheadMovement();
        }
    }

    void ResetOverheadCamera()
    {

        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }

    void HandleOverheadMovement()
    {
        if (Input.GetMouseButton(2))
        {
            float h = Input.GetAxis("Mouse X") * panSpeed * Time.deltaTime;
            float v = Input.GetAxis("Mouse Y") * panSpeed * Time.deltaTime;

            transform.Translate(-h, -v, 0);
        }
    }
    public void ResetCamera()
    {
        ResetOverheadCamera();

        if (eshesPlayerEye != null)
        {
            eshesPlayerEye.ResetPosition(); // Reset player eye position

            // Wait for the player position to reset before updating the preview
            StartCoroutine(ResetPreviewAfterDelay(0.1f)); // Adjust delay if needed
        }
    }

    IEnumerator ResetPreviewAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (eshesPlayerEye != null)
        {
            eshesPlayerEye.ObjectPreview(); // Recreate the preview after resetting
        }
    }
    #endregion
}
