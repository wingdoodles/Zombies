using UnityEngine;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float regenDelay = 5f;
    public float regenRate = 10f;
    
    [Header("Armor Settings")]
    public float maxArmor = 150f;
    public float currentArmor;
    public float armorDamageReduction = 0.4f;
    
    [Header("Armor Break Effects")]
    public GameObject armorBreakEffect;
    public AudioClip armorBreakSound;
    private AudioSource audioSource;

    private bool isRegenerating = false;
    private float lastDamageTime;
    
    void Start()
    {
        currentHealth = maxHealth;
        currentArmor = 0f;
    }
    
    void Update()
    {
        if (Time.time > lastDamageTime + regenDelay && currentHealth < maxHealth && !isRegenerating)
        {
            StartCoroutine(RegenerateHealth());
        }
    }
    
    public void TakeDamage(float damage, DamageType damageType = DamageType.Normal)
    {
        float finalDamage = damage;
        
        if (currentArmor > 0)
        {
            float damageToArmor = damage * armorDamageReduction;
            currentArmor -= damageToArmor;
            finalDamage -= damageToArmor;
            
            if (currentArmor < 0) currentArmor = 0;
        }
        
        currentHealth -= finalDamage;
        lastDamageTime = Time.time;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private IEnumerator RegenerateHealth()
    {
        isRegenerating = true;
        
        while (currentHealth < maxHealth)
        {
            currentHealth += regenRate * Time.deltaTime;
            if (currentHealth > maxHealth) currentHealth = maxHealth;
            yield return null;
        }
        
        isRegenerating = false;
    }
    public void AddArmor(float amount)
    {
        currentArmor = Mathf.Min(currentArmor + amount, maxArmor);
    }
    
    private void HandleArmorBreak()
    {
        if (armorBreakEffect) Instantiate(armorBreakEffect, transform.position, Quaternion.identity);
        if (armorBreakSound) audioSource.PlayOneShot(armorBreakSound);
    }
    private void Die()
    {
        // Implement death logic
        Debug.Log("Player died!");
    }
}

public enum DamageType
{
    Normal,
    Fire,
    Explosive,
    Poison
}


