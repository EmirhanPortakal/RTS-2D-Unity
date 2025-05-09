using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Eğer yoksa, namespace’inizi ekleyin
public class SoldierModel
{
    public string UnitName { get; }
    public int    Health   { get; }
    public int    Damage   { get; }
    public Sprite Icon     { get; }

    // <<< YENİ: 4-parametreli ctor
    public SoldierModel(string unitName, int health, int damage, Sprite icon)
    {
        UnitName = unitName;
        Health   = health;
        Damage   = damage;
        Icon     = icon;
    }

    // <<< İsteğe bağlı: ScriptableObject’den kolay ctor
    public SoldierModel(SoldierTypeData data)
        : this(data.typeName, data.health, data.damage, data.icon)
    {
    }
}

