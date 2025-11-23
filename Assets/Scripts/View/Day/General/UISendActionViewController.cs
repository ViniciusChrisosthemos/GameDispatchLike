using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISendActionViewController : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animatorShowTrigger = "Show";
    [SerializeField] private string _stateName = "Animation_SendActionView_SendAction";

    private void Start()
    {
        SetActive(false);
    }

    public void SetActive(bool isActive)
    {
        _view.SetActive(isActive);
    }

    public void StartSendAction(Action callback)
    {
        StartCoroutine(AnimateSendActionCoroutine(callback));
    }

    private IEnumerator AnimateSendActionCoroutine(Action callback)
    {
        SetActive(true);

        _animator.SetTrigger(_animatorShowTrigger);

        Debug.Log("Start Send Action Animation");
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName(_stateName));

        Debug.Log("Wait Send Action Animation end");
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        Debug.Log("Animation Finished");
        callback?.Invoke();

        SetActive(false);
    }
}
