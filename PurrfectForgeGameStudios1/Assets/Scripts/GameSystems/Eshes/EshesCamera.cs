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
    private Vector3 moveDirection;
    float panSpeed = 6f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;



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

        HandleOverheadMovement();
    }

    IEnumerator PerformReset()
    {

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
