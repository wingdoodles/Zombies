using UnityEngine;

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
    public Vector2[] recoilPattern; // Array of recoil offsets
    public float recoilRecoverySpeed = 1f;
    
    [Header("Movement Effects")]
    public float weaponWeight = 1f; // Affects movement speed
    public float adsMovementMultiplier = 0.8f;

    [Header("Firing System")]
    public float timeBetweenShots;
    private float nextTimeToFire;
    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public ParticleSystem impactEffect;
    public AudioClip fireSound;
    public AudioClip emptySound;
    private AudioSource audioSource;


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
    private void PlayWeaponEffects()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();
            
        if (audioSource != null && fireSound != null)
            audioSource.PlayOneShot(fireSound);
    }
    
    private void PlayImpactEffect(RaycastHit hit)
    {
        if (impactEffect != null)
        {
            ParticleSystem impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact.gameObject, 2f);
        }
    }

}





