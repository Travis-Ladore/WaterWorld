using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    //Floats//
    public float sensX;
    public float sensY;

    float xRotation;
    float yRotation;
    public float tiltAmount = 10f; // Adjust this value to control the tilt amount.
    public float returnSpeed = 5f; // Adjust this value to control the return speed.




    //Transforms//

    public Transform orientation;
    public Transform camHolder;

    //Input System variables
    public PlayerInputActions playerControls;
    private InputAction look;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }
    private void OnEnable()
    {
        look = playerControls.Player.Look;
        look.Enable();
    }
    private void OnDisable()
    {
        look.Disable();
    }

    private void Start()
    {
        //Cursor is locked in the middle of the screen on start 
        //and made invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (camHolder == null)
        {
            Debug.LogError("camHolder is not assigned. Please assign the camHolder in the inspector.");
            return;
        }

        // Make the camera a child of the camHolder
        transform.parent = camHolder;
        transform.localPosition = Vector3.zero; // Position the camera at the camHolder's origin
    }




    private void Update()
    {
        //get mouse input
        float mouseX = look.ReadValue<Vector2>().x * Time.deltaTime * sensX;
        float mouseY = look.ReadValue<Vector2>().y * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);

    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);

    }






}