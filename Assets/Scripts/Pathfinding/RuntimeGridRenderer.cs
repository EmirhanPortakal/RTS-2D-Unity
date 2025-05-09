using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RuntimeGridRenderer : MonoBehaviour
{
    [Header("Grid Ayarları")]
    public Vector2 gridWorldSize = new Vector2(20, 20);
    public float    cellSize      = 1f;
    public Color    lineColor     = Color.white;
    public Material lineMaterial;    // Unlit/Color tipi bir material atayın

    private int cols, rows;
    private Vector3 origin;

    void Awake()
    {
        cols = Mathf.CeilToInt(gridWorldSize.x / cellSize);
        rows = Mathf.CeilToInt(gridWorldSize.y / cellSize);
        // Eğer kendini (GridManager) ortalamak istersen:
        origin = new Vector3(-gridWorldSize.x, -gridWorldSize.y, 0) * 0.5f;
    }

    void OnPostRender()
    {
        if (lineMaterial == null) return;
        lineMaterial.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Color(lineColor);

        // Dikey çizgiler
        for (int x = 0; x <= cols; x++)
        {
            Vector3 p0 = origin + Vector3.right * x * cellSize;
            Vector3 p1 = p0 + Vector3.up * rows * cellSize;
            GL.Vertex(p0);
            GL.Vertex(p1);
        }

        // Yatay çizgiler
        for (int y = 0; y <= rows; y++)
        {
            Vector3 p0 = origin + Vector3.up * y * cellSize;
            Vector3 p1 = p0 + Vector3.right * cols * cellSize;
            GL.Vertex(p0);
            GL.Vertex(p1);
        }

        GL.End();
    }
}

