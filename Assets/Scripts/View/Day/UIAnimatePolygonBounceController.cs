using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIAnimatePolygonBounceController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _target;

    [Header("Parameters")]
    [SerializeField] private AnimationCurve speedCurve;

    private Vector2 direction;
    private bool isMoving = false;
    private Transform _pivot;

    private List<Vector2> _polygon;
    
    private void OnDrawGizmos()
    {
        if (_polygon == null || _polygon.Count < 2)
            return;

        Gizmos.color = Color.yellow;
        for (int i = 0; i < _polygon.Count; i++)
        {
            Vector2 a = _polygon[i];
            Vector2 b = _polygon[(i + 1) % _polygon.Count];
            Gizmos.DrawLine(a, b);
        }

        // Mostra direção atual
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, direction * 0.5f);
        }
    }
    
    /// <summary>
    /// Inicia o movimento com duração e desaceleração controlada.
    /// </summary>
    public void Animate(List<Vector2> polygon, Transform pivot, float duration, float speed, Action callback)
    {
        if (isMoving)
            StopAllCoroutines();

        StartCoroutine(MoveInsidePolygon(polygon, pivot, duration, speed, callback));
    }

    private IEnumerator MoveInsidePolygon(List<Vector2> polygon, Transform pivot, float duration, float speed, Action callback)
    {
        isMoving = true;
        float elapsed = 0f;

        _pivot = pivot;
        _polygon = new List<Vector2>();

        polygon.ForEach(p => _polygon.Add(p + (Vector2)pivot.position));

        var center = Vector2.zero;
        _polygon.ForEach(p => center += p);
        center *= 1 / (float)_polygon.Count;

        _target.localPosition = center;

        direction = Random.insideUnitCircle.normalized;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float speedFactor = speedCurve.Evaluate(t); // curva define desaceleração
            float currentSpeed = speed * speedFactor;

            Vector2 pos = _target.localPosition;
            Vector2 newPos = pos + direction * currentSpeed * Time.deltaTime;

            // Verifica colisões com as bordas
            for (int i = 0; i < _polygon.Count; i++)
            {
                Vector2 a = _polygon[i];
                Vector2 b = _polygon[(i + 1) % _polygon.Count];

                if (SegmentIntersection(pos, newPos, a, b, out Vector2 intersection))
                {
                    // Calcula a normal da parede
                    Vector2 edge = b - a;
                    Vector2 normal = new Vector2(-edge.y, edge.x).normalized;

                    // Reflete a direção
                    direction = Vector2.Reflect(direction, normal);

                    // Move levemente para dentro do polígono após o ricochete
                    newPos = intersection + direction * 0.01f;
                    break;
                }
            }

            _target.localPosition = newPos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        isMoving = false;

        callback?.Invoke();
    }

    /// <summary>
    /// Testa interseção entre dois segmentos (p1,p2) e (p3,p4)
    /// </summary>
    private bool SegmentIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersection)
    {
        intersection = Vector2.zero;

        Vector2 r = p2 - p1;
        Vector2 s = p4 - p3;
        float rxs = r.x * s.y - r.y * s.x;
        float qpxr = (p3 - p1).x * r.y - (p3 - p1).y * r.x;

        if (Mathf.Abs(rxs) < 0.0001f) return false; // Paralelos

        float t = ((p3 - p1).x * s.y - (p3 - p1).y * s.x) / rxs;
        float u = ((p3 - p1).x * r.y - (p3 - p1).y * r.x) / rxs;

        if (t >= 0f && t <= 1f && u >= 0f && u <= 1f)
        {
            intersection = p1 + t * r;
            return true;
        }

        return false;
    }

    public Transform Point => _target;
    public Transform Pivot => _pivot;
}
