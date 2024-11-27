using UnityEngine;
using System.Collections;


public enum WeaponType
{
    AssaultRifle,
    SMG,
    Shotgun,
    Sniper,
    LMG,
    Pistol,
    Special
}

public class WeaponBase : MonoBehaviour
{
    [Header("Weapon Identity")]
    public string weaponName = "Default Weapon";
    public WeaponType weaponType;
    
    [Header("Weapon Stats")]
    public float baseDamage = 25f;
    public float headshotMultiplier = 1.5f;
    public float fireRate = 600f; // Rounds per minute
    public float effectiveRange = 100f;
    public float maxRange = 200f;
    
    [Header("Accuracy & Recoil")]
    public float baseAccuracy = 0.9f; // 1 = perfect accuracy
    public float defaultAccuracy = 0.9f;

    public Vector2[] recoilPattern; // Array of recoil offsets
    public float recoilRecoverySpeed = 1f;
    
    [Header("Movement Effects")]
    public float moveSpeed = 5f;
    public float weaponWeight = 1f; // Affects movement speed
    public float adsMovementMultiplier = 0.8f;

    [Header("Firing System")]
    public float timeBetweenShots;
    private float nextTimeToFire;
    
    [Header("State")]
    private bool isFiring = false;
    private bool isReloading = false;

    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public ParticleSystem impactEffect;
    public AudioClip fireSound;
    public AudioClip emptySound;
    private AudioSource audioSource;

    [Header("Ammunition")]
    public int magazineSize = 30;
    public int currentAmmo;
    public int reserveAmmo = 90;
    public float reloadTime = 2.0f;
    public AmmoType ammoType;

    [Header("Audio")]
    public AudioClip reloadSound;

    private WeaponState currentState = WeaponState.Idle;
    private Camera mainCamera;
    private int currentRecoilIndex = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mainCamera = Camera.main;
        timeBetweenShots = 60f / fireRate;
    }
    public float CalculateDamage(float distance, bool isHeadshot)
    {
        float damage = baseDamage;
        
        // Apply damage falloff based on distance
        if (distance > effectiveRange)
        {
            float falloffMultiplier = 1f - ((distance - effectiveRange) / (maxRange - effectiveRange));
            damage *= Mathf.Clamp01(falloffMultiplier);
        }
        
        // Apply headshot multiplier
        if (isHeadshot)
        {
            damage *= headshotMultiplier;
        }
        
        return damage;
    }
    
    public float CalculateAccuracy()
    {
        // Base accuracy affected by movement, stance, etc.
        float currentAccuracy = baseAccuracy;
        
        // Additional accuracy modifiers can be added here
        
        return Mathf.Clamp01(currentAccuracy);
    }
    public virtual void Fire()
    {
        if (isReloading || !isFiring) return;
        
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + timeBetweenShots;
            
            RaycastHit hit;
            Vector3 direction = CalculateSpread();
            
            if (Physics.Raycast(transform.position, direction, out hit, maxRange))
            {
                HandleHit(hit);
            }
            
            ApplyRecoil();
        }
    }

    public virtual void Update()
    {
        // Use currentState for state-based behavior
        if (currentState == WeaponState.ADS)
        {
            // ADS specific behavior
        }
    }

    private Vector3 CalculateSpread()
    {
        float accuracy = CalculateAccuracy();
        Vector3 spread = Random.insideUnitSphere * (1f - accuracy);
        return transform.forward + spread;
    }

    private void HandleHit(RaycastHit hit)
    {
        float distance = Vector3.Distance(transform.position, hit.point);
        bool isHeadshot = hit.collider.CompareTag("Head");
        float damage = CalculateDamage(distance, isHeadshot);
        
        // Apply damage to target
        IDamageable target = hit.collider.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }
    }
    private void ApplyRecoil()
    {
        if (recoilPattern != null && recoilPattern.Length > 0)
        {
            // Apply recoil based on pattern
            Vector2 currentRecoil = recoilPattern[currentRecoilIndex];
            // Rotate weapon/camera based on recoil values
            transform.localRotation *= Quaternion.Euler(-currentRecoil.y, currentRecoil.x, 0);
            
            currentRecoilIndex = (currentRecoilIndex + 1) % recoilPattern.Length;
        }
    }

    public interface IDamageable
    {
        void TakeDamage(float damage);
    }

    private void PlayWeaponEffects()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        if (audioSource != null && fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
    }


    private void PlayImpactEffect(RaycastHit hit)
    {
        if (impactEffect != null)
        {
            ParticleSystem impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact.gameObject, 2f);
        }
    }
    public virtual void OnADSEnter()
    {
        baseAccuracy *= 1.5f;
        moveSpeed *= adsMovementMultiplier;
        PlayWeaponEffects();
    }

    public virtual void OnADSExit()
    {
        baseAccuracy /= 1.5f;
        moveSpeed /= adsMovementMultiplier;
    }

    public virtual void OnFiringEnter()
    {
        isFiring = true;
        PlayWeaponEffects();
    }

    public virtual void OnFiringExit()
    {
        isFiring = false;
    }

    public virtual void OnReloadStart()
    {
        isReloading = true;
        if (audioSource && reloadSound)
            audioSource.PlayOneShot(reloadSound);
    }

    public virtual void OnReloadComplete()
    {
        isReloading = false;
        RefillAmmo();
    }

    private void RefillAmmo()
    {
        int ammoNeeded = magazineSize - currentAmmo;
        int ammoToAdd = Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToAdd;
        reserveAmmo -= ammoToAdd;
    }

}
