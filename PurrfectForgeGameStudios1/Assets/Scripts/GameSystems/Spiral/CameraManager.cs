using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
    private float currentXRotation = 0.0f;
    #endregion

    #region Processes
    void Awake()
    {
        AwakenProcesses(); //all awken processes
    }

    void Update()
    {

        UpdateProcesses(); //all update processes
    }

    #endregion

    #region Organzational Systems

    private void AwakenProcesses() //awkens
    {
        Camera = GetComponent<Camera>(); //grab camera
    }

    private void UpdateProcesses() //updates
    {
        HandleMouse(); //handled mouse work
        if (playerManager.FPActive) //check for First Person
        {
            LookVertical(); //do vertical axis looking in First person
        }
        else //not in First person
        {
            //FPerson.transform.rotation = Quaternion.Euler(0f, playerManager.transform.eulerAngles.y, 0f); //rotate FP cam
            HandleOverheadMovement(); //handle trhe overheadcameras movement
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
            float mouseY = panSpeed * Input.GetAxis("Mouse Y"); //grab y axis from mouse
            currentXRotation -= mouseY; // Invert to match input direction
            currentXRotation = Mathf.Clamp(currentXRotation, -45, 45);// Clamp the rotation
            FPerson.transform.localRotation = Quaternion.Euler(currentXRotation, 0, 0); // Apply the clamped rotation
        }
    }

    void ZoomCamera(float offset, float speed)
    {
        if (offset == 0) //if no offset
        {
            return;
        }
        OverHead.fieldOfView = Mathf.Clamp(OverHead.fieldOfView - (offset * speed), ZoomBounds[0], ZoomBounds[1]); //xoom with clamp
    }
    void HandleOverheadMovement() //moveOver head cam
    {
        if (Input.GetMouseButton(2)) //mouse left right
        {
            float h = Input.GetAxis("Mouse X") * panSpeed * Time.deltaTime; //grab mouse x
            float v = Input.GetAxis("Mouse Y") * panSpeed * Time.deltaTime; //grab mouse y
            transform.Translate(-h, -v, 0); //Rotate player
        }
    }
    #endregion 

}
