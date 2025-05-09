using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Eğer TextMeshPro kullanıyorsan
using System.Linq;


public class ProductionUI : MonoBehaviour
{
    [Header("Üretilebilir Bina Tipleri")]
    [Tooltip("Buraya ScriptableObject tipi BuildingTypeData asset’lerinizi atayın")]
    [SerializeField] private BuildingTypeData[] types;

    [Header("UI Referansları")]
    [Tooltip("Scroll View → Viewport → Content objesi")]
    [SerializeField] private Transform content;
    [Tooltip("Üzerinde Button, Image (ikon) ve Text/TMP_Text (isim) bileşenleri olan prefab")]
    [SerializeField] private GameObject buttonPrefab;
    [Tooltip("Sahnedeki BuildingPlacer (BuildingSystem) objeniz")]
    [SerializeField] private BuildingPlacer placer;

    private void Start()
    {
        PopulateProductionMenu();
    }

    private void PopulateProductionMenu()
    {
        // —————— BURAYA EKLE ——————
        // Eğer daha önce eklenen butonlar kaldıysa temizle:
        foreach (Transform child in content)
            Destroy(child.gameObject);
        // ————————————————————————

        if (content == null)      Debug.LogError("ProductionUI: content Transform atanmamış!");
        if (buttonPrefab == null) Debug.LogError("ProductionUI: buttonPrefab atanmamış!");
        if (placer == null)       Debug.LogError("ProductionUI: placer atanmamış!");

        foreach (var t in types)
        {
            // parent = content, worldPositionStays = false
            GameObject btnGO = Instantiate(buttonPrefab, content, false);
            btnGO.transform.localScale = Vector3.one;

            var btn = btnGO.GetComponent<Button>();
            if (btn == null) Debug.LogError("Button component yok! Prefab’ı kontrol et.");

            // ——> Sadece Icon child’ındaki Image’ı alıyoruz:
            var iconTransform = btnGO.transform.Find("Icon");
            if (iconTransform == null)
            {
                Debug.LogError("ProductionUI: Prefab içinde 'Icon' adında child bulunamadı!");
                continue;
            }

            var iconImg = iconTransform.GetComponent<Image>();
            if (iconImg == null)
            {
                Debug.LogError("ProductionUI: 'Icon' child’da Image component yok!");
                continue;
            }

            // ——> TextMeshPro bileşenini doğrudan Label child’dan alalım:
            var labelTransform = btnGO.transform.Find("Label");
            if (labelTransform == null)
            {
                Debug.LogError("ProductionUI: Prefab içinde 'Label' adında child bulunamadı!");
                continue;
            }

            var label = labelTransform.GetComponent<TMP_Text>();
            if (label == null)
            {
                Debug.LogError("ProductionUI: 'Label' child’da TMP_Text component yok!");
                continue;
            }

            // Sonra atamalar:
            iconImg.sprite = t.buildingIcon;
            label.text     = t.buildingName;
            label.color    = Color.black;

            btn.onClick.AddListener(() => placer.StartPlacingBuilding(t));
        }
    }

}
