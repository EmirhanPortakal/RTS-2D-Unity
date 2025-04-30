using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] private GameObject buildingPrefab;
    private GameObject currentBuilding;
    private bool isPlacing = false;

    private SpriteRenderer currentRenderer;
    private Color validColor = new Color(0, 1, 0, 0.5f);     // Yeşil transparan
    private Color invalidColor = new Color(1, 0, 0, 0.5f);   // Kırmızı transparan


public void StartPlacingBuilding()
{
    if (buildingPrefab == null) return;

    // 1. Prefab'ı instantiate et
    currentBuilding = Instantiate(buildingPrefab);

    // 2. SpriteRenderer referansını al ve rengi ayarla
    currentRenderer = currentBuilding.GetComponent<SpriteRenderer>();
    currentRenderer.color = validColor;

    // 🔥 3. Building script'ine eriş ve model ile başlat
    var building = currentBuilding.GetComponent<Building>();

    string buildingName = building.defaultName;
    int buildingHP = building.defaultHP;
    BuildingType type = building.buildingType;

    building.Initialize(new BuildingModel(buildingName, buildingHP, type));




    // 4. Yerleştirme modu aktif
    isPlacing = true;
}



    private void Update()
    {
    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    if (currentBuilding != null)
    {
    currentBuilding.transform.position = mousePos;



    // Çarpışma kontrolü
    Collider2D[] hits = Physics2D.OverlapBoxAll(
        currentBuilding.transform.position,
        currentBuilding.GetComponent<Collider2D>().bounds.size,
        0f
    );

    bool isBlocked = false;
    foreach (var hit in hits)
    {
        if (hit.gameObject != currentBuilding)
        {
            isBlocked = true;
            break;
        }
    }

    // Renk güncellemesi
    currentRenderer.color = isBlocked ? invalidColor : validColor;

    // Yerleştirme onayı
    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
    {
        if (!isBlocked)
        {
            currentRenderer.color = Color.white; // Normal hale dön
            currentBuilding = null;
            isPlacing = false;
        }
        else
        {
            Debug.Log("Yerleştirilemez bölge.");
        }
    }

    }
}
}

