using UnityEngine;

public class WindController : MonoBehaviour
{
    public static WindController Instance;

    [SerializeField] private float windForce = 5f;
    [SerializeField] private Vector3 windDirection = Vector3.forward;
    [SerializeField] private ParticleSystem windParticleSystem;

    [SerializeField] private float minChangeInterval = 5f;
    [SerializeField] private float maxChangeInterval = 15f;

    private float timeSinceLastChange;
    private float timeToNextChange;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        // Set the initial time to change the wind direction
        SetNextChangeTime();
    }
    private void Update()
    {
        timeSinceLastChange += Time.deltaTime;

        // Check if it's time to change the wind direction
        if (timeSinceLastChange >= timeToNextChange)
        {
            // Change the wind direction
            ChangeWindDirection();

            // Reset the timer
            timeSinceLastChange = 0f;

            // Set the next time to change the wind direction
            SetNextChangeTime();
        }
    }

    private void ChangeWindDirection()
    {
        // Randomly rotate the particle system's transform to a new direction
        float randomYRotation = Random.Range(0f, 360f);
        windParticleSystem.transform.rotation = Quaternion.Euler(0f, randomYRotation, 0f);
    }

    private void SetNextChangeTime()
    {
        // Set a random time for the next wind direction change
        timeToNextChange = Random.Range(minChangeInterval, maxChangeInterval);
    }



    public Vector3 GetWindForce()
    {
        return windDirection * windForce;
    }

    public Vector3 GetWindDirection()
    {
        return windParticleSystem.transform.forward;
    }
}
