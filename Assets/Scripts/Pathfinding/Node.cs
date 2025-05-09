using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.Pathfinding
{
    public class Node
    {
        public bool IsWalkable;
        public Vector3 WorldPosition;
        public int GridX;
        public int GridY;

        public int GCost;  // Start’dan buraya kadarki maliyet
        public int HCost;  // Buradan hedefe tahmini maliyet
        public Node Parent;

        public int FCost => GCost + HCost;

        public Node(bool isWalkable, Vector3 worldPos, int gridX, int gridY)
        {
            IsWalkable = isWalkable;
            WorldPosition = worldPos;
            GridX = gridX;
            GridY = gridY;
        }
    }
}
