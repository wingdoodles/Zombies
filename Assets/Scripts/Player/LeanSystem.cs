using UnityEngine;

public class LeanSystem : MonoBehaviour
{
    [Header("Lean Settings")]
    public float leanAngle = 15f;
    public float leanAmount = 0.5f;
    public float headTiltAngle = 5f;
    public float leanSpeed = 8f;
    
    private Camera playerCamera;
    private float currentLean = 0f;
    private float currentTilt = 0f;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    
    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        originalPosition = playerCamera.transform.localPosition;
        originalRotation = playerCamera.transform.localRotation;
    }
    
    void Update()
    {
        HandleLean();
    }
    
    void HandleLean()
    {
        float targetLean = 0f;
        float targetTilt = 0f;
        
        if (Input.GetKey(KeyCode.Q))
        {
            targetLean = -1f;
            targetTilt = 1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            targetLean = 1f;
            targetTilt = -1f;
        }
            
        currentLean = Mathf.Lerp(currentLean, targetLean, Time.deltaTime * leanSpeed);
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * leanSpeed);
        
        Vector3 targetEulerAngles = new Vector3(
            currentTilt * headTiltAngle,
            0f,
            currentLean * leanAngle
        );
        
        playerCamera.transform.localRotation = Quaternion.Euler(targetEulerAngles);
        playerCamera.transform.localPosition = originalPosition + (Vector3.right * currentLean * leanAmount);
    }
}