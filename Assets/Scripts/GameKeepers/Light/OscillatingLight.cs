using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OscillatingLight : MonoBehaviour
{
    [SerializeField] private Light2D light2D;
    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 1.5f;
    [SerializeField] private float speed = 2f;

    private void Awake()
    {
        if (light2D == null)
        {
            light2D = GetComponent<Light2D>();
        }
    }

    private void Update()
    {
        float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f;

        light2D.intensity = Mathf.Lerp(
            minIntensity,
            maxIntensity,
            t
        );
    }
}