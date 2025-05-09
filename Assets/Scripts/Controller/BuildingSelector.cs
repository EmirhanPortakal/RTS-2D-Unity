using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingSelector : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                var building = hit.collider.GetComponent<Building>();
                if (building != null)
                {
                    var model = building.GetModel();
                    if (model != null && UIManager.Instance != null)
                    {
                        // önce eski imzayı değil, ikonlu imzayı çağırıyoruz:
                        UIManager.Instance.ShowBuildingInfo(
                            model.Name,
                            model.Icon,
                            model.Health
                        );
                    }
                    return;
                }
            }

            // Hiçbir bina seçilmediyse paneli gizle
            UIManager.Instance.HideBuildingInfo();
        }
    }
}

