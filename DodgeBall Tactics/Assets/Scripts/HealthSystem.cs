using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int health = 100;

    public event EventHandler OnDead, OnDamaged;

    private int healthMax;
    private void Start()
    {
        healthMax = health;
    }
    public void TakeDamage(int damageAmmount)
    {
        health -= damageAmmount;

        if (health < 0)
        {
            health = 0;
        }
        OnDamaged?.Invoke(this, EventArgs.Empty);

        if(health == 0)
        {
            Die();
        }
    }
    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }
    public float GetHealthNormalized() => (float)health / healthMax;
}
