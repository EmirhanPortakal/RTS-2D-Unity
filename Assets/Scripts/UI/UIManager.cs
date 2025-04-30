using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private TMP_Text buildingNameText;
    [SerializeField] private TMP_Text buildingHPText;

    [SerializeField] private GameObject productionPanel;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowBuildingInfo(string name, int hp)
    {
        buildingNameText.text = name;
        buildingHPText.text = $"HP: {hp}";
    }

    public void HideBuildingInfo()
    {
        buildingNameText.text = "";
        buildingHPText.text = "";
    }

    public void ShowProductionPanel(bool show)
{
    productionPanel.SetActive(show);
}
}
