using System.Collections.Generic;
using UnityEngine;
using RTS.Pathfinding;

public class Soldier : MonoBehaviour
{
    private SoldierModel model;

    private List<Node> path;
    private int pathIndex;
    private float speed = 5f;

    private Damageable currentTarget;
    private float attackRange    = 1.5f;
    private float attackCooldown = 1f;
    private float attackTimer    = 0f;

    private GridManager gridManager;

    public void Initialize(SoldierModel soldierModel)
    {
        model = soldierModel;
    }

    public SoldierModel GetModel() => model;

    public void MoveTo(Vector3 position)
    {
        // A* ile yol bul ve başlat
        path      = Pathfinding.Instance.FindPath(transform.position, position);
        pathIndex = 0;
    }

    private void Awake()
    {
        // Sahnedeki GridManager’ı bul
        gridManager = FindObjectOfType<GridManager>();
    }

    private void Update()
    {
        // Dönmesini engelle
        transform.rotation = Quaternion.identity;
        // Varsa scale sapmasını düzelt
        transform.localScale = new Vector3(1f, 1f, 1f);
        // Eğer bir yol varsa düğümler boyunca hareket et
        if (path != null && pathIndex < path.Count)
        {
            Vector3 targetPos = path[pathIndex].WorldPosition;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPos) < 0.05f)
                pathIndex++;
            return;
        }

        // Saldırı kontrolü
        if (currentTarget != null)
        {
            float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
            if (distance <= attackRange)
            {
                // Menzildeyse saldır
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackCooldown)
                {
                    currentTarget.TakeDamage(model.Damage);
                    attackTimer = 0f;
                }
            }
            else
            {
                // Menzile girene kadar binanın etrafındaki yürünebilir en yakın noktaya yolla
                Vector3 attackPos = GetNearestReachablePoint(currentTarget.transform.position);
                MoveTo(attackPos);
            }
        }
    }

    public void SetAttackTarget(Damageable target)
    {
        currentTarget = target;
    }

    /// <summary>
    /// Verilen dünya konumu etrafındaki, grid'de yürünebilir en yakın hücreyi bulur.
    /// Eğer bulunamazsa doğrudan hedefin kendisini döner.
    /// </summary>
    public Vector3 GetNearestReachablePoint(Vector3 goalWorldPos)
    {
        if (gridManager == null)
            return goalWorldPos;

        Node goalNode = gridManager.NodeFromWorldPoint(goalWorldPos);
        if (goalNode == null)
            return goalWorldPos;

        Node best = null;
        float bestDist = float.MaxValue;

        // 4 yönlü komşuları kontrol et
        int gx = goalNode.GridX;
        int gy = goalNode.GridY;
        var deltas = new (int dx, int dy)[]
        {
            ( 1,  0),
            (-1,  0),
            ( 0,  1),
            ( 0, -1)
        };

        foreach (var (dx, dy) in deltas)
        {
            Node neighbor = gridManager.GetNode(gx + dx, gy + dy);
            if (neighbor != null && neighbor.IsWalkable)
            {
                float d = Vector3.SqrMagnitude(neighbor.WorldPosition - transform.position);
                if (d < bestDist)
                {
                    bestDist = d;
                    best = neighbor;
                }
            }
        }

        return best != null ? best.WorldPosition : goalWorldPos;
    }
}
