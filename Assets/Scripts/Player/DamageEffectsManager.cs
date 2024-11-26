using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageEffectsManager : MonoBehaviour
{
    [Header("Hit Indicator")]
    public Image hitIndicatorPrefab;
    public float hitIndicatorDuration = 0.5f;
    public float fadeSpeed = 2f;
    
    [Header("Screen Effects")]
    public Image damageVignette;
    public float maxVignetteAlpha = 0.8f;
    public float vignetteRecoveryRate = 1f;
    
    private HealthSystem healthSystem;
    private float currentVignetteAlpha;
    
    void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        damageVignette.color = new Color(1, 0, 0, 0);
    }
    
    void Update()
    {
        UpdateVignette();
    }
    
    public void ShowHitIndicator(Vector3 damageDirection)
    {
        Image indicator = Instantiate(hitIndicatorPrefab, transform);
        float angle = Mathf.Atan2(damageDirection.x, damageDirection.z) * Mathf.Rad2Deg;
        indicator.transform.rotation = Quaternion.Euler(0, 0, angle);
        
        StartCoroutine(FadeOutHitIndicator(indicator));
    }
    
    private void UpdateVignette()
    {
        float healthPercentage = healthSystem.currentHealth / healthSystem.maxHealth;
        float targetAlpha = (1 - healthPercentage) * maxVignetteAlpha;
        
        currentVignetteAlpha = Mathf.Lerp(currentVignetteAlpha, targetAlpha, Time.deltaTime * vignetteRecoveryRate);
        damageVignette.color = new Color(1, 0, 0, currentVignetteAlpha);
    }
    private IEnumerator FadeOutHitIndicator(Image indicator)
    {
        float elapsed = 0f;
        Color startColor = indicator.color;

        while (elapsed < hitIndicatorDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsed / hitIndicatorDuration);
            indicator.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        Destroy(indicator.gameObject);
    }
}
