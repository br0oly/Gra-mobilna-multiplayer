using System;
using System.Collections;
using UnityEngine;

public class EnemyDamageReceived : MonoBehaviour
{
    [SerializeField] private float damageCooldown;
    [SerializeField] private PlayerAttack damage;

    private bool isAttackHit = false;
    private int finalDamage;

    public event Action<int> OnEnemyDamageReceived;
    public event Action OnHitBack;
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
        OnHitBack?.Invoke();
        if (!isAttackHit)
        { 
            OnEnemyDamageReceived?.Invoke(amount);

            isAttackHit = true;
            StartCoroutine(StartDamageCooldown());
        }
    }

    IEnumerator StartDamageCooldown()
    {
        yield return new WaitForSecondsRealtime(damageCooldown);
        isAttackHit = false;
    }
}
