using UnityEngine;


public enum BuildingType
{
    Barracks,
    PowerPlant,
}

public class BuildingModel
{
    public string       Name   { get; }
    public int          Health { get; }
    public BuildingType Type   { get; }
    public Sprite       Icon   { get; }

    /// <summary>
    /// Ana kurucu: tüm değerleri manuel olarak verirsiniz.
    /// </summary>
    public BuildingModel(string name, int health, BuildingType type, Sprite icon)
    {
        Name   = name;
        Health = health;
        Type   = type;
        Icon   = icon;
    }

    /// <summary>
    /// BuildingTypeData’dan otomatik oluşturacak kolay kurucu.
    /// </summary>
    public BuildingModel(BuildingTypeData data)
        : this(
            data.buildingName,
            data.defaultHP,
            data.buildingType,
            data.buildingIcon
          )
    {
    }
}
