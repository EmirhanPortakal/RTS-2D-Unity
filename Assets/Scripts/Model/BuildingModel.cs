using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType { Barracks, PowerPlant }

public class BuildingModel
{
    public string Name { get; private set; }
    public int Health { get; private set; }
    public BuildingType Type { get; private set; }

    public BuildingModel(string name, int health, BuildingType type)
    {
        Name = name;
        Health = health;
        Type = type;
    }
}

