using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingHoverer : MonoBehaviour
{
    private Building hoveredBuilding;
    private Building selectedBuilding;

    void Update()
    {
        // UI üzerindeysek işlem yapma
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // Mouse altındaki binayı bul
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f;
        Collider2D col = Physics2D.OverlapPoint(worldPos);
        hoveredBuilding = col != null
            ? col.GetComponentInParent<Building>()
            : null;

        // Sol tıkla bina seçimi
        if (Input.GetMouseButtonDown(0))
        {
            if (hoveredBuilding != null)
            {
                selectedBuilding = hoveredBuilding;
            }
            else
            {
                selectedBuilding = null;
                UIManager.Instance.HideBuildingInfo();
                UIManager.Instance.ShowProductionPanel(false);
                UIManager.Instance.SetCurrentProductionSource(null);
            }
        }

        // Paneli güncelle (öncelik seçilende)
        Building display = selectedBuilding != null ? selectedBuilding : hoveredBuilding;

        if (display != null)
        {
            var model = display.GetModel();
            UIManager.Instance.ShowBuildingInfo(model.Name, model.Icon, model.Health);

            bool canProd = display.TypeData != null && display.TypeData.canProduceUnits;
            UIManager.Instance.ShowProductionPanel(canProd);

            if (selectedBuilding == display && canProd)
                UIManager.Instance.SetCurrentProductionSource(display);
        }
        else
        {
            UIManager.Instance.HideBuildingInfo();
            UIManager.Instance.ShowProductionPanel(false);
        }
    }
}
