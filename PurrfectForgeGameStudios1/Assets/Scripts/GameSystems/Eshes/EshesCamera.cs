using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EshesCamera : MonoBehaviour
{
    #region Fields/Objects

    private float ZoomSpeed = 5f;
    private float[] ZoomBounds = new float[] { 10f, 85f };
    private Camera Camera;
    float panSpeed = 6f;

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

    public void Turn() //turns player left or right
    {
        float x = panSpeed * Input.GetAxis("Mouse X");
        float y = panSpeed * Input.GetAxis("Mouse Y");
        transform.Rotate(0, x, 0);
    }
    #endregion 
}
