using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    [SerializeField] private EnemyDamageReceived damage;

    public event Action OnEnemyDie;

    private void OnEnable()
    {
        damage.OnEnemyDamageReceived += TakeDamage;
    }

    private void OnDisable()
    {
        damage.OnEnemyDamageReceived -= TakeDamage;
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
