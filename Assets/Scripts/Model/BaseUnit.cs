using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUnit
{
    public string UnitName { get; protected set; }
    public int Health { get; protected set; }
    public int Damage { get; protected set; }

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        // Override edilebilir
    }
}

