// Assets/Scripts/Controller/BarracksProduction.cs
using System.Collections;
using UnityEngine;

public class BarracksProduction : MonoBehaviour
{
    [Header("Üretim Ayarları")]
    [Tooltip("Her üretim arasında geçen süre (saniye)")]
    [SerializeField] private float productionInterval = 5f;

    [Tooltip("Hangi asker tipi üretilecek (1,2 veya 3)")]
    [SerializeField] private int soldierType = 1;

    [Tooltip("Barracks prefab’ı altına ekleyeceğin 'SpawnPoint' adlı child Transform")]
    [SerializeField] private Transform spawnPoint;

    private Coroutine produceRoutine;

    private void Awake()
    {
        // Eğer inspector'dan atamadıysan, otomatik 'SpawnPoint' child’ını bul:
        if (spawnPoint == null)
            spawnPoint = transform.Find("SpawnPoint");

        if (spawnPoint == null)
            Debug.LogWarning("BarracksProduction: 'SpawnPoint' child bulunamadı, bu objede bir Transform kullanılacak.");

        if (SoldierSpawner.Instance == null)
            Debug.LogError("BarracksProduction: SoldierSpawner sahnede yok!");
    }

    private void OnEnable()
    {
        produceRoutine = StartCoroutine(ProductionLoop());
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

            // Spawn noktasını al (child yoksa barracks pozisyonu)
            Vector3 pos = spawnPoint != null
                ? spawnPoint.position
                : transform.position;

            // Askeri orada doğur:
            SoldierSpawner.Instance.SpawnSoldier(soldierType, pos);
        }
    }
}
