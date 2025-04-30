using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingModel model;

    public string defaultName = "Barracks";
    public int defaultHP = 100;
    public BuildingType buildingType = BuildingType.Barracks;

    public void Initialize(BuildingModel m)
    {
        model = m;
    }

    public BuildingModel GetModel()
    {
        if (model != null)
            return model;

        // Yedek model oluştur
        return new BuildingModel(defaultName, defaultHP, buildingType);
    }

}

