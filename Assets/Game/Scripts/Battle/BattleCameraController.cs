using System;
using System.Collections;
using UnityEngine;

public class BattleCameraController : AbstractSubComponent<BattleManager>
{
    [Header("UI References")]
    [SerializeField] private Transform _camera;

    [Header("Animation Parameters")]
    [SerializeField] private float _cameraMoveDuration = 1f;

    private void Start()
    {
        _camera.gameObject.SetActive(false);
    }

    private IEnumerator AnimateCameraMovementCoroutine(Transform target, float duration)
    {
        var accumTime = 0f;

        var startPosition = _camera.position;
        var startRotation = _camera.rotation;

        while (accumTime < duration)
        {
            accumTime += Time.deltaTime;

            var t = accumTime / duration;

            _camera.position = Vector3.Lerp(startPosition, target.position, t);
            _camera.rotation = Quaternion.Slerp(startRotation, target.rotation, t);

            yield return null;
        }
    }

    public void MoveCameraTo(Transform target)
    {
        StartCoroutine(AnimateCameraMovementCoroutine(target, _cameraMoveDuration));
    }

    protected override void BindHandles(BattleManager mainComponent)
    {
        mainComponent.OnBattleEnd.AddListener(() =>
        {
            _camera.gameObject.SetActive(false);
        });
    }

    protected override void HandleInternalSetup(BattleManager mainComponent)
    {
        _camera.gameObject.SetActive(true);
    }
}
