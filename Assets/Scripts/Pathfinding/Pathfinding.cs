using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.Pathfinding
{
    public class Pathfinding : MonoBehaviour
    {
        public static Pathfinding Instance { get; private set; }
        private GridManager grid;

        private void Awake()
        {
            Instance = this;
            grid = GetComponent<GridManager>();
            if (grid == null)
                Debug.LogError("Pathfinding: GridManager component’i bulunamadı!");
        }

        public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
        {
            Node startNode = grid.NodeFromWorldPoint(startPos);
            Node targetNode = grid.NodeFromWorldPoint(targetPos);

            var openSet   = new List<Node> { startNode };
            var closedSet = new HashSet<Node>();

            startNode.GCost = 0;
            startNode.HCost = GetDistance(startNode, targetNode);

            while (openSet.Count > 0)
            {
                // fCost en düşük olanı al
                Node current = openSet[0];
                foreach (var node in openSet)
                    if (node.FCost < current.FCost ||
                    (node.FCost == current.FCost && node.HCost < current.HCost))
                        current = node;

                openSet.Remove(current);
                closedSet.Add(current);

                if (current == targetNode)
                    return RetracePath(startNode, targetNode);

                foreach (var neighbour in grid.GetNeighbours(current))
                {
                    if (!neighbour.IsWalkable || closedSet.Contains(neighbour))
                        continue;

                    int newCost = current.GCost + GetDistance(current, neighbour);
                    if (newCost < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.GCost = newCost;
                        neighbour.HCost = GetDistance(neighbour, targetNode);
                        neighbour.Parent = current;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }

            // Yol bulunamadıysa boş liste
            return new List<Node>();
        }

        private List<Node> RetracePath(Node start, Node end)
        {
            var path = new List<Node>();
            Node current = end;
            while (current != start)
            {
                path.Add(current);
                current = current.Parent;
            }
            path.Reverse();
            return path;
        }

        private int GetDistance(Node a, Node b)
        {
            int dx = Mathf.Abs(a.GridX - b.GridX);
            int dy = Mathf.Abs(a.GridY - b.GridY);
            // diagonal hareket maliyeti 14, düz maliyet 10
            if (dx > dy)
                return 14*dy + 10*(dx-dy);
            return 14*dx + 10*(dy-dx);
        }
    }

}