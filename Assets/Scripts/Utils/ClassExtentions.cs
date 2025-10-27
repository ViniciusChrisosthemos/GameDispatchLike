using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClassExtentions
{
    private const string DELETED_OBJECT_NAME = "DELETED";

    public static Vector2 SetX(this Vector2 vector, float x)
    {
        return new Vector2(x, vector.y);
    }
    public static Vector2 SetY(this Vector2 vector, float y)
    {
        return new Vector2(vector.x, y);
    }

    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : MonoBehaviour
    {
        var component = gameObject.GetComponent<T>();
        if (component == null) component = gameObject.AddComponent<T>();
        return component;
    }

    public static bool HasComponent<T>(this GameObject gameObject) where T : MonoBehaviour
    {
        return gameObject.GetComponent<T>() != null;
    }

    public static T GetRandomItem<T>(this IList<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static void ClearChilds(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            child.name = DELETED_OBJECT_NAME;
            GameObject.Destroy(child.gameObject);
        }
    }

    public static List<GameObject> GetChilds(this Transform transform)
    {
        var childs = new List<GameObject>();

        foreach (Transform child in transform)
        {
            if (child.name == DELETED_OBJECT_NAME) continue;

            childs.Add(child.gameObject);
        }

        return childs;
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}