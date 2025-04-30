using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSpawner : MonoBehaviour
{
    [SerializeField] private GameObject soldier1Prefab;
    [SerializeField] private GameObject soldier2Prefab;
    [SerializeField] private GameObject soldier3Prefab;

    public void SpawnSoldier(int type)
    {
        GameObject prefab = type switch
        {
            1 => soldier1Prefab,
            2 => soldier2Prefab,
            3 => soldier3Prefab,
            _ => null
        };

        if (prefab == null) return;

        var soldierGO = Instantiate(prefab, Vector3.zero, Quaternion.identity); // TODO: Spawn noktasını bağla
        var model = SoldierFactory.CreateSoldier(type);
        soldierGO.GetComponent<Soldier>().Initialize(model);
    }
}

