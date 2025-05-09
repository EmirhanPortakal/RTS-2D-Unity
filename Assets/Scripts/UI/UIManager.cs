using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Info Panel Bileşenleri")]
    [SerializeField] private TMP_Text buildingNameText;
    [SerializeField] private TMP_Text buildingHPText;
    [SerializeField] private Image    buildingIconImage;

    [Header("Production Panel")]
    [SerializeField] private GameObject productionPanel;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// İkona sahip olan hal: hem isim, hem ikon, hem HP gösterir.
    /// </summary>
    public void ShowBuildingInfo(string name, Sprite icon, int hp)
    {
        buildingNameText.text     = name;
        buildingHPText.text       = $"HP: {hp}";
        buildingIconImage.sprite  = icon;
        buildingIconImage.enabled = icon != null;
    }

    /// <summary>
    /// Eski imza: ikon gösterimi yok.
    /// </summary>
    public void ShowBuildingInfo(string name, int hp)
    {
        ShowBuildingInfo(name, null, hp);
    }

    /// <summary>
    /// Info paneli temizler ve ikonu gizler.
    /// </summary>
    public void HideBuildingInfo()
    {
        buildingNameText.text     = "";
        buildingHPText.text       = "";
        buildingIconImage.sprite  = null;
        buildingIconImage.enabled = false;
    }

    /// <summary>
    /// Production panelini aç/kapa.
    /// </summary>
    public void ShowProductionPanel(bool show)
    {
        productionPanel.SetActive(show);
    }
}
