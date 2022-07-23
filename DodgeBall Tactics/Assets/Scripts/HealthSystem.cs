using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int health = 100;

    public event EventHandler OnDead;

    public void TakeDamage(int damageAmmount)
    {
        health -= damageAmmount;

        if (health < 0)
        {
            health = 0;
        }
        if(health == 0)
        {
            Die();
        }

        print(transform + " Health left: " + health);
    }
    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }
}
