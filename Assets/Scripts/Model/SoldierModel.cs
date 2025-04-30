using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierModel : BaseUnit
{
    public SoldierModel(string name, int damage)
    {
        UnitName = name;
        Health = 10;
        Damage = damage;
    }
}
