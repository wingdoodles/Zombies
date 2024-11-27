using UnityEngine;
public class WeaponStateMachine : MonoBehaviour
{
    private WeaponState currentState = WeaponState.Idle;
    private WeaponBase weapon;
    private Animator animator;
    private bool isAiming = false;
    private float adsSpeed = 8f;
    private Vector3 hipPosition;
    private Vector3 adsPosition;

    private void Start()
    {
        weapon = GetComponent<WeaponBase>();
        animator = GetComponent<Animator>();
        hipPosition = transform.localPosition;
        adsPosition = hipPosition + (Vector3.forward * 0.3f);
    }
    private void Update()
    {
        HandleStateUpdate();
        HandleInput();
        
        if (isAiming)
        {
            weapon.baseAccuracy *= 1.5f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, adsPosition, Time.deltaTime * adsSpeed);
        }
        else 
        {
            weapon.baseAccuracy = weapon.defaultAccuracy;
            transform.localPosition = Vector3.Lerp(transform.localPosition, hipPosition, Time.deltaTime * adsSpeed);
        }
    }
    

    private void HandleStateUpdate()
    {
        switch (currentState)
        {
            case WeaponState.Idle:
                HandleIdleState();
                break;
            case WeaponState.ADS:
                HandleADSState();
                break;
            case WeaponState.Firing:
                HandleFiringState();
                break;
            case WeaponState.Reloading:
                HandleReloadingState();
                break;
        }
    }
    private void HandleIdleState()
    {
        isAiming = false;
        transform.localPosition = Vector3.Lerp(transform.localPosition, hipPosition, Time.deltaTime * adsSpeed);
    }

    private void HandleADSState()
    {
        isAiming = true;
        transform.localPosition = Vector3.Lerp(transform.localPosition, adsPosition, Time.deltaTime * adsSpeed);
    }

    private void HandleFiringState()
    {
        if (weapon != null)
        {
            weapon.Fire();
        }
    }

    private void HandleReloadingState()
    {
        if (animator != null)
        {
            animator.SetTrigger("Reload");
        }
    }
    private void HandleInput()
    {
        // ADS Input
        if (Input.GetButtonDown("Fire2")) // Right mouse button
        {
            TransitionTo(WeaponState.ADS);
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            TransitionTo(WeaponState.Idle);
        }

        // Firing Input
        if (Input.GetButton("Fire1")) // Left mouse button
        {
            TransitionTo(WeaponState.Firing);
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            TransitionTo(WeaponState.Idle);
        }

        // Reload Input
        if (Input.GetKeyDown(KeyCode.R))
        {
            TransitionTo(WeaponState.Reloading);
        }
    }
    private void EnterState(WeaponState state)
    {
        switch (state)
        {
            case WeaponState.ADS:
                if (animator) animator.SetBool("IsAiming", true);
                weapon.OnADSEnter();
                break;
            case WeaponState.Firing:
                if (animator) animator.SetBool("IsFiring", true);
                weapon.OnFiringEnter();
                break;
            case WeaponState.Reloading:
                if (animator) animator.SetTrigger("StartReload");
                weapon.OnReloadStart();
                break;
        }
    }

    private void ExitState(WeaponState state)
    {
        switch (state)
        {
            case WeaponState.ADS:
                if (animator) animator.SetBool("IsAiming", false);
                weapon.OnADSExit();
                break;
            case WeaponState.Firing:
                if (animator) animator.SetBool("IsFiring", false);
                weapon.OnFiringExit();
                break;
            case WeaponState.Reloading:
                if (animator) animator.SetTrigger("EndReload");
                weapon.OnReloadComplete();
                break;
        }
    }
    public void TransitionTo(WeaponState newState)
    {
        if (currentState == newState) return;
        
        ExitState(currentState);
        currentState = newState;
        EnterState(newState);
    }
}


