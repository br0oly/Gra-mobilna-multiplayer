using System;
using UnityEngine;


public enum BossState
{
    Idle,
    Chase,
    Attack
}
public class BossAI : MonoBehaviour
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

    public event Action<int> OnBossAttackHit;

    public BossState CurrentState { get; private set; } 
        = BossState.Idle;

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
            case BossState.Idle:
                UpdateIdle();
                break;

            case BossState.Chase:
                UpdateChase();
                break;

            case BossState.Attack:
                UpdateAttack();
                break;
        }
    }

    private bool IsPlayerInAggroRange()
    {
        Vector3 toPlayerA = player.position - transform.position;

        return toPlayerA.sqrMagnitude <= aggroRadiusSqr;
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
            ChangeState(BossState.Chase);
                return;
        }
    }

    private void UpdateChase() 
    {
        if (!IsPlayerInAggroRange()) 
        {
            ChangeState(BossState.Idle);
            return;
        }

        if (IsPlayerInAttackRange()) 
        {
            ChangeState(BossState.Attack);
        }

        Chase();
    }
    private void UpdateAttack() 
    {
        if (!IsPlayerInAttackRange()) 
        {
            ChangeState(BossState.Chase);
            return;
        }

        Attack();
    }

    private void LookAt() 
    {
        transform.LookAt(transform.position);
    }

    private void ChangeState(BossState newState) 
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
    }

    private void Chase()
    {
        transform.LookAt(player.position);

        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }


    private void Attack()
    {
        if (isHit) 
        {
            OnBossAttackHit?.Invoke(damage);
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
