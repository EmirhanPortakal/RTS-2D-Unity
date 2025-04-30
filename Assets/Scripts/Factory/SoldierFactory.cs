using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoldierFactory
{
    public static SoldierModel CreateSoldier(int type)
    {
        return type switch
        {
            1 => new SoldierModel("Soldier 1", 10),
            2 => new SoldierModel("Soldier 2", 5),
            3 => new SoldierModel("Soldier 3", 2),
            _ => null
        };
    }
}

