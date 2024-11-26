using UnityEngine;
using UnityEngine.UI;
public class StateEffectsManager : MonoBehaviour
{
    [Header("Sound Emission")]
    public float walkEmissionLevel = 0.3f;
    public float runEmissionLevel = 0.8f;
    public float crouchEmissionLevel = 0.1f;
    
    [Header("Camera Effects")]
    public float runningFOVIncrease = 5f;
    public float ADSFOVDecrease = -10f;
    
    [Header("UI Elements")]
    public Image stateIndicator;
    public Text currentStateText;
    
    private PlayerStateManager stateManager;
    private Camera playerCamera;
    private float baseFOV;
    
    void Start()
    {
        stateManager = GetComponent<PlayerStateManager>();
        playerCamera = GetComponentInChildren<Camera>();
        baseFOV = playerCamera.fieldOfView;
    }
    
    public void UpdateStateEffects(PlayerState state)
    {
        UpdateSoundEmission(state);
        UpdateCameraEffects(state);
        UpdateUI(state);
    }
    
    private void UpdateSoundEmission(PlayerState state)
    {
        float emissionLevel = state switch
        {
            PlayerState.Running => runEmissionLevel,
            PlayerState.Walking => walkEmissionLevel,
            PlayerState.Crouching => crouchEmissionLevel,
            _ => 0f
        };
        
        // Set AI detection radius based on emission level
        GameEvents.EmitSoundLevel(emissionLevel);
    }
    
    private void UpdateCameraEffects(PlayerState state)
    {
        float targetFOV = baseFOV;
        
        if (state == PlayerState.Running)
            targetFOV += runningFOVIncrease;
            
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * 5f);
    }
    
    private void UpdateUI(PlayerState state)
    {
        if (currentStateText != null)
            currentStateText.text = state.ToString();
            
        if (stateIndicator != null)
            stateIndicator.color = GetStateColor(state);
    }
    
    private Color GetStateColor(PlayerState state)
    {
        return state switch
        {
            PlayerState.Running => Color.red,
            PlayerState.Walking => Color.green,
            PlayerState.Crouching => Color.blue,
            PlayerState.Damaged => Color.yellow,
            _ => Color.white
        };
    }
}
