using UnityEngine;
using UnityEngine.UI;

public class HealtBar : MonoBehaviour
{
    [SerializeField] private PlayerHealth health;
    [SerializeField] private Image healthBar;

    private void OnEnable()
    {
        health.OnHealthChanged += HandleHealthChanged;
        HandleHealthChanged(health.CurrentHealth, health.MaxHealth);
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(int current, int max)
    {
        healthBar.fillAmount = max > 0
            ? Mathf.Clamp01((float)current / max)
            : 0f;
    }
}
