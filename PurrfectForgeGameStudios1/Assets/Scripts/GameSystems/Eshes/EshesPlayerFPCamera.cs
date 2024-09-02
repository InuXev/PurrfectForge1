using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class EshesPlayerFPCamera : MonoBehaviour
{
    #region Fields/Objects

    private float ZoomSpeed = 5f;
    private float[] ZoomBounds = new float[] { 10f, 85f };
    private Camera Camera;
    [SerializeField] EshesGameManager gameManager;
    [SerializeField] CharacterController eyeCharacterControl;
    [SerializeField] Camera FPersonCam;
    float panSpeed = 6f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;


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
            VerticalLook();
        }
    }

    void HandleOverheadMovement()
    {
        if (gameManager.FPActive)
        {
            return;
        }
        else if (Input.GetMouseButton(2))
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

