using System;
using System.Collections.Generic;
using UnityEngine;

public class UIRadarChartController : MonoBehaviour
{
    [Header("Referências Principais")]
    [SerializeField] private CanvasRenderer _canvasRenderer;
    [SerializeField] private Material _radarChartMaterial;
    [SerializeField] private Transform _middleReference;
    [SerializeField] private Transform _topReference;

    [Header("Border (optional)")]
    [SerializeField] private bool _drawBorder = true;
    [SerializeField] private CanvasRenderer _borderRenderer;
    [SerializeField] private Material _borderMaterial;
    [SerializeField] private float _borderThickness = 1f;

    [Header("Pontas (círculos)")]
    [SerializeField] private bool _drawCircles = true;
    [SerializeField] private UIRadarCircleController _circlePrefab;
    [SerializeField] private Transform _circleReferencesParent;
    [SerializeField] private Transform _circleInstancesParent;
    [SerializeField] private float _circleRadius = 2f;
    [SerializeField] private Color _circleColor = Color.white;
    [SerializeField] private bool _addNumber = false;
    [SerializeField] private bool _trackRadar = false;

    private Mesh _mainMesh;
    private Mesh _borderMesh;
    private CanvasRenderer _circlesRenderer;
    private Vector3[] _vertices;
    private Vector3[] _points;

    private void Awake()
    {
        // Cria dinamicamente renderers auxiliares
        if (_borderRenderer == null)
        {
            GameObject borderObj = new GameObject("RadarChartBorder");
            borderObj.transform.SetParent(transform, false);
            _borderRenderer = borderObj.AddComponent<CanvasRenderer>();
        }

        GameObject circlesObj = new GameObject("RadarChartCircles");
        circlesObj.transform.SetParent(transform, false);
        _circlesRenderer = circlesObj.AddComponent<CanvasRenderer>();
    }

    public void UpdateStats(List<float> values)
    {
        var mask = new List<bool>();

        values.ForEach(v => mask.Add(true));

        UpdateStats(values, mask);
    }

