// Assets/Scripts/Controller/BarracksProduction.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarracksProduction : MonoBehaviour
{
    [Header("Üretim Ayarları")]
    [Tooltip("Varsayılan üretim süresi (eğer SoldierTypeData kullanılmazsa)")]
    [SerializeField] private float productionInterval = 5f;

    public float ProductionInterval => productionInterval;

    [Tooltip("Hangi asker tipi üretilecek (1,2 veya 3)")]
    [SerializeField] private int soldierType = 1;

    [Tooltip("Barracks prefab’ı altına ekleyeceğin 'SpawnPoint' adlı child Transform")]
    [SerializeField] private Transform spawnPoint;

    private Coroutine produceRoutine;

    // 🔥 SoldierTypeData -> kalan cooldown süresi
    private Dictionary<SoldierTypeData, float> cooldowns = new();

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

    private void SpawnSoldier()
    {
        Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position;
        SoldierSpawner.Instance.SpawnSoldier(soldierType, pos);
    }

    public void SpawnSoldier(SoldierTypeData data)
    {
        if (data == null) return;
        Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position;
        SoldierSpawner.Instance.SpawnSoldier(data, pos);
    }

    // ✅ Asker üretimi UI’dan çağrılır
    public void ProduceSoldierWithCooldown(SoldierTypeData data, Button button)
    {
        if (!gameObject.activeInHierarchy || data == null) return;

        if (IsOnCooldown(data)) return;

        Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position;
        SoldierSpawner.Instance.SpawnSoldier(data, pos);

        cooldowns[data] = data.productionTime;

        if (button != null)
            StartCoroutine(ButtonCooldown(button, data.productionTime));
    }

    // ✅ Button cooldown görseli
    private IEnumerator ButtonCooldown(Button btn, float duration)
    {
        btn.interactable = false;
        yield return new WaitForSeconds(duration);
        btn.interactable = true;
    }

    // ✅ Bu asker türü üretim cooldown’unda mı?
    public bool IsOnCooldown(SoldierTypeData data)
    {
        return cooldowns.ContainsKey(data);
    }

    public void ProduceSoldierWithCooldown(SoldierTypeData data)
    {
        if (data == null || GetCooldownRemaining(data) > 0f)
            return;

        Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position;
        SoldierSpawner.Instance.SpawnSoldier(data, pos);
        cooldowns[data] = Time.time + data.productionTime;
    }

    public float GetCooldownRemaining(SoldierTypeData data)
    {
        if (cooldowns.TryGetValue(data, out float readyAt))
        {
            float remaining = readyAt - Time.time;
            return Mathf.Max(0f, remaining);
        }

        return 0f;
    }


    private void Update()
    {
        if (cooldowns.Count == 0) return;

        var keys = new List<SoldierTypeData>(cooldowns.Keys);
        foreach (var key in keys)
        {
            cooldowns[key] -= Time.deltaTime;
            if (cooldowns[key] <= 0f)
                cooldowns.Remove(key);
        }
    }
}
