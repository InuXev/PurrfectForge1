using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    #region Fields/Objects
    [SerializeField] PlayerManager playerManager;
    private float ZoomSpeed = 5f;
    private float panSpeed = 6F;
    private float[] ZoomBounds = new float[] { 10f, 85f };
    private Camera Camera;

    [SerializeField] Camera OverHead;
    [SerializeField] Camera FPerson;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private Vector3 updatedPosition;
    private Quaternion updatedRotation;
    private float currentXRotation = 0.0f;
    private bool resetOverheadCamera = false;
    #endregion

    #region Processes
    void Awake()
    {
        AwakenProcesses();
        originalRotation = transform.rotation;
    }

    void Update()
    {
        updatedRotation = transform.rotation;
        UpdateProcesses();
    }

    #endregion

    #region Organzational Systems

    private void AwakenProcesses()
    {
        Camera = GetComponent<Camera>();
    }
    private void StartUpProcesses()
    {

    }

    private void UpdateProcesses()
    {
        HandleMouse();
        if (playerManager.FPActive)
        {
            // First-person camera is active
            LookVertical();
            resetOverheadCamera = false; // Reset the flag so it can trigger when switching back
        }
        else
        {
            FPerson.transform.rotation = Quaternion.Euler(0f, playerManager.transform.eulerAngles.y, 0f);
            // First-person camera is not active (Overhead camera mode)
            if (!resetOverheadCamera)
            {
                ResetOverheadCamera(); // Reset overhead camera position and rotation
                resetOverheadCamera = true; // Ensure it only resets once
            }

            HandleOverheadMovement();
        }
    }

    #endregion

    #region Camera Systems
    void HandleMouse()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, ZoomSpeed);
    }
    void LookVertical()
    {
        if (playerManager.FPActive)
        {
            float mouseY = panSpeed * Input.GetAxis("Mouse Y");

            // Update the current rotation
            currentXRotation -= mouseY; // Invert to match input direction

            // Clamp the rotation
            currentXRotation = Mathf.Clamp(currentXRotation, -45, 45);

            // Apply the clamped rotation
            FPerson.transform.localRotation = Quaternion.Euler(currentXRotation, 0, 0);
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

    void ZoomCamera(float offset, float speed)
    {
        if (offset == 0)
        {
            return;
        }

        OverHead.fieldOfView = Mathf.Clamp(OverHead.fieldOfView - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
    }
    void ResetOverheadCamera()
    {
        
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
