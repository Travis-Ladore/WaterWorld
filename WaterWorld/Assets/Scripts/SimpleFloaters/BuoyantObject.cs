using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyantObject : MonoBehaviour
{
    public Rigidbody rb;
    public float depthBeforeSubmermged = 1f;
    public float displacementAmount = 3f;
    public int floaterCount = 1;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;

    private Quaternion initialRotation;
    public float tippingThreshold = 30f;
    private void Start()
    {
        // Store the initial rotation of the object.
        initialRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        float waveHeight = WaterController.current.getHeightAtPosition(transform.position);
        if (transform.position.y < waveHeight)
        {
            rb.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);

            if(transform.position.y < 0f)
            {
                float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmermged) * displacementAmount;
                rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
                rb.AddForce(displacementMultiplier * -rb.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
                rb.AddTorque(displacementMultiplier * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
                if (IsTippingOver())
                {
                    Debug.Log("Tipping over!");
                    // Perform actions when tipping over.
                }
            }
        }
    }

    private bool IsTippingOver()
    {
        // Calculate the angle between the initial rotation and the current rotation.
        float angle = Quaternion.Angle(initialRotation, transform.rotation);

        // Check if the angle exceeds the tipping threshold.
        return angle > tippingThreshold;
    }
}
