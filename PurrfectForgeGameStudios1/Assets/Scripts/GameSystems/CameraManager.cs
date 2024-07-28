using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    #region Fields/Objects

    private float ZoomSpeed = 5f;
    private float[] ZoomBounds = new float[] { 10f, 85f };
    private Camera Camera;

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
    #endregion 

}
