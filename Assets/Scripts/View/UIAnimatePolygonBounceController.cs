using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIAnimatePolygonBounceController : MonoBehaviour
{
    [Header("Definição do Polígono (em coordenadas locais)")]
    public List<Transform> tranforms = new List<Transform>();

    [Header("Movimento")]
    private Vector2 direction;
    public AnimationCurve speedCurve;

    private bool isMoving = false;

    private void Start()
    {
        // Define uma direção inicial aleatória normalizada
        direction = Random.insideUnitCircle.normalized;
    }

    /*
    private void OnDrawGizmos()
    {
        if (polygonVertices == null || polygonVertices.Count < 2)
            return;

        Gizmos.color = Color.yellow;
        for (int i = 0; i < polygonVertices.Count; i++)
        {
            Vector2 a = polygonVertices[i];
            Vector2 b = polygonVertices[(i + 1) % polygonVertices.Count];
            Gizmos.DrawLine(a, b);
        }

        // Mostra direção atual
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, direction * 0.5f);
        }
    }
    */
    /// <summary>
    /// Inicia o movimento com duração e desaceleração controlada.
    /// </summary>
    public void Animate(List<Vector2> polygon, float duration, float speed)
    {
        if (isMoving)
            StopAllCoroutines();

        StartCoroutine(MoveInsidePolygon(polygon, duration, speed));
    }

    private IEnumerator MoveInsidePolygon(List<Vector2> polygon, float duration, float speed)
    {
        isMoving = true;
        float elapsed = 0f;

        Vector2 center = Vector3.zero;
        polygon.ForEach(p => center += p);
        center *= (1 / (float)polygon.Count);

        transform.position = center;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float speedFactor = speedCurve.Evaluate(t); // curva define desaceleração
            float currentSpeed = speed * speedFactor;

            Vector2 pos = transform.position;
            Vector2 newPos = pos + direction * currentSpeed * Time.deltaTime;

            // Verifica colisões com as bordas
            for (int i = 0; i < polygon.Count; i++)
            {
                Vector2 a = polygon[i];
                Vector2 b = polygon[(i + 1) % polygon.Count];

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

            transform.position = newPos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        isMoving = false;
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
}
