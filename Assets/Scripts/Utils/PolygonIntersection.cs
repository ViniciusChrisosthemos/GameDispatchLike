using System.Collections.Generic;
using UnityEngine;

public static class PolygonIntersection
{
    public static List<Vector3> IntersectPolygons(List<Vector3> poly1, List<Vector3> poly2)
    {
        if (poly1 == null || poly2 == null || poly1.Count < 3 || poly2.Count < 3)
        {
            Debug.LogError("Polígonos inválidos!");
            return new List<Vector3>();
        }

        List<Vector3> intersection = new List<Vector3>();

        int count = Mathf.Min(poly1.Count, poly2.Count);

        for (int i = 0; i < count; i++)
        {
            Vector3 p1a = poly1[i];
            Vector3 p1b = poly1[(i + 1) % count];
            Vector3 p2a = poly2[i];
            Vector3 p2b = poly2[(i + 1) % count];

            Vector3 inter;
            if (LineIntersection(p1a, p1b, p2a, p2b, out inter))
            {
                // Se há interseção, adiciona o ponto de interseção
                intersection.Add(inter);
            }
            else
            {
                // Caso contrário, adiciona o ponto do primeiro polígono
                
                if (Vector3.Distance(p1a, Vector3.zero) < Vector3.Distance(p2a, Vector3.zero))
                {
                    intersection.Add(p1a);
                }
                else
                {
                    intersection.Add(p2a);
                }

                if (Vector3.Distance(p1b, Vector3.zero) < Vector3.Distance(p2b, Vector3.zero))
                {
                    intersection.Add(p1b);
                }
                else
                {
                    intersection.Add(p2b);
                }
            }
        }

        return intersection;
    }

    /// <summary>
    /// Verifica se há interseção entre os segmentos (p1,p2) e (p3,p4).
    /// Retorna true e o ponto de interseção caso exista.
    /// </summary>
    private static bool LineIntersection(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, out Vector3 intersection)
    {
        intersection = Vector3.zero;

        float denom = (p1.x - p2.x) * (p3.y - p4.y) - (p1.y - p2.y) * (p3.x - p4.x);
        if (Mathf.Abs(denom) < 1e-6f)
            return false; // linhas paralelas ou coincidentes

        float x = ((p1.x * p2.y - p1.y * p2.x) * (p3.x - p4.x) -
                   (p1.x - p2.x) * (p3.x * p4.y - p3.y * p4.x)) / denom;

        float y = ((p1.x * p2.y - p1.y * p2.x) * (p3.y - p4.y) -
                   (p1.y - p2.y) * (p3.x * p4.y - p3.y * p4.x)) / denom;

        Vector3 pt = new Vector3(x, y, 0);

        // Verifica se o ponto está dentro dos dois segmentos
        if (IsBetween(p1, p2, pt) && IsBetween(p3, p4, pt))
        {
            intersection = pt;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Verifica se o ponto 'p' está entre 'a' e 'b' (no segmento).
    /// </summary>
    private static bool IsBetween(Vector3 a, Vector3 b, Vector3 p)
    {
        return (p.x >= Mathf.Min(a.x, b.x) - 1e-5f && p.x <= Mathf.Max(a.x, b.x) + 1e-5f &&
                p.y >= Mathf.Min(a.y, b.y) - 1e-5f && p.y <= Mathf.Max(a.y, b.y) + 1e-5f);
    }
}
