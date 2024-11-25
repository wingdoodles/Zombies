using UnityEngine;

public class FOVManager : MonoBehaviour
{
    [Header("FOV Settings")]
    public float defaultFOV = 90f;
    public float sprintFOV = 100f;
    public float aimFOV = 60f;
    
    [Header("Transition Settings")]
    public float transitionSpeed = 10f;
    
    private Camera playerCamera;
    private PlayerStateManager stateManager;
    private float targetFOV;
    
    void Start()
    {
        playerCamera = GetComponent<Camera>();
        stateManager = GetComponentInParent<PlayerStateManager>();
        targetFOV = defaultFOV;
        playerCamera.fieldOfView = defaultFOV;
    }
    
    void Update()
    {
        UpdateTargetFOV();
        SmoothFOVTransition();
    }
    
    void UpdateTargetFOV()
    {
        switch (stateManager.currentState)
        {
            case PlayerState.Running:
                targetFOV = sprintFOV;
                break;
            default:
                targetFOV = defaultFOV;
                break;
        }
        
        // Override with aim FOV when aiming
        if (Input.GetButton("Fire2")) // Right mouse button
        {
            targetFOV = aimFOV;
        }
    }
    
    void SmoothFOVTransition()
    {
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * transitionSpeed);
    }
}
