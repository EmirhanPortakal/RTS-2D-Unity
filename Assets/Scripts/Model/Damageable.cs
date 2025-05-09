using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
[SerializeField] private int maxHealth = 100;
private int currentHealth;

[SerializeField] private HealthBar healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetTarget(transform);
            healthBar.UpdateHealth(1f);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} {amount} hasar aldı! Kalan: {currentHealth}");
        float percent = Mathf.Clamp01((float)currentHealth / maxHealth);

        if (healthBar != null)
            healthBar.UpdateHealth(percent);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} yok edildi!");
        Destroy(gameObject);
    }
}


