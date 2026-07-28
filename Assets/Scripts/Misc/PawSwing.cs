using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PawSwing : MonoBehaviour
{
    public float swingAngle = 30f;      // Maximum swing angle
    public float swingSpeed = 2f;       // Speed multiplier for time
    public float swingFrequency = 2f;   // Frequency of the oscillation
    public float damping = 0.995f;      // Energy loss (lower = stops faster)
    public float bounceElasticity = 0.8f; // How much energy is kept after a bounce (0.8 = 80%)


    private float currentAmplitude;
    private float time;
    private bool isSwinging = false;

    private float direction = 1f;
    private float bounceCooldown = 0f;

    void Start()
    {
        // Start with no motion
        currentAmplitude = 0f;
    }

    // This triggers when you click the object (must have a Collider)
    private void OnMouseDown()
    {
        // Give it a "push" by setting the amplitude to the max angle
        currentAmplitude = swingAngle;
        // Reset time so the swing starts from a consistent point
        time = 0f;
        isSwinging = true;
    }


    void ApplySwinging()
    {
        // 1. Advance time based on your speed settings
        time += Time.deltaTime * swingSpeed;

        // 2. Reduce amplitude over time (Damping)
        currentAmplitude *= damping;

        // 3. Stop calculating if the motion is too small to see
        if (currentAmplitude < 0.2f)
        {
            currentAmplitude = 0f;
            isSwinging = false;
            transform.rotation = Quaternion.Euler(0, 0, 0); // Reset to center
            return;
        }

        // 4. Calculate the current angle using the sine wave
        float angle = currentAmplitude * Mathf.Sin(time * swingFrequency);

        // 5. Apply the rotation
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        // Check if we hit another swinging object
        if (collision.CompareTag("SwingingObject"))
        {
            Debug.Log("Clink! They touched.");

            // Example: Make them both stop swinging slightly
            currentAmplitude *= 0.5f;

            
        }
    }

    public void HandleCollision(Collider other)
    {
        // Only bounce if cooldown is over (prevents sticking)
        if (bounceCooldown <= 0)
        {
            Debug.Log("Bounce!");

            // 1. Reverse direction!
            direction *= -1f;

            // 2. Lose some energy from the impact
            currentAmplitude *= bounceElasticity;

            // 3. Set a small cooldown so it doesn't bounce again for 0.2 seconds
            bounceCooldown = 0.2f;
        }
    }

    public void StartSwinging()
    {
        currentAmplitude = swingAngle;
        time = 0f;
        direction = 1f; // Reset to forward motion
        isSwinging = true;
    }

    void Update()
    {
        if (isSwinging)
        {
            ApplySwinging2();
        }

        // Reduce cooldown timer
        if (bounceCooldown > 0) bounceCooldown -= Time.deltaTime;
    }

    void ApplySwinging2()
    {
        // 1. Time now moves forward OR backward based on direction
        time += Time.deltaTime * swingSpeed * direction;

        currentAmplitude *= damping;

        if (currentAmplitude < 0.05f)
        {
            currentAmplitude = 0f;
            isSwinging = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }

        float angle = currentAmplitude * Mathf.Sin(time * swingFrequency);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

  

}
