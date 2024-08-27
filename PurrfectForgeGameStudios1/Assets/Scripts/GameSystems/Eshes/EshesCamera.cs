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
    private bool isResetting = false; 

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
        Dash();
    }

    #endregion
    #region Camera Systems
    void Dash()
    {
        if(Input.GetKeyDown("left shift"))
        {
            moveSpeed *= dashMult;
        }
        if(Input.GetKeyUp("left shift"))
        {
            moveSpeed /= dashMult;
        }
    }
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

    public void Turn() 
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
            if (!resetOverheadCamera && !isResetting)
            {
                StartCoroutine(PerformReset());
            }

            HandleOverheadMovement();
        }
    }

    IEnumerator PerformReset()
    {
        isResetting = true; 

        transform.position = originalPosition;
        transform.rotation = originalRotation;

        eshesPlayerEye.ResetPosition();

        yield return null;

        for (int i = 0; i < 5; i++)  
        {
            transform.position = originalPosition;
            transform.rotation = originalRotation;
            eshesPlayerEye.ResetPosition();
            yield return new WaitForEndOfFrame(); 
        }

        // After ensuring everything is reset, update the preview
        // eshesPlayerEye.UpdatePreviewAfterReset();

        isResetting = false; 
        resetOverheadCamera = true; 
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

    #endregion
}
