using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    public event Action<int, int> OnHealthChanged;
    
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    private void Start()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth = Math.Max(0, currentHealth - amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
