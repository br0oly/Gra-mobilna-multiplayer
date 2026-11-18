using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    [SerializeField] private DamageReceived damage;

    public event Action<int, int> OnHealthChanged;
    public event Action OnEnemyDie;

    private void OnEnable()
    {
        damage.OnDamageReceived += TakeDamage;
    }

    private void OnDisable()
    {
        damage.OnDamageReceived -= TakeDamage;
    }
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth = Math.Max(0, currentHealth - amount);

        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnEnemyDie?.Invoke();
    }
}
