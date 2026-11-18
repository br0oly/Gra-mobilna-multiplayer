using System;
using UnityEngine;

public class DamageReceived : MonoBehaviour
{
    [SerializeField] private int armor = 5;
    [SerializeField] private PlayerAttack damage;

    private int finalDamage;

    public event Action<int> OnDamageReceived;
    private void OnEnable()
    {
        damage.OnAttackHit += HandleDamageReceived;
    }

    private void OnDisable()
    {
        damage.OnAttackHit -= HandleDamageReceived;
    }

    private void HandleDamageReceived(int amount) 
    {
        finalDamage = Mathf.Max(1, amount - armor);

        OnDamageReceived(finalDamage);
    }
}
