using System;
using System.Collections;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Chase,
    Attack
}
public class EnemyAi : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    [Header("Damage")]
    [SerializeField] private int damage;

    [Header("Range")]
    [SerializeField] private int aggroRadius;
    [SerializeField] private int attackRadius;

    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Lock")]
    [SerializeField] private float attackLockDuration;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Vector2 attackRange = new Vector2(1, 1);

    private SpriteRenderer spriteRenderer;
    private bool attackLock;

    public event Action<int> OnAttackHit;

    public EnemyState CurrentState { get; private set; }
        = EnemyState.Idle;

    private float aggroRadiusSqr;
    private float attackRadiusSqr;
    private bool isHit;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        aggroRadiusSqr = aggroRadius * aggroRadius;

        attackRadiusSqr = attackRadius * attackRadius;
    }

    private void Update()
    {
        if (player == null) return;

        switch (CurrentState)
        {
            case EnemyState.Idle:
                UpdateIdle();
                break;

            case EnemyState.Chase:
                UpdateChase();
                break;

            case EnemyState.Attack:
                UpdateAttack();
                break;
        }
    }

    private bool IsPlayerInAggroRange()
    {
        Vector3 toPlayer = player.position - transform.position;

        return toPlayer.sqrMagnitude <= aggroRadiusSqr;
    }


    private bool IsPlayerInAttackRange()
    {
        Vector3 toPlayer = player.position - transform.position;

        return toPlayer.sqrMagnitude <= attackRadiusSqr;
    }

    private void UpdateIdle()
    {
        if (IsPlayerInAggroRange())
        {
            ChangeState(EnemyState.Chase);
            return;
        }
    }

    private void UpdateChase()
    {
        if (!IsPlayerInAggroRange())
        {
            ChangeState(EnemyState.Idle);
            return;
        }

        if (IsPlayerInAttackRange())
        {
            ChangeState(EnemyState.Attack);
        }

        Chase();
    }
    private void UpdateAttack()
    {
        if (!IsPlayerInAttackRange())
        {
            ChangeState(EnemyState.Chase);
            return;
        }

        Attack();
    }

    private void LookAtPlayer()
    {
        spriteRenderer.flipX = player.position.x > transform.position.x;
    }

    private void ChangeState(EnemyState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
    }

    private void Chase()
    {
        LookAtPlayer();

        transform.position = Vector3.MoveTowards(transform.position, player.position,moveSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        if (attackLock) return;
        if (CheckIsAttackInRange()) 
        {
            Debug.Log("trafi³em");
            OnAttackHit?.Invoke(damage);
            attackLock = true;
            StartCoroutine(AttackLock());
        }
    }

    private bool CheckIsAttackInRange()
    {
        return Physics2D.OverlapBox(
            attackPoint.position,
            attackRange,
            0f,
            playerLayer
        ) != null;
    }

    IEnumerator AttackLock() 
    {
        yield return new WaitForSecondsRealtime(attackLockDuration);

        attackLock = false;
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
}
