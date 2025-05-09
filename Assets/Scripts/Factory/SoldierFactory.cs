using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierFactory : MonoBehaviour
{
    [SerializeField] private Transform       spawnPoint;
    [SerializeField] private SoldierTypeData[] soldierTypes;

    /// <summary>
    /// Tek bir SoldierTypeData asset’inden asker oluşturur ve geri döner.
    /// </summary>
    public Soldier CreateSoldier(SoldierTypeData data)
    {
        if (data == null || data.prefab == null) return null;

        GameObject go = Instantiate(data.prefab, spawnPoint.position, Quaternion.identity);
        var soldier = go.GetComponent<Soldier>();
        soldier.Initialize(new SoldierModel(data));   // veya 4-param ctor
        return soldier;
    }

    // Eğer index ile çağrılan Produce metonuz varsa, onun içinde de CreateSoldier’ı kullanabilirsiniz:
    public void Produce(int dataIndex)
    {
        if (dataIndex < 0 || dataIndex >= soldierTypes.Length) return;
        CreateSoldier(soldierTypes[dataIndex]);
    }
}


