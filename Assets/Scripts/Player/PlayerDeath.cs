using System;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private PlayerHealth die;

    public event Action OnPlayerDeath;

    private void Awake()
    {
        die = GetComponent<PlayerHealth>();
    }

    private void OnEnable()
    {
        die.OnPlayerDie += HandleDeathState;
    }

    private void OnDisable()
    {
        die.OnPlayerDie -= HandleDeathState;
    }

    private void HandleDeathState() 
    {
        OnPlayerDeath?.Invoke();
    }
}
