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

            Button btn = go.GetComponent<Button>();

            btn.onClick.AddListener(() =>
            {
                var currentBuilding = UIManager.Instance.GetCurrentProductionSource();
                var bp = currentBuilding?.GetComponentInChildren<BarracksProduction>();

                if (bp != null)
                {
                    float remaining = bp.GetCooldownRemaining(t);
                    if (remaining > 0f)
                    {
                        Debug.Log("Cooldown devam ediyor");
                    }
                    else
                    {
                        bp.ProduceSoldierWithCooldown(t);
                    }
                }
            });

        }
    }

    private void Update()
    {
        var currentBuilding = UIManager.Instance.GetCurrentProductionSource();
        var bp = currentBuilding?.GetComponentInChildren<BarracksProduction>();
        if (bp == null) return;

        for (int i = 0; i < content.childCount; i++)
        {
            var btn = content.GetChild(i).GetComponent<Button>();
            var cdTxt = btn.transform.Find("CooldownText")?.GetComponent<TMP_Text>();
            var data = types[i];

            float remaining = bp.GetCooldownRemaining(data);
            if (cdTxt != null)
            {
                if (remaining > 0)
                {
                    cdTxt.text = $"{Mathf.CeilToInt(remaining)}s";
                    btn.interactable = false;
                }
                else
                {
                    cdTxt.text = "";
                    btn.interactable = true;
                }
            }
        }
    }


    private IEnumerator CooldownRoutine(float duration, Button btn, TMP_Text cdTxt)
    {
        btn.interactable = false;
        float timer = duration;

        while (timer > 0)
        {
            cdTxt.text = $"{timer:F1}s";
            timer -= Time.deltaTime * 0.75f; // cooldown daha yavaş aksın
            yield return null;
        }

        cdTxt.text = "";
        btn.interactable = true;
    }

}
