using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float health = 100.0f;
    public event Action<string, float> HealthChanged;
    public event Action<string> Death;
    
    private bool _isDead;

    public void ChangeHealth(float delta)
    {
        health += delta;
        HealthChanged?.Invoke(gameObject.name, health);
        if (health <= 0 && !_isDead)
        {
            Death?.Invoke(gameObject.name);
            _isDead = true;
        }
    }
}
