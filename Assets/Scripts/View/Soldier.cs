using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class Soldier : MonoBehaviour
{
    private SoldierModel model;

    public void Initialize(SoldierModel soldierModel)
    {
        model = soldierModel;
        Debug.Log($"{model.UnitName} üretildi, hasar: {model.Damage}");
    }

    public SoldierModel GetModel()
    {
        return model;
    }
}

