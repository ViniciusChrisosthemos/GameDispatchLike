using System;
using System.Collections.Generic;
using UnityEngine;

public class UIRadarChartController : MonoBehaviour
{
    [Header("Refer�ncias Principais")]
    [SerializeField] private CanvasRenderer _canvasRenderer; // Mesh principal
    [SerializeField] private Material _radarChartMaterial;
    [SerializeField] private Transform _middleReference;
    [SerializeField] private Transform _topReference;

    [Header("Border (optional)")]
    [SerializeField] private bool _drawBorder = true;
    [SerializeField] private CanvasRenderer _borderRenderer;
    [SerializeField] private Material _borderMaterial;
    [SerializeField] private float _borderThickness = 1f;

     // CanvasRenderer da borda
    private Mesh _mainMesh;
    private Mesh _borderMesh;

    private Vector3[] _vertices;

    private void Awake()
    {
        // Cria dinamicamente um segundo CanvasRenderer para a borda
        GameObject borderObj = new GameObject("RadarChartBorder");
        borderObj.transform.SetParent(transform, false);
    }

    public void UpdateStats(List<float> values)
    {
        if (values == null || values.Count < 3)
        {
            Debug.LogWarning("RadarChart precisa de pelo menos 3 valores para formar um pol�gono.");
            return;
        }

        int valuesAmount = values.Count;
        float angleIncrement = 360f / valuesAmount;
        float radarChartSize = Mathf.Abs(_middleReference.position.y - _topReference.position.y);

        // --- MESH PRINCIPAL ---
        _mainMesh = new Mesh();

        _vertices = new Vector3[valuesAmount + 1];
        Vector2[] uvs = new Vector2[valuesAmount + 1];
        int[] triangles = new int[3 * valuesAmount];

        _vertices[0] = Vector3.zero;

        for (int i = 1; i <= valuesAmount; i++)
        {
            float normalizedStat = Mathf.Clamp01(values[i - 1]);
            _vertices[i] = Quaternion.Euler(0, 0, -angleIncrement * (i - 1)) * Vector3.up * radarChartSize * normalizedStat;
        }

        int counter = 0;
        for (int i = 1; i <= valuesAmount; i++)
        {
            triangles[counter++] = 0;
            triangles[counter++] = i;
            triangles[counter++] = (i % valuesAmount) + 1;
        }

        _mainMesh.vertices = _vertices;
        _mainMesh.uv = uvs;
        _mainMesh.triangles = triangles;

        // Renderiza o pol�gono principal
        _canvasRenderer.SetMesh(_mainMesh);
        _canvasRenderer.SetMaterial(_radarChartMaterial, null);

        // --- BORDA SEPARADA ---
        if (_drawBorder && _borderMaterial != null)
            DrawBorder(_vertices);
        else
            _borderRenderer.Clear();
    }

    private void DrawBorder(Vector3[] vertices)
    {
        if (vertices.Length < 2)
            return;

        if (_borderMesh == null)
            _borderMesh = new Mesh();

        List<Vector3> borderVertices = new List<Vector3>();
        List<int> borderTriangles = new List<int>();

        for (int i = 1; i < vertices.Length; i++)
        {
            Vector3 current = vertices[i];
            Vector3 next = vertices[i == vertices.Length - 1 ? 1 : i + 1];

            Vector3 dir = (next - current).normalized;
            Vector3 normal = new Vector3(-dir.y, dir.x, 0) * _borderThickness;

            int startIndex = borderVertices.Count;

            // quatro v�rtices para cada segmento da borda
            borderVertices.Add(current - normal);
            borderVertices.Add(current + normal);
            borderVertices.Add(next - normal);
            borderVertices.Add(next + normal);

            // dois tri�ngulos (quad)
            borderTriangles.Add(startIndex);
            borderTriangles.Add(startIndex + 1);
            borderTriangles.Add(startIndex + 2);

            borderTriangles.Add(startIndex + 1);
            borderTriangles.Add(startIndex + 3);
            borderTriangles.Add(startIndex + 2);
        }

        _borderMesh.Clear();
        _borderMesh.SetVertices(borderVertices);
        _borderMesh.SetTriangles(borderTriangles, 0);

        _borderRenderer.SetMesh(_borderMesh);
        _borderRenderer.SetMaterial(_borderMaterial, null);
    }

    private void OnDisable()
    {
        _canvasRenderer.Clear();
        if (_borderRenderer != null)
            _borderRenderer.Clear();
    }

    public List<Vector3> GetVertices()
    {
        return new List<Vector3>(_vertices);
    }
}
