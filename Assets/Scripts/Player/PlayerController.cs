using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController), typeof(PlayerStateManager))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float crouchSpeed = 3f;
    public float gravity = -9.81f;
    
    [Header("Jump Settings")]
    public float jumpForce = 5f;
    public float jumpCooldown = 0.25f;
    private bool canJump = true;
    private float verticalVelocity;
    
    [Header("Look Settings")]
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 80f;
    public bool invertY = false;
    
    [Header("References")]
    public Camera playerCamera;
    public Transform weaponSocket;
    
    private CharacterController characterController;
    private PlayerStateManager stateManager;
    private Vector3 moveDirection;
    private float verticalRotation = 0f;
    private float currentMovementSpeed;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        stateManager = GetComponent<PlayerStateManager>();
        currentMovementSpeed = walkSpeed;
        
        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void Update()
    {
        HandleMovementInput();
        HandleMouseLook();
        ApplyGravity();
        HandleStateTransitions();
    }
    
    private void HandleMovementInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        moveDirection = move.normalized;
        
        // Handle jumping
        if (Input.GetButtonDown("Jump") && canJump && characterController.isGrounded)
        {
            verticalVelocity = jumpForce;
            canJump = false;
            StartCoroutine(ResetJumpCooldown());
        }
        
        // Apply gravity and vertical movement
        if (!characterController.isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        else if (verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
        
        Vector3 finalMovement = moveDirection * currentMovementSpeed;
        finalMovement.y = verticalVelocity;
        
        characterController.Move(finalMovement * Time.deltaTime);
    }
    
    private IEnumerator ResetJumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }
    
    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * (invertY ? 1 : -1);
        
        verticalRotation += mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    
    private void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    
    private void HandleStateTransitions()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            stateManager.TransitionToState(PlayerState.Running);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            stateManager.TransitionToState(PlayerState.Walking);
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            stateManager.TransitionToState(PlayerState.Crouching);
        }
    }
    
    public void SetMovementSpeed(float speed)
    {
        currentMovementSpeed = speed;
    }
}
