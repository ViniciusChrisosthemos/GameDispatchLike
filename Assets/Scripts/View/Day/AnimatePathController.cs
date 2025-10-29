using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatePathController : MonoBehaviour
{
    [SerializeField] private PathFinderManager _pathFinderManager;

    [SerializeField] private UIHeroPathIconController _heroPathPrefab;
    [SerializeField] private Transform _objsParent;

    [SerializeField] private float _moveSpeed = 2f;

    public static IEnumerator AnimatePathCoroutine(Transform obj, List<Node> path, float velocity, Action callback)
    {
        float t = 0f;
        float dist = 0f;
        float tOffset = velocity * Time.deltaTime;

        for (int i = 1; i < path.Count; i++)
        {
            var node1 = path[i-1];
            var node2 = path[i];

            do
            {
                obj.position = Vector3.Lerp(node1.Transform.position, node2.Transform.position, t);

                t += tOffset;
                dist = Vector3.Distance(obj.position, node2.Transform.position);

                if (t > 1f)
                {
                    break;
                }
                else
                {
                    yield return null;
                }

            } while (dist > 0.01f);

            t -= 1f;
        }

        callback?.Invoke();
    }

    public void AnimatePath(Team currentTeam, Transform baseTransform, Transform location, Action callback)
    {
        var path = _pathFinderManager.GetPath(baseTransform.GetComponent<Node>(), location.GetComponent<Node>());

        var instante = Instantiate(_heroPathPrefab, _objsParent);
        instante.Init(currentTeam);

        StartCoroutine(AnimatePathCoroutine(instante.transform, path, _moveSpeed, () =>
        {
            callback?.Invoke();
            Destroy(instante.gameObject);
        }));
    }
}
