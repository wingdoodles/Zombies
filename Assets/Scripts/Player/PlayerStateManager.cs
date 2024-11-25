using UnityEngine;

public enum PlayerState
{
    Idle,
    Walking,
    Running,
    Crouching,
    Jumping,
    Sliding,
    Damaged
}

public class PlayerStateManager : MonoBehaviour
{
    public PlayerState currentState = PlayerState.Idle;
    private PlayerController playerController;
    
    void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
    
    public void TransitionToState(PlayerState newState)
    {
        if (currentState == newState) return;
        
        ExitState(currentState);
        currentState = newState;
        EnterState(newState);
    }
    
    private void EnterState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Running:
                playerController.SetMovementSpeed(playerController.runSpeed);
                break;
            case PlayerState.Crouching:
                playerController.SetMovementSpeed(playerController.crouchSpeed);
                break;
            default:
                playerController.SetMovementSpeed(playerController.walkSpeed);
                break;
        }
    }
    
    private void ExitState(PlayerState state)
    {
        // Handle state exit logic
    }
}
