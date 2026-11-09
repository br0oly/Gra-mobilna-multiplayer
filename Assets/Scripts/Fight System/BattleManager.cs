using UnityEngine;
using System;


public class BattleManager : MonoBehaviour
{
    [SerializeField] private EnemyAi enemy;
    [SerializeField] private BossAI boss;
    [SerializeField] private PlayerAttack player;

    public event Action<int> OnDamageRecedivePlayer;
    public static event Action<int> OnDamageReceivedEnemy;
    public static event Action<int> OnDamageReceivedBoss;


    private void OnEnable()
    {
        enemy.OnEnemyAttackHit += HandleEnemyAttack;
        boss.OnBossAttackHit += HandleBoosAttack;
    }

    private void OnDisable()
    {
        enemy.OnEnemyAttackHit -= HandleEnemyAttack;
        boss.OnBossAttackHit -= HandleBoosAttack;
    }
    
    public void HandleEnemyAttack(int damage)
    {
        
    }

    public void HandleBoosAttack(int damage)
    {
        
    }
}
