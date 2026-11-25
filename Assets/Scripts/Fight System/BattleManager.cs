using System;
using UnityEngine;

public enum FightArena 
{
    Waiting,
    Fight,
    Finished
}
public class BattleManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerA;
    [SerializeField] private PlayerHealth playerB;
    [SerializeField] private Vector3 playerAStartPos;
    [SerializeField] private Vector3 playerBStartPos;

    public event Action OnFightStart;

    public FightArena CurrentState { get; private set; } = FightArena.Waiting;

    private void Awake()
    {
        playerAStartPos = playerA.transform.position;
        playerBStartPos = playerB.transform.position;

        CurrentState = FightArena.Waiting;
    }
    private void OnEnable()
    {
        playerA.OnPlayerDie += HandlePlayerADeath;
        playerB.OnPlayerDie += HandlePlayerBDeath;
    }

    private void Start()
    {
        OnFightStart?.Invoke();
        StartFight();
    }
    private void OnDisable()
    {
        playerA.OnPlayerDie -= HandlePlayerADeath;
        playerB.OnPlayerDie -= HandlePlayerBDeath;
    }

    private void StartFight() 
    {
        CurrentState = FightArena.Fight;
    }
    private void HandlePlayerADeath() 
    {
        if (CurrentState != FightArena.Fight) return;

        CurrentState = FightArena.Finished;
        FinishFight("Plaeyr B");
    }

    private void HandlePlayerBDeath() 
    {
        if (CurrentState != FightArena.Fight) return;

            CurrentState = FightArena.Finished;
            FinishFight("Plaeyr A");
    }

    private void FinishFight(string name) 
    {
        if (CurrentState == FightArena.Finished) return;

        Debug.Log("Zwyciêsc¹ Pojedynku zostaje" + name);
    }
}