    public void UpdateStats(List<float> values, List<bool> mask)
    {
        if (values == null || values.Count < 3)
        {
            Debug.LogWarning("RadarChart precisa de pelo menos 3 valores para formar um polígono.");
            return;
        }

        int valuesAmount = values.Count;
        float angleIncrement = 360f / valuesAmount;
        float radarChartSize = Mathf.Abs(_middleReference.position.y - _topReference.position.y);

        // --- MESH PRINCIPAL ---
        _mainMesh = new Mesh();

        _points = new Vector3[valuesAmount];
        _vertices = new Vector3[valuesAmount + 1];
        Vector2[] uvs = new Vector2[valuesAmount + 1];
        int[] triangles = new int[3 * valuesAmount];

        _vertices[0] = Vector3.zero;

        for (int i = 1; i <= valuesAmount; i++)
        {
            float normalizedStat = Mathf.Clamp01(values[i - 1]);
            // Usa coordenadas LOCAIS, relativo ao centro
            Vector3 localPos = Quaternion.Euler(0, 0, -angleIncrement * (i - 1)) * Vector3.up * radarChartSize * normalizedStat;
            _vertices[i] = localPos;
            _points[i - 1] = localPos;
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

        _canvasRenderer.SetMesh(_mainMesh);
        _canvasRenderer.SetMaterial(_radarChartMaterial, null);

        // --- BORDA ---
        if (_drawBorder && _borderMaterial != null)
            DrawBorder(_vertices);
        else
            _borderRenderer.Clear();


        // --- CÍRCULOS NAS PONTAS ---
        if (_drawCircles)
        {
            CreateCircles(_vertices, values, mask);
            //DrawCircles(_vertices);
        }
        else
            _circlesRenderer.Clear();
    }

    public void UpdateStats(List<Vector2> vertices2D)
    {
        if (vertices2D == null || vertices2D.Count < 3)
        {
            Debug.LogWarning("RadarChart precisa de pelo menos 3 vértices para formar um polígono.");
            return;
        }

        int vertexCount = vertices2D.Count;

        // --- MESH PRINCIPAL ---
        _mainMesh = new Mesh();

        // O centro (pivot) é o vértice 0
        _vertices = new Vector3[vertexCount + 1];
        Vector2[] uvs = new Vector2[vertexCount + 1];
        int[] triangles = new int[3 * vertexCount];

        // Centro
        _vertices[0] = Vector3.zero;

        // Converte Vector2 → Vector3 (coordenadas locais)
        for (int i = 0; i < vertexCount; i++)
        {
            _vertices[i + 1] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0f);
        }

        // Cria triângulos ligando cada ponto ao centro
        int counter = 0;
        for (int i = 1; i <= vertexCount; i++)
        {
            triangles[counter++] = 0;
            triangles[counter++] = i;
            triangles[counter++] = (i % vertexCount) + 1;
        }

        _mainMesh.vertices = _vertices;
        _mainMesh.uv = uvs;
        _mainMesh.triangles = triangles;

        _canvasRenderer.SetMesh(_mainMesh);
        _canvasRenderer.SetMaterial(_radarChartMaterial, null);

        // --- BORDA ---
        if (_drawBorder && _borderMaterial != null)
            DrawBorder(_vertices);
        else
            _borderRenderer.Clear();

        // --- CÍRCULOS NAS PONTAS ---
        if (_drawCircles)
        {
            CreateCircles(_vertices, new List<float>(), new List<bool>(_vertices.Length));
        }
        else
        {
            _circlesRenderer.Clear();
        }
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

            borderVertices.Add(current - normal);
            borderVertices.Add(current + normal);
            borderVertices.Add(next - normal);
            borderVertices.Add(next + normal);

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

    private void CreateCircles(Vector3[] vertices, List<float> values, List<bool> mask)
    {
        _circleInstancesParent.ClearChilds();
        _circleReferencesParent.ClearChilds();

        Vector3 centerOffset = _middleReference != null ? _middleReference.localPosition : Vector3.zero;

        for(int i=1; i < vertices.Length; i++)
        {
            if (!mask[i-1]) continue;

            var vertex = vertices[i];
            Vector3 center = vertex + centerOffset;

            if (_trackRadar)
            {
                var reference = new GameObject($"Circle Reference - {i}");
                reference.transform.SetParent(_circleReferencesParent);
                reference.transform.localPosition = center;

                var controller = Instantiate(_circlePrefab, _circleInstancesParent);
                controller.SetReference(reference.transform);
                controller.UpdateCircle(_circleRadius, _circleColor);

                if (_addNumber && values.Count != 0)
                {
                    controller.SetNumber((int)(values[i - 1] * 10));
                }
            }
            else
            {
                var controller = Instantiate(_circlePrefab, _circleInstancesParent);
                controller.transform.localPosition = center;
                controller.UpdateCircle(_circleRadius, _circleColor);

                if (_addNumber && values.Count != 0)
                {
                    controller.SetNumber((int)(values[i - 1] * 10));
                }
            }

        }
    }

    /*
    private void DrawCircles(Vector3[] vertices)
    {
        if (_circlesMesh == null)
            _circlesMesh = new Mesh();

        List<Vector3> circleVerts = new List<Vector3>();
        List<int> circleTris = new List<int>();
        List<Color> circleColors = new List<Color>();

        // Corrigir o centro — usa a posição do _middleReference como base
        Vector3 centerOffset = _middleReference != null ? _middleReference.localPosition : Vector3.zero;

        for (int i = 1; i < vertices.Length; i++)
        {
            // Garante que as posições estejam relativas ao centro correto
            Vector3 center = vertices[i] + centerOffset;

            int startIndex = circleVerts.Count;
            circleVerts.Add(center);
            circleColors.Add(_circleColor);

            float angleStep = 360f / _circleSegments;

            for (int j = 0; j <= _circleSegments; j++)
            {
                float angle = Mathf.Deg2Rad * (j * angleStep);
                Vector3 point = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * _circleRadius;
                circleVerts.Add(point);
                circleColors.Add(_circleColor);
            }

            for (int j = 0; j < _circleSegments; j++)
            {
                circleTris.Add(startIndex);
                circleTris.Add(startIndex + j + 1);
                circleTris.Add(startIndex + j + 2);
            }
        }

        _circlesMesh.Clear();
        _circlesMesh.SetVertices(circleVerts);
        _circlesMesh.SetTriangles(circleTris, 0);
        _circlesMesh.SetColors(circleColors);

        // Mantém material transparente
        _materialCircles.color = _circleColor;

        _circlesRenderer.SetMesh(_circlesMesh);
        _circlesRenderer.SetMaterial(_materialCircles, null);
    }
    */

    private void OnDisable()
    {
        _canvasRenderer.Clear();
        if (_borderRenderer != null)
            _borderRenderer.Clear();
        if (_circlesRenderer != null)
            _circlesRenderer.Clear();
    }

    public List<Vector3> GetVertices()
    {
        return new List<Vector3>(_vertices);
    }

    public List<Vector3> GetPoints() => new List<Vector3>(_points);
}
