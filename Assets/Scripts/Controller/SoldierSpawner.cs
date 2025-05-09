using System;
using UnityEngine;

public class SoldierSpawner : MonoBehaviour
{
    public static SoldierSpawner Instance { get; private set; }

    [Header("Üretilebilir Asker Tipleri")]
    [Tooltip("ScriptableObject tipi SoldierTypeData asset’lerinizi atayın")]
    [SerializeField] private SoldierTypeData[] soldierTypes;

    [Header("Spawn Noktası")]
    [SerializeField] private Transform spawnPoint;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Data’yı doğrudan vererek asker üretir.
    /// </summary>
    public void SpawnSoldier(SoldierTypeData data)
    {
        if (data == null || data.prefab == null)
        {
            Debug.LogError("SoldierSpawner: Geçersiz SoldierTypeData!");
            return;
        }

        // Sahnedeki spawn noktasında yarat ve modele data’yı geç
        var go = Instantiate(data.prefab, spawnPoint.position, Quaternion.identity);
        var soldier = go.GetComponent<Soldier>();
        soldier.Initialize(new SoldierModel(data)); 
    }

    /// <summary>
    /// Index ve pozisyon vererek üretim (eğer belirli bir noktaya spawnlamak isterseniz).
    /// </summary>
    public void SpawnSoldier(int type, Vector3 position)
    {
        // Eğer UI’dan 1,2,3 geliyorsa 0-based dizimize uyarlamak için:
        int index = type - 1;

        if (index < 0 || index >= soldierTypes.Length)
        {
            Debug.LogError($"SoldierSpawner: Geçersiz index ({type})!");
            return;
        }

        var data = soldierTypes[index];
        if (data.prefab == null)
        {
            Debug.LogError($"SoldierSpawner: {data.name} için prefab atanmamış!");
            return;
        }

        var go = Instantiate(data.prefab, position, Quaternion.identity);
        var soldier = go.GetComponent<Soldier>();
        soldier.Initialize(new SoldierModel(data));
    }

}
