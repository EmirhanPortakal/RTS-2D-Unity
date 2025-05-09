using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.Pathfinding
{
    public class GridManager : MonoBehaviour
    {
        [Header("Grid Ayarları")]
        [SerializeField] private Vector2 gridWorldSize = new Vector2(20, 20);
        [SerializeField] private float nodeRadius = 0.5f;
        [SerializeField] private LayerMask unwalkableMask;

        private Node[,] grid;
        private float nodeDiameter;
        private int gridSizeX, gridSizeY;

        private void Awake()
        {
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            CreateGrid();
        }

        private void CreateGrid()
        {
            grid = new Node[gridSizeX, gridSizeY];
            Vector3 worldBottomLeft = transform.position 
                - Vector3.right * gridWorldSize.x / 2 
                - Vector3.up    * gridWorldSize.y / 2;

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft 
                        + Vector3.right * (x * nodeDiameter + nodeRadius) 
                        + Vector3.up    * (y * nodeDiameter + nodeRadius);

                    bool walkable = !Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask);
                    grid[x, y] = new Node(walkable, worldPoint, x, y);
                }
            }
        }

        public List<Node> GetNeighbours(Node node)
        {
            var neighbours = new List<Node>();

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    int checkX = node.GridX + dx;
                    int checkY = node.GridY + dy;
                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                        neighbours.Add(grid[checkX, checkY]);
                }
            }
            return neighbours;
        }

        public Node NodeFromWorldPoint(Vector3 worldPos)
        {
            float percentX = Mathf.Clamp01((worldPos.x + gridWorldSize.x/2) / gridWorldSize.x);
            float percentY = Mathf.Clamp01((worldPos.y + gridWorldSize.y/2) / gridWorldSize.y);
            int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
            int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
            return grid[x, y];
        }

        // Geliştirme aşamasında görebilmek için
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
            if (grid == null) return;
            foreach (var node in grid)
            {
                Gizmos.color = node.IsWalkable ? Color.white : Color.red;
                Gizmos.DrawCube(node.WorldPosition, Vector3.one * (nodeDiameter - 0.05f));
            }
        }

        public Node GetNode(int x, int y)
        {
            if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
                return grid[x, y];
            return null;
        }

        public void SetWalkable(int x, int y, bool walkable)
        {
            var n = GetNode(x, y);
            if (n != null)
                n.IsWalkable = walkable;
        }

    }
}

