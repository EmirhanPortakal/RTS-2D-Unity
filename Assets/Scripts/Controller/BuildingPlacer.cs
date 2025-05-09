using UnityEngine;
using UnityEngine.EventSystems;
using RTS.Pathfinding;  // GridManager, Node

public class BuildingPlacer : MonoBehaviour
{
    [Header("Grid Manager")]
    [SerializeField] private GridManager gridManager;

    private BuildingTypeData placingData;
    private GameObject        currentBuilding;
    private SpriteRenderer    currentRenderer;
    private bool              isPlacing;

    private Color validColor   = new Color(0, 1, 0, 0.5f);
    private Color invalidColor = new Color(1, 0, 0, 0.5f);

    /// <summary>
    /// UI butonundan çağırılır. Data’dan prefab’ı instantiate eder,
    /// önizleme rengini ayarlar ve modelle initialize eder.
    /// </summary>
    public void StartPlacingBuilding(BuildingTypeData data)
    {
        placingData    = data;
        currentBuilding = Instantiate(data.prefab);
        currentRenderer = currentBuilding.GetComponent<SpriteRenderer>();
        currentRenderer.color = validColor;

        // Model’i ata
        var buildingComponent = currentBuilding.GetComponent<Building>();
        buildingComponent.Initialize(new BuildingModel(placingData));

        isPlacing = true;
        var buildingComp = currentBuilding.GetComponent<Building>();
        buildingComp.SetData(data);
        buildingComp.Initialize(new BuildingModel(data));

    }

    private void Update()
    {
        if (!isPlacing || currentBuilding == null) 
            return;

        // Fare pozisyonunu grid hücresine snap et
        Vector3 worldMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldMouse.z = 0f;
        Node baseNode = gridManager.NodeFromWorldPoint(worldMouse);
        if (baseNode == null) return;

        currentBuilding.transform.position = baseNode.WorldPosition;

        // Footprint kontrolü
        bool canPlace = true;
        int  w        = placingData.size.x;
        int  h        = placingData.size.y;
        int  startX   = baseNode.GridX - w / 2;
        int  startY   = baseNode.GridY - h / 2;

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                var n = gridManager.GetNode(startX + x, startY + y);
                if (n == null || !n.IsWalkable)
                {
                    canPlace = false;
                    break;
                }
            }
            if (!canPlace) break;
        }

        // Rengi güncelle
        currentRenderer.color = canPlace ? validColor : invalidColor;

        // Yerleştir onayı: sol tık + UI dışında
        if (canPlace
            && Input.GetMouseButtonDown(0)
            && !EventSystem.current.IsPointerOverGameObject())
        {
            // 1) Seçilen hücreleri engelliye al
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    gridManager.SetWalkable(startX + x, startY + y, false);

            // 2) Binaya footprint bilgisini ve origin’i kaydet
            var buildingComp = currentBuilding.GetComponent<Building>();
            buildingComp.InitPlacement(
                gridManager,
                baseNode.GridX,
                baseNode.GridY,
                placingData.size
            );

            // 3) Önizlemeyi finalize et
            currentRenderer.color = Color.white;
            currentBuilding       = null;
            isPlacing             = false;
        }
    }
}
