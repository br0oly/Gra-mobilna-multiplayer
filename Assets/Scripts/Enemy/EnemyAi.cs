using System;
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

    public event Action<int> OnEnemyAttackHit;

    public EnemyState CurrentState { get; private set; }
        = EnemyState.Idle;

    private float aggroRadiusSqr;
    private float attackRadiusSqr;
    private bool isHit;

    private void Awake()
    {
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

    private void LookAt()
    {
        transform.LookAt(transform.position);
    }

    private void ChangeState(EnemyState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
    }

    private void Chase()
    {
        transform.LookAt(player.position);

        transform.position = Vector3.MoveTowards(transform.position, player.position,moveSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        if (isHit) 
        {
            OnEnemyAttackHit?.Invoke(damage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerA") || collision.gameObject.CompareTag("PlayerB")) 
        {
            isHit = true;
        }
    }
}
