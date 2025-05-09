using UnityEngine;

[CreateAssetMenu(
  fileName = "NewSoldierType",
  menuName = "RTS/Soldier Type Data",
  order = 1)]
public class SoldierTypeData : ScriptableObject
{
    [Header("Genel Bilgiler")]
    public string   typeName;      // Örn: "Weak", "Medium", "Strong"
    public Sprite   icon;          // UI’da gösterilecek ikon
    public GameObject prefab;      // Instantiate edilecek prefab

    [Header("Stat’ler")]
    public int      health;        // SoldierModel.Health
    public int      damage;        // SoldierModel.Damage
    public float    productionTime = 5f;  // İsteğe bağlı, üretim süresi
}
