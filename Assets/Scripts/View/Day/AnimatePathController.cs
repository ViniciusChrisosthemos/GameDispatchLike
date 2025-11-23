using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AnimatePathController : MonoBehaviour
{
    [SerializeField] private PathFinderManager _pathFinderManager;

    [SerializeField] private UIHeroPathIconController _heroPathPrefab;
    [SerializeField] private Transform _objsParent;

    [Header("Events")]
    public UnityEvent OnStartAnimate;

    private static bool _isPaused = false;

    public static IEnumerator AnimatePathCoroutine(Transform obj, List<Node> path, float velocity, Action callback)
    {
        if (path == null || path.Count == 0)
        {
            callback?.Invoke();
            yield break;
        }

        // velocidade em unidades por segundo
        for (int i = 1; i < path.Count; i++)
        {
            var node1 = path[i - 1];
            var node2 = path[i];

            Vector3 endPos = node2.Transform.position;

            // se já estiver praticamente no destino, pular
            if (Vector3.Distance(obj.position, endPos) <= 0.01f)
            {
                obj.position = endPos;
                continue;
            }

            // mover-se em velocidade constante até alcançar o nó destino
            while (Vector3.Distance(obj.position, endPos) > 0.01f)
            {
                if (_isPaused)
                {
                    yield return new WaitWhile(() => _isPaused);
                }

                float step = velocity * Time.deltaTime; // unidades por frame
                obj.position = Vector3.MoveTowards(obj.position, endPos, step);

                yield return null;
            }

            obj.position = endPos;
        }

        callback?.Invoke();
    }

    public void AnimatePath(Team currentTeam, float moveSpeed, Transform baseTransform, Transform location, Action callback)
    {
        var path = _pathFinderManager.GetPath(baseTransform.GetComponent<Node>(), location.GetComponent<Node>());

        var instante = Instantiate(_heroPathPrefab, path[0].transform.position, Quaternion.identity, _objsParent);
        instante.Init(currentTeam);

        StartCoroutine(AnimatePathCoroutine(instante.transform, path, moveSpeed, () =>
        {
            callback?.Invoke();
            Destroy(instante.gameObject);
        }));

        OnStartAnimate?.Invoke();
    }

    public static void SetPause(bool isPaused)
    {
        _isPaused = isPaused;
    }
}
