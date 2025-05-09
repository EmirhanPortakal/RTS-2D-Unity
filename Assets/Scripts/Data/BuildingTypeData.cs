using UnityEngine;

[CreateAssetMenu(fileName = "BuildingType", menuName = "RTS/BuildingTypeData")]
public class BuildingTypeData : ScriptableObject
{
    [Header("Tanım")]
    public string      buildingName;
    public Sprite      buildingIcon;
    public GameObject  prefab;
    public Vector2Int  size = new Vector2Int(1,1);
    public bool        canProduceUnits = false;

    [Header("Runtime Stats")]
    [Tooltip("Bu binanın HP değeri")]
    public int         defaultHP       = 100;         // EKLENDİ

    [Tooltip("Bu binanın tipi (enum BuildingType)")]
    public BuildingType buildingType;                // EKLENDİ
}
