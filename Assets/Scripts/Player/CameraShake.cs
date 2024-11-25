using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Shake Parameters")]
    public float traumaDecaySpeed = 1.5f;
    public float maxShakeRotation = 5f;
    public float maxShakeOffset = 0.5f;
    public float noiseFrequency = 30f;
    
    private float trauma = 0f;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    
    void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }
    
    void Update()
    {
        if (trauma > 0)
        {
            ApplyShake();
            DecayTrauma();
        }
    }
    
    public void AddTrauma(float amount)
    {
        trauma = Mathf.Clamp01(trauma + amount);
    }
    
    void ApplyShake()
    {
        float shake = trauma * trauma;
        float time = Time.time * noiseFrequency;
        
        Vector3 rotationOffset = new Vector3(
            maxShakeRotation * shake * (Mathf.PerlinNoise(time, 0) * 2 - 1),
            maxShakeRotation * shake * (Mathf.PerlinNoise(0, time) * 2 - 1),
            maxShakeRotation * shake * (Mathf.PerlinNoise(time, time) * 2 - 1)
        );
        
        transform.localRotation = originalRotation * Quaternion.Euler(rotationOffset);
        
        Vector3 positionOffset = new Vector3(
            maxShakeOffset * shake * (Mathf.PerlinNoise(time + 1, 0) * 2 - 1),
            maxShakeOffset * shake * (Mathf.PerlinNoise(0, time + 1) * 2 - 1),
            maxShakeOffset * shake * (Mathf.PerlinNoise(time + 1, time + 1) * 2 - 1)
        );
        
        transform.localPosition = originalPosition + positionOffset;
    }
    
    void DecayTrauma()
    {
        trauma = Mathf.Max(0, trauma - Time.deltaTime * traumaDecaySpeed);
    }
}
