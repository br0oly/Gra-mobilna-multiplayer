using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DamageReceived : MonoBehaviour
{
    [SerializeField] private int armor = 5;
    [SerializeField] private float damageCooldown;
    [SerializeField] private PlayerAttack damage;

    private bool isAttackHit = false;
    private int finalDamage;
    private int fatalDamage = 100;

    public event Action<int> OnDamageReceived;
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
            finalDamage = Mathf.Max(1, amount - armor);
            OnDamageReceived?.Invoke(finalDamage);

            isAttackHit = true;
            StartCoroutine(StartDamageCooldown());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Damage")) 
        {
            OnDamageReceived(fatalDamage);
        }
    }

    IEnumerator StartDamageCooldown() 
    {
        yield return new WaitForSecondsRealtime(damageCooldown);
        isAttackHit = false;
    }
}
