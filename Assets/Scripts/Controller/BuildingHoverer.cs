using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingHoverer : MonoBehaviour
{
    private Building hoveredBuilding;
    private Building selectedBuilding;

    void Update()
    {
        // UI üzerindeysek hiçbir işlem yapma
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // Fare altındaki binayı tespit et
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f;
        Collider2D col = Physics2D.OverlapPoint(worldPos);
        hoveredBuilding = col != null 
            ? col.GetComponentInParent<Building>() 
            : null;

        // Sol tıkla seçim veya deselection yap
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
            }
        }

        // Eğer bir bina seçiliyse
        if (selectedBuilding != null)
        {
            var model = selectedBuilding.GetModel();
            UIManager.Instance.ShowBuildingInfo(
                model.Name,
                model.Icon,
                model.Health
            );
            bool canProd = selectedBuilding.TypeData != null
                           && selectedBuilding.TypeData.canProduceUnits;
            UIManager.Instance.ShowProductionPanel(canProd);
        }
        // Seçili yoksa ama hover’daysak geçici göster
        else if (hoveredBuilding != null)
        {
            var model = hoveredBuilding.GetModel();
            UIManager.Instance.ShowBuildingInfo(
                model.Name,
                model.Icon,
                model.Health
            );
            bool canProd = hoveredBuilding.TypeData != null
                           && hoveredBuilding.TypeData.canProduceUnits;
            UIManager.Instance.ShowProductionPanel(canProd);
        }
        else
        {
            // Ne seçili ne hover varsa tamamen gizle
            UIManager.Instance.HideBuildingInfo();
            UIManager.Instance.ShowProductionPanel(false);
        }
    }
}
