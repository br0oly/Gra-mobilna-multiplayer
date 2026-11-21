using UnityEngine;

public class EnenmyDie : MonoBehaviour
{
    [SerializeField] private EnemyHealth health;

    private void OnEnable()
    {
        health.OnEnemyDie += Destroy;
    }

    private void OnDisable()
    {
        health.OnEnemyDie -= Destroy;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
