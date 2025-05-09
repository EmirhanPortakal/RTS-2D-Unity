using UnityEngine;
using RTS.Pathfinding;  // Eğer GridManager ve Node’lar bu namespace içindeyse

public class Building : MonoBehaviour
{
    private BuildingModel model;

    [Header("Default Settings")]
    public string       defaultName  = "Barracks";
    public int          defaultHP    = 100;
    public BuildingType buildingType = BuildingType.Barracks;

    public BuildingTypeData TypeData { get; private set; }

    private GridManager _gridManager;
    private Vector2Int  _origin;
    private Vector2Int  _size;

    public void InitPlacement(GridManager gridManager, int gridX, int gridY, Vector2Int size)
    {
        _gridManager = gridManager;
        _origin      = new Vector2Int(gridX, gridY);
        _size        = size;
    }

    public void SetData(BuildingTypeData data)
    {
        TypeData = data;
    }

    public void Initialize(BuildingModel m)
    {
        model = m;
    }

    public BuildingModel GetModel()
    {
        if (model != null)
            return model;

        Sprite sprite = GetComponent<SpriteRenderer>()?.sprite;
        model = new BuildingModel(
            defaultName,
            defaultHP,
            buildingType,
            sprite
        );
        return model;
    }

    private void OnDestroy()
    {
        if (_gridManager == null) return;

        int halfW = _size.x / 2;
        int halfH = _size.y / 2;
        for (int x = 0; x < _size.x; x++)
            for (int y = 0; y < _size.y; y++)
                _gridManager.SetWalkable(_origin.x - halfW + x,
                                        _origin.y - halfH + y,
                                        true);
    }
}
