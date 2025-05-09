// Assets/Scripts/Controller/SoldierSpawner.cs
using UnityEngine;

public class SoldierSpawner : MonoBehaviour
{
    public static SoldierSpawner Instance;

    [Header("Soldier Data Assets")]
    [Tooltip("ScriptableObject array: her biri bir asker tipini tanımlar")]
    [SerializeField] private SoldierTypeData[] soldierTypes;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Varsayılan pozisyon olarak spawner objesinin pozisyonunu kullanır.
    /// </summary>
    public void SpawnSoldier(SoldierTypeData data)
    {
        SpawnSoldier(data, transform.position);
    }

    /// <summary>
    /// Belirtilen pozisyonda data.prefab ile asker spawnlar.
    /// </summary>
    public void SpawnSoldier(SoldierTypeData data, Vector3 position)
    {
        if (data == null || data.prefab == null)
        {
            Debug.LogError("SoldierSpawner: Geçersiz SoldierTypeData veya prefab!");
            return;
        }

        GameObject soldierGO = Instantiate(data.prefab, position, Quaternion.identity);
        var soldier = soldierGO.GetComponent<Soldier>();
        if (soldier != null)
        {
            // Model'i data üzerinden oluştur
            soldier.Initialize(new SoldierModel(data));
        }
    }

    /// <summary>
    /// Index'e göre SoldierTypeData döndürür.
    /// </summary>
    public SoldierTypeData GetSoldierType(int index)
    {
        if (index < 0 || index >= soldierTypes.Length)
            return null;
        return soldierTypes[index];
    }

    /// <summary>
    /// Geri uyumluluk için int index ile spawn (pozisyon vererek)
    /// </summary>
    public void SpawnSoldier(int index, Vector3 position)
    {
        var data = GetSoldierType(index);
        if (data == null)
        {
            Debug.LogError($"SoldierSpawner: Geçersiz soldier index: {index}");
            return;
        }
        SpawnSoldier(data, position);
    }
}
