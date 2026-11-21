using System;
using UnityEngine;

public class PushBack : MonoBehaviour
{
    [SerializeField] private float backForce;

    private DamageReceived attack;
    private Rigidbody2D body;

    public event Action OnPushBack;

    private void Awake()
    {
        attack = GetComponent<DamageReceived>();
        body = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        attack.OnHitBack += HandlePushBack;
    }

    private void OnDisable()
    {
        attack.OnHitBack -= HandlePushBack;
    }

    private void HandlePushBack() 
    {
        body.AddForce(Vector2.left * backForce, ForceMode2D.Impulse);
        OnPushBack?.Invoke();
    }
}
