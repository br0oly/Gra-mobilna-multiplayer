using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{ 
    [Header("Input")]
    [SerializeField] private InputActionReference attackAction;

    [Header("Attack Range")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Vector2 attackRange = new Vector2(1, 1);
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask playerLayer;

    [Header("Attack Lock")]
    [SerializeField] private float attackLock = 2f;

    [Header("Damge")]
    [SerializeField] private int attackDamage = 25;

    private PlayerHealth health;
    private PlayerDeath death;

    private bool isLive;
    private bool isLockActive = false;

    public event Action<int> OnAttackHit;
    public event Action<int> OnHitEnemy;
    private void Awake()
    {
        death = GetComponent<PlayerDeath>();
        health = GetComponent<PlayerHealth>();
    }
    private void OnEnable()
    {
        health.OnPlayerLiving += IsPlayerLive;
        death.OnPlayerDeath += DeathState;
    }

    private void OnDisable()
    {
        health.OnPlayerLiving -= IsPlayerLive;
        death.OnPlayerDeath -= DeathState;
    }

    private void Update()
    {
        if (!isLive) return;

        if (attackAction.action.WasPressedThisFrame() && !isLockActive) 
        {
            isLockActive = true;
            Attack();
        }
    }

    private void Attack() 
    {
        if (CheckIsPlayerAttackInRange())
        {
            Debug.Log("trafi³em");
            OnAttackHit?.Invoke(attackDamage);
            StartCoroutine(AttackCooldown());
        }
        else if (CheckIsEnemyAttackInRange())
        {
            Debug.Log("trafi³em");
            OnHitEnemy?.Invoke(attackDamage);
            StartCoroutine(AttackCooldown());
        }
        else
        {
            Debug.Log("chybi³em");
            StartCoroutine(AttackCooldown());
        }
    }
    
    private bool CheckIsPlayerAttackInRange() 
    {
       return Physics2D.OverlapBox(
       attackPoint.position, 
        attackRange,
        0f,
        playerLayer
       ) != null;
    }

    private bool CheckIsEnemyAttackInRange()
    {
        return Physics2D.OverlapBox(
        attackPoint.position,
         attackRange,
         0f,
         enemyLayer
        ) != null;
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(
            attackPoint.position,
            attackRange
        );
    }
    IEnumerator AttackCooldown() 
    {
        yield return new WaitForSecondsRealtime(attackLock);

        isLockActive = false;
    }
    private void IsPlayerLive()
    {
        isLive = true;
    }
    private void DeathState()
    {
        isLive = false;
    }
}
