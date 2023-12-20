using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class BoatController : MonoBehaviour
{
    private Rigidbody rigidbody;

    [SerializeField] private float baseForwardForce = 10f;
    [SerializeField] private float maxForwardForce = 20f; // Maximum forward force with wind
    [SerializeField] private float forceIncreaseSpeed = 2f;
    [SerializeField] private float turningTorque = 50f;
    [SerializeField] private float sailForce = 5f;
    [SerializeField] private float sailRotationSpeed = 50f;
    private bool isAnchored = false;
    [SerializeField] private Transform sailTransform;

    private float currentForwardForce = 0f;
    public TextMeshProUGUI forceText;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleAnchoredState();
        }
        if (!isAnchored && !WaterController.current.isGamePaused)
        {
            GoForward();
        }

        if (Input.GetKey(KeyCode.J))
        {
            RotateSail(-1);
        }

        // Rotate Sail Right
        if (Input.GetKey(KeyCode.L))
        {
            RotateSail(1);
        }

        // Rotate the boat based on WheelRotator
        float rotation = WheelRotator.Instance.angle;
        transform.Rotate(Vector3.up, rotation * turningTorque * Time.deltaTime);
    

    UpdateForce();
        UpdateForceText();
    }

    void GoForward()
    {
        // Calculate the forward force based on the sail alignment with wind
        float forwardForce = baseForwardForce;

        // Get the wind direction from the WindController
        Vector3 windDirection = WindController.Instance.GetWindDirection();

        // Calculate the angle between the sail forward direction and the wind direction
        float angle = Vector3.Angle(sailTransform.forward, windDirection);

        // You may need to adjust this threshold based on your specific requirements
        float alignmentThreshold = 30f; // Adjust as needed

        // If the sail is aligned with the wind, increase the forward force over time
        if (angle < alignmentThreshold)
        {
            currentForwardForce = Mathf.MoveTowards(currentForwardForce, maxForwardForce, forceIncreaseSpeed * Time.deltaTime);
            forwardForce += currentForwardForce;
        }
        else
        {
            // Reset the current forward force if the sail is not aligned with the wind
            currentForwardForce = 0f;
        }

        // Apply the adjusted forward force
        rigidbody.AddForce(transform.forward * forwardForce, ForceMode.Acceleration);
    }



    bool IsSailAlignedWithWind()
    {
        // Get the wind direction from the WindController
        Vector3 windDirection = WindController.Instance.GetWindDirection();

        // Calculate the angle between the sail forward direction and the wind direction
        float angle = Vector3.Angle(sailTransform.forward, windDirection);

        // You may need to adjust this threshold based on your specific requirements
        float alignmentThreshold = 30f; // Adjust as needed

        // Check if the angle is within the alignment threshold
        return angle < alignmentThreshold;
    }
    void ToggleAnchoredState()
    {
        isAnchored = !isAnchored;
        // Optionally, you can add visuals or sound effects to indicate the anchored state change
    }

    void RotateSail(int direction)
    {
        // Rotate the sail in the specified direction
        sailTransform.Rotate(Vector3.up, direction * sailRotationSpeed * Time.deltaTime);
    }
    void UpdateForce()
    {
        // Optionally, you can use this method to perform additional logic related to force updates
    }

    void UpdateForceText()
    {
        // Display the current forward force in the UI Text element
        if (forceText != null)
        {
            forceText.text = $"Forward Force: {currentForwardForce:F2}";
        }
    }
}