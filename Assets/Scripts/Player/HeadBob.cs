using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("Bob Parameters")]
    public float walkingBobbingSpeed = 14f;
    public float runningBobbingSpeed = 18f;
    public float bobbingAmount = 0.05f;
    public float sprintBobbingAmount = 0.08f;
    
    [Header("Smooth Transitions")]
    public float smoothTransitionSpeed = 10f;
    
    private float defaultPosY = 0;
    private float timer = 0;
    private PlayerStateManager stateManager;
    
    void Start()
    {
        defaultPosY = transform.localPosition.y;
        stateManager = GetComponentInParent<PlayerStateManager>();
    }
    
    void Update()
    {
        if (stateManager.currentState == PlayerState.Walking || 
            stateManager.currentState == PlayerState.Running)
        {
            HandleHeadBob();
        }
        else
        {
            ResetPosition();
        }
    }
    
    void HandleHeadBob()
    {
        float bobbingSpeed = stateManager.currentState == PlayerState.Running ? 
            runningBobbingSpeed : walkingBobbingSpeed;
            
        float bobAmount = stateManager.currentState == PlayerState.Running ? 
            sprintBobbingAmount : bobbingAmount;
            
        timer += Time.deltaTime * bobbingSpeed;
        
        float newY = defaultPosY + Mathf.Sin(timer) * bobAmount;
        Vector3 newPos = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, Time.deltaTime * smoothTransitionSpeed);
    }
    
    void ResetPosition()
    {
        timer = 0;
        Vector3 newPos = new Vector3(transform.localPosition.x, defaultPosY, transform.localPosition.z);
        transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, Time.deltaTime * smoothTransitionSpeed);
    }
}
