using UnityEngine;

public class ArmorPickup : MonoBehaviour
{
    public float armorAmount = 50f;
    public GameObject pickupEffect;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
        {
            healthSystem.AddArmor(armorAmount);
            if (pickupEffect) Instantiate(pickupEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
