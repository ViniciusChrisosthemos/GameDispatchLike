using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UICompareStatsController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UIAnimatePolygonBounceController _uiAnimatePolygonBounceController;
    [SerializeField] private UIRadarChartController _expectedStatRadarController;
    [SerializeField] private UIRadarChartController _teamStatRadarController;
    [SerializeField] private Transform _pivot;

    [Header("Parameters")]
    [SerializeField] private float _duration = 2f;
    [SerializeField] private float _speed = 150f;

    public void CreateRadarChartForStats(List<float> expectedValues, List<float> teamValues)
    {
        _expectedStatRadarController.UpdateStats(expectedValues);
        _teamStatRadarController.UpdateStats(teamValues);
    }

    public void CompareStatAnimation(List<float> expectedValues, List<float> teamValues, Action<bool> onResult)
    {
        var polygon = _expectedStatRadarController.GetVertices().Select(v => new Vector2(v.x, v.y)).ToList();
        polygon.RemoveAt(0);
        polygon.Add(polygon[0]);

        _uiAnimatePolygonBounceController.Animate(polygon, _pivot, _duration, _speed, () =>
        {
            var teamPolygon = _teamStatRadarController.GetVertices();
            teamPolygon.RemoveAt(0);
            teamPolygon.Add(teamPolygon[0]);

            var point = _uiAnimatePolygonBounceController.Point.position - _uiAnimatePolygonBounceController.Pivot.position;
            var result = IsPointInPolygon(point, teamPolygon);

            onResult?.Invoke(result);
        });
    }

    public static bool IsPointInPolygon(Vector2 point, List<Vector3> polygon)
    {
        int n = polygon.Count;
        bool inside = false;

        // Percorre todas as arestas do polígono
        for (int i = 0, j = n - 1; i < n; j = i++)
        {
            Vector2 pi = polygon[i];
            Vector2 pj = polygon[j];

            // Verifica se o ponto está entre as duas extremidades da aresta
            if ((pi.y > point.y) != (pj.y > point.y) &&
                point.x < (pj.x - pi.x) * (point.y - pi.y) / (pj.y - pi.y) + pi.x)
            {
                inside = !inside;
            }
        }

        return inside;
    }

    public List<Vector3> ExpectedStatPolygon => _expectedStatRadarController.GetPoints();
    public List<Vector3> TeamStatPolygon => _teamStatRadarController.GetPoints();
}
