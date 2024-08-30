using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class EshesCamera : MonoBehaviour
{
    #region Fields/Objects

    private float ZoomSpeed = 5f;
    private float[] ZoomBounds = new float[] { 10f, 85f };
    private Camera Camera;
    [SerializeField] EshesGameManager gameManager;
    [SerializeField] CharacterController eyeCharacterControl;
    [SerializeField] EshesPlayerEye eshesPlayerEye;
    [SerializeField] Camera FPersonCam;
    private Vector3 moveDirection;
    public float moveSpeed;
    public float dashMult;
    float panSpeed = 6f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private bool resetOverheadCamera = false;
    private bool isResetting = false;
    private float currentXRotation = 0.0f;

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
        VerticalLook();
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

    public void Movement()
    {
        if (gameManager.FPActive)
        {
            resetOverheadCamera = false;
            VerticalLook();
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
        eshesPlayerEye.UpdatePreviewAfterReset();

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
    private void VerticalLook()
    {
        float y = panSpeed * Input.GetAxis("Mouse Y");
        currentXRotation -= y;
        currentXRotation = Mathf.Clamp(currentXRotation, -45, 45);
        FPersonCam.transform.localRotation = Quaternion.Euler(currentXRotation, 0, 0);
    }
    #endregion
}
