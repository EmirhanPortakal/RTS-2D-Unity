using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierModel
{
    public string UnitName { get; }
    public int    Health   { get; }
    public int    Damage   { get; }
    public Sprite Icon     { get; }

    public SoldierModel(string unitName, int health, int damage, Sprite icon)
    {
        UnitName = unitName;
        Health   = health;
        Damage   = damage;
        Icon     = icon;
    }

    public SoldierModel(SoldierTypeData data)
        : this(data.typeName, data.health, data.damage, data.icon)
    {
    }
}

