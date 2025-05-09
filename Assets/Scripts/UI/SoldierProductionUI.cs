using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoldierProductionUI : MonoBehaviour
{
    [SerializeField] private SoldierTypeData[] types;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject soldierButtonPrefab;
    [SerializeField] private SoldierSpawner spawner;

    private void Start()
    {
        Populate();
    }

    private void Populate()
    {
        foreach (Transform ch in content)
            Destroy(ch.gameObject);

        foreach (var t in types)
        {
            var go = Instantiate(soldierButtonPrefab, content, false);
            go.transform.localScale = Vector3.one;

            // Icon
            go.transform.Find("Icon")
              .GetComponent<Image>().sprite = t.icon;

            // Name
            go.transform.Find("NameText")
              .GetComponent<TMP_Text>().text = t.typeName;

            // HP
            go.transform.Find("HPText")
              .GetComponent<TMP_Text>().text = $"HP: {t.health}";

            // Damage
            go.transform.Find("DmgText")
              .GetComponent<TMP_Text>().text = $"DMG: {t.damage}";

            // Hook up spawn
            go.GetComponent<Button>()
              .onClick.AddListener(() => spawner.SpawnSoldier(t));
        }
    }
}
