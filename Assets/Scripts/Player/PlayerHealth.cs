using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    private DamageReceived damage;

    public event Action<int, int> OnHealthChanged;
    public event Action OnPlayerLiving;
    public event Action OnPlayerDie;
    
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

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
        damage = GetComponent<DamageReceived>();
        currentHealth = maxHealth;
    }
    private void Start()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnPlayerLiving?.Invoke();
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
        OnPlayerDie?.Invoke();
    }
}
