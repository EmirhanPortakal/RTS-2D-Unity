// Assets/Scripts/Controller/BarracksProduction.cs
using System.Collections;
using UnityEngine;

public class BarracksProduction : MonoBehaviour
{
    [Header("Üretim Ayarları")]
    [Tooltip("Varsayılan üretim süresi (eğer SoldierTypeData kullanılmazsa)")]
    [SerializeField] private float productionInterval = 5f;

    public float ProductionInterval => productionInterval;

    [Tooltip("Hangi asker tipi üretilecek (1,2 veya 3)")]
    [SerializeField] private int soldierType = 1;

    [Tooltip("Barracks prefab’ı altına ekleyeceğim 'SpawnPoint' adlı child Transform")]
    [SerializeField] private Transform spawnPoint;

    private Coroutine produceRoutine;

    private void Awake()
    {
        if (spawnPoint == null)
            spawnPoint = transform.Find("SpawnPoint");

        if (spawnPoint == null)
            Debug.LogWarning("BarracksProduction: 'SpawnPoint' child bulunamadı");

        if (SoldierSpawner.Instance == null)
            Debug.LogError("BarracksProduction: SoldierSpawner sahnede yok!");
    }

    private void OnEnable()
    {
        // Eğer otomatik üretim yapmak istersen burayı aç
        // produceRoutine = StartCoroutine(ProductionLoop());
    }

    private void OnDisable()
    {
        if (produceRoutine != null)
            StopCoroutine(produceRoutine);
    }

    private IEnumerator ProductionLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(productionInterval);
            SpawnSoldier();
        }
    }

    // Varsayılan asker üretimi (int soldierType)
    private void SpawnSoldier()
    {
        Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position;
        SoldierSpawner.Instance.SpawnSoldier(soldierType, pos);
    }

    // SoldierTypeData ile spawn
    public void SpawnSoldier(SoldierTypeData data)
    {
        if (data == null) return;
        Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position;
        SoldierSpawner.Instance.SpawnSoldier(data, pos);
    }


    public void ProduceSoldierWithCooldown(SoldierTypeData data, UnityEngine.UI.Button button)
    {
        if (!gameObject.activeInHierarchy || data == null) return;

        Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position;
        SoldierSpawner.Instance.SpawnSoldier(data, pos);

        if (button != null)
            StartCoroutine(ButtonCooldown(button, data.productionTime));
    }

    private IEnumerator ButtonCooldown(UnityEngine.UI.Button btn, float duration)
    {
        btn.interactable = false;
        yield return new WaitForSeconds(duration);
        btn.interactable = true;
    }
}
