using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSelector : MonoBehaviour
{
    private Soldier selectedSoldier;

    private void Update()
    {
        // Sol tık ile seçim
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Soldier soldier = hit.collider.GetComponent<Soldier>();
                if (soldier != null)
                {
                    selectedSoldier = soldier;
                    Debug.Log($"Seçilen asker: {soldier.GetModel().UnitName}");
                }
            }
        }

        // Sağ tık ile hedef belirleme
        if (Input.GetMouseButtonDown(1) && selectedSoldier != null)
        {
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = 0;

            selectedSoldier.MoveTo(target);
        }
    }
}

