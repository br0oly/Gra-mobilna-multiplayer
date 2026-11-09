using UnityEngine;
using System;


public class BattleManager : MonoBehaviour
{
    [SerializeField] private EnemyAi enemy;
    [SerializeField] private PlayerAttack player;

    public event Action<int> OnDamageRecedivePlayer;
    public static event Action<int> OnDamageReceivedEnemy;

    private void OnEnable()
    {
        enemy.OnEnemyAttackHit += HandleEnemyAttack;
    }

    private void OnDisable()
    {
        enemy.OnEnemyAttackHit -= HandleEnemyAttack;
    }
    
    public void HandleEnemyAttack(int damage)
    {
        
    }
}
