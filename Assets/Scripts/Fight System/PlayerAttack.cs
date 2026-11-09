using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private InputActionReference attackAction;
    [SerializeField] private int damage;
    private bool isAttacking;
    private bool isLockActive;
    private bool isHit;
    private float attackLock = 2f;

    public event Action<int> OnPlayerAttackHit;

    private void Update()
    {
        if (isLockActive) return;
        if (attackAction.action.WasPressedThisFrame()) 
        {
            isAttacking = true;
            Attack();
        }
    }

    private void Attack() 
    {
        if (isAttacking)
        {
            if (isHit) 
            {
                OnPlayerAttackHit?.Invoke(damage);
            }
            isAttacking = false;
            isLockActive = true;
            StartCoroutine(AttackCooldown());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            isHit = true;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            isHit = true;
        }
        else if (collision.gameObject.CompareTag("PlayerB")) 
        {
            isHit = true;
        }
    }
    IEnumerator AttackCooldown() 
    {
        yield return new WaitForSecondsRealtime(attackLock);
        isLockActive = false;
    }
}
