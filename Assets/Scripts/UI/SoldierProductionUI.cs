using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SoldierProductionUI : MonoBehaviour
{
    [SerializeField] private SoldierTypeData[] types;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject soldierButtonPrefab;

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
            go.transform.Find("Icon").GetComponent<Image>().sprite = t.icon;

            // Name
            go.transform.Find("NameText").GetComponent<TMP_Text>().text = t.typeName;

            // HP
            go.transform.Find("HPText").GetComponent<TMP_Text>().text = $"HP: {t.health}";

            // Damage
            go.transform.Find("DmgText").GetComponent<TMP_Text>().text = $"DMG: {t.damage}";

            var cdTxt = go.transform.Find("CooldownText").GetComponent<TMP_Text>();

            cdTxt.text = "";

            // Button
            Button btn = go.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                var currentBuilding = UIManager.Instance.GetCurrentProductionSource();
                var bp = currentBuilding?.GetComponentInChildren<BarracksProduction>();
                Debug.Log("UIManager.Instance: " + UIManager.Instance);
                Debug.Log("CurrentBuilding: " + currentBuilding);

                if (bp != null)
                {
                    bp.ProduceSoldierWithCooldown(t, btn); // soldier üret
                    StartCoroutine(CooldownRoutine(t.productionTime, btn, cdTxt)); // yazılı cooldown
                }
                else
                {
                    Debug.LogWarning("Seçilen bina Barracks değil. Bina: " + currentBuilding?.name);
                }
            });
        }
    }

    private IEnumerator CooldownRoutine(float duration, Button btn, TMP_Text cdTxt)
    {
        btn.interactable = false;
        float timer = duration;

        while (timer > 0)
        {
            cdTxt.text = $"{Mathf.CeilToInt(timer)}s";

            timer -= Time.deltaTime;
            yield return null;
        }

        cdTxt.text = "";
        btn.interactable = true;
    }
}
