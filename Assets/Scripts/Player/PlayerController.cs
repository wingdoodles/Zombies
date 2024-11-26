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

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standingHeight = 2f;
    private bool isCrouching = false;
    private float currentHeight;
        
    [Header("Momentum Settings")]
    public float accelerationSpeed = 10f;
    public float decelerationSpeed = 15f;
    private Vector3 currentVelocity;

    [Header("Slide Settings")]
    public float slideSpeed = 10f;
    public float slideTime = 1f;
    public float slideCooldown = 1f;
    private bool canSlide = true;
    private float currentSlideTime;
    
    [Header("References")]
    public Camera playerCamera;
    public Transform weaponSocket;
    
    private CharacterController characterController;
    private PlayerStateManager stateManager;
    private Vector3 moveDirection;
    private float verticalRotation = 0f;
    private float currentMovementSpeed;

    public Vector3 GetMovementVector()
    {
        return currentVelocity;
    }
    
    public Vector3 GetVelocity()
    {
        return characterController.velocity;
    }
    
    public bool IsGrounded()
    {
        return characterController.isGrounded;
    }
    
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
        
        Vector3 targetDirection = transform.right * horizontal + transform.forward * vertical;
        targetDirection = targetDirection.normalized * currentMovementSpeed;
        
        // Apply momentum
        currentVelocity = Vector3.Lerp(
            currentVelocity, 
            targetDirection, 
            (targetDirection.magnitude > 0 ? accelerationSpeed : decelerationSpeed) * Time.deltaTime
        );
        
        Vector3 finalMovement = currentVelocity;
        finalMovement.y = verticalVelocity;
        
        characterController.Move(finalMovement * Time.deltaTime);
        
        // Handle jumping
        if (Input.GetButtonDown("Jump") && canJump && characterController.isGrounded)
        {
            verticalVelocity = jumpForce;
            canJump = false;
            StartCoroutine(ResetJumpCooldown());
        }
        // Handle crouching toggle
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isCrouching)
            {
                characterController.height = standingHeight;
                currentMovementSpeed = walkSpeed;
                isCrouching = false;
            }
            else
            {
                characterController.height = crouchHeight;
                currentMovementSpeed = crouchSpeed;
                isCrouching = true;
            }
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
    }
    
    private IEnumerator ResetJumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }
    private IEnumerator DoCrouch(float targetHeight)
    {
        while (Mathf.Abs(characterController.height - targetHeight) > 0.01f)
        {
            characterController.height = Mathf.Lerp(characterController.height, targetHeight, crouchSpeed * Time.deltaTime);
            yield return null;
        }
        characterController.height = targetHeight;
    }
    
    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        if (invertY)
            verticalRotation += mouseY;
        else
            verticalRotation -= mouseY;
        
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    
    private void ApplyGravity()
    {
        if (characterController.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
        
        verticalVelocity += gravity * Time.deltaTime;
        Vector3 gravityMove = new Vector3(0, verticalVelocity, 0);
        characterController.Move(gravityMove * Time.deltaTime);
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
    private void HandleSlide()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && canSlide && characterController.isGrounded)
        {
            StartSlide();
        }
        
        if (stateManager.currentState == PlayerState.Sliding)
        {
            UpdateSlide();
        }
    }
    
    private void StartSlide()
    {
        stateManager.TransitionToState(PlayerState.Sliding);
        currentSlideTime = slideTime;
        canSlide = false;
        StartCoroutine(ResetSlideCooldown());
    }
    
    private void UpdateSlide()
    {
        currentSlideTime -= Time.deltaTime;
        if (currentSlideTime <= 0)
        {
            stateManager.TransitionToState(PlayerState.Walking);
        }
    }
    
    private IEnumerator ResetSlideCooldown()
    {
        yield return new WaitForSeconds(slideCooldown);
        canSlide = true;
    }
}

