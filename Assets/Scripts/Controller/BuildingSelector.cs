using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelector : MonoBehaviour
{
    public void BeginPlay()
    {
        Debug.Log(UIManager.Instance);

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Sol tıklama
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Building building = hit.collider.GetComponent<Building>();
                if (building != null)
                {
                    var model = building.GetModel();
                    if (model != null && UIManager.Instance != null)
                    {
                        UIManager.Instance.ShowBuildingInfo(model.Name, model.Health);

                        // Sadece barracks ise üretim panelini aç
                        bool isBarracks = model.Type == BuildingType.Barracks;
                        UIManager.Instance.ShowProductionPanel(isBarracks);
                    }
                }
                else
                {
                    UIManager.Instance.HideBuildingInfo();
                }
            }
            else
            {
                UIManager.Instance.HideBuildingInfo();
            }
        }
    }
}
