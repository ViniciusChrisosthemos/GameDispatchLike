using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatManager;

public class UIRadarChartController : MonoBehaviour
{
    [SerializeField] private CanvasRenderer _canvasRenderer;
    [SerializeField] private Material _radarChartMaterial;
    [SerializeField] private CharacterSO CharacterSO;
    [SerializeField] private float _radarChartSize = 200f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            var values = new List<float>();

            foreach (StatType stat in Enum.GetValues(typeof(StatType)))
            {
                var value = CharacterSO.BaseStats.GetStat(stat).BaseValue / 10f;

                Debug.Log($"{value} {stat}");

                values.Add(value);
            }

            UpdateStats(values);
        }
    }

    public void UpdateStats(List<float> values)
    {
        Mesh mesh = new Mesh();

        int valuesAmount = values.Count;

        Vector3[] vertices = new Vector3[valuesAmount + 1];
        Vector2[] uvs = new Vector2[valuesAmount + 1];
        int[] traingles = new int[3 * valuesAmount] ;

        float angleIncrement = 360f / valuesAmount;

        vertices[0] = Vector3.zero;

        for (int i = 1; i <= valuesAmount; i++)
        {
            float normalizedStat = Mathf.Min(values[i - 1], 1f);

            vertices[i] = Quaternion.Euler(0.0f, 0.0f, -angleIncrement * (i-1)) * Vector3.up * _radarChartSize * normalizedStat;
        }

        int counter = 0;
        for (int i = 1; i <= valuesAmount; i++)
        {
            traingles[counter++] = 0;
            traingles[counter++] = i;
            traingles[counter++] = (i + 1) % (valuesAmount + 1) + (i / valuesAmount);
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = traingles;

        _canvasRenderer.SetMesh(mesh);
        _canvasRenderer.SetMaterial(_radarChartMaterial, null);
    }
}