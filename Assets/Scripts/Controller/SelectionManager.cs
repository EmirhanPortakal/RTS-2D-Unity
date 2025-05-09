using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Canvas        uiCanvas;        // Inspector’dan atayabilirsin
    [SerializeField] private RectTransform selectionPanel; // Selection box’un RT
    [SerializeField] private GameObject    selfObject;      // selectionPanel’in root objesi

    public List<Soldier> allSoldiers      = new List<Soldier>();
    public List<Soldier> selectedSoldiers = new List<Soldier>();

    private Vector2 startLocal;   // UI‐local tıklama başlangıcı
    private Vector2 startScreen;  // Screen‐space tıklama başlangıcı
    private Camera  canvasCamera;
    private bool    isSelecting;

    private void Awake()
    {
        if (uiCanvas == null)
            uiCanvas = selectionPanel.GetComponentInParent<Canvas>();

        canvasCamera = uiCanvas.renderMode == RenderMode.ScreenSpaceCamera
                       ? uiCanvas.worldCamera
                       : null;

        // Pivot’ı sol-alt köşe yap
        selectionPanel.pivot = Vector2.zero;

        selfObject.SetActive(false);
    }

    private void Update()
    {
        // SOL TIK BAŞLAT
        if (Input.GetMouseButtonDown(0))
            BeginSelection();

        // SOL TIK SÜRÜKLE
        if (isSelecting && Input.GetMouseButton(0))
            UpdateSelectionBox();

        // SOL TIK BIRAK
        if (isSelecting && Input.GetMouseButtonUp(0))
            EndSelection();

        // SAĞ TIK: hareket veya saldırı
        if (Input.GetMouseButtonDown(1) && selectedSoldiers.Count > 0)
            HandleRightClick();
    }

    private void BeginSelection()
    {
        isSelecting    = true;
        selfObject.SetActive(true);
        selectionPanel.SetAsLastSibling();

        startScreen = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            uiCanvas.transform as RectTransform,
            startScreen,
            canvasCamera,
            out startLocal
        );

        selectionPanel.anchoredPosition = startLocal;
        selectionPanel.sizeDelta       = Vector2.zero;
    }

    private void UpdateSelectionBox()
    {
        Vector2 currentLocal;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            uiCanvas.transform as RectTransform,
            Input.mousePosition,
            canvasCamera,
            out currentLocal
        );

        Vector2 size = currentLocal - startLocal;
        selectionPanel.sizeDelta = new Vector2(
            Mathf.Abs(size.x),
            Mathf.Abs(size.y)
        );

        // Sürükleme yönüne göre kutuyu taşı
        Vector2 offset = new Vector2(
            size.x < 0 ? size.x : 0,
            size.y < 0 ? size.y : 0
        );
        selectionPanel.anchoredPosition = startLocal + offset;
    }

    private void EndSelection()
    {
        isSelecting    = false;
        selfObject.SetActive(false);
        PerformSelection();
    }

    private void PerformSelection()
    {
        selectedSoldiers.Clear();
        allSoldiers.Clear();
        allSoldiers.AddRange(FindObjectsOfType<Soldier>());

        Vector2 endScreen = Input.mousePosition;
        Vector2 min       = Vector2.Min(startScreen, endScreen);
        Vector2 max       = Vector2.Max(startScreen, endScreen);
        Rect    selRect   = new Rect(min, max - min);

        foreach (var s in allSoldiers)
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(s.transform.position);
            if (selRect.Contains(screenPos))
                selectedSoldiers.Add(s);
        }
    }

    private void HandleRightClick()
{
    Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    worldPos.z = 0f;

    // ─── DEBUG: Tıklanan noktadaki tüm collider’ları yakala ───
    float debugRadius = 0.1f;
    Collider2D[] hits = Physics2D.OverlapCircleAll(worldPos, debugRadius);
    Debug.Log($"[SelectionManager] HandleRightClick at {worldPos} found {hits.Length} colliders:");
    foreach (var h in hits)
        Debug.Log($"    • {h.gameObject.name} (Layer: {LayerMask.LayerToName(h.gameObject.layer)})");

    // ─── Damageable’ı bul ───
    Damageable dmg = null;
    foreach (var h in hits)
    {
        dmg = h.GetComponentInParent<Damageable>();
        if (dmg != null)
        {
            Debug.Log($"[SelectionManager] Found Damageable on '{h.gameObject.name}' → {dmg.gameObject.name}");
            break;
        }
    }

    if (dmg != null)
    {
        // Bina tıklandı: yürünebilir en yakın noktaya yolla ve saldırı başlat
        foreach (var soldier in selectedSoldiers)
        {
            Vector3 attackPos = soldier.GetNearestReachablePoint(dmg.transform.position);
            soldier.SetAttackTarget(dmg);
            soldier.MoveTo(attackPos);
        }
    }
    else
    {
        // Boş alana tıklanmış: saldırıyı iptal et, doğrudan oraya yolla
        float spread = 0.5f;
        int   idx    = 0;
        Debug.Log("[SelectionManager] No Damageable found—moving to point");
        foreach (var soldier in selectedSoldiers)
        {
            soldier.SetAttackTarget(null);
            Vector3 offset = new Vector3((idx % 3) * spread,
                                         (idx / 3) * spread,
                                         0);
            soldier.MoveTo(worldPos + offset);
            idx++;
        }
    }
}


}
