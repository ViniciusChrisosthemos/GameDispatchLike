using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISendActionViewController : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private Image _slider;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _endPosition;
    [SerializeField] private Transform _heroIcon;
    [SerializeField] private GameObject _labelSendingView;
    [SerializeField] private GameObject _labelCompleteView;
    [SerializeField] private float _timeToSend = 2f;
    [SerializeField] private float _timeToWait = 1f;
    [SerializeField] private float _moveAmount = 5f;

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
        StartCoroutine(AnimateSendActionCoroutine(_timeToSend, callback));
    }

    private IEnumerator AnimateSendActionCoroutine(float time, Action callback)
    {
        SetActive(true);
        _heroIcon.position = _startPosition.position;

        _labelSendingView.SetActive(true);
        _labelCompleteView.SetActive(false);

        var accumTime = 0f;
        var accumTimeToMove = 0f;
        var timeToMove = time / _moveAmount;
        var posOffeset = (_endPosition.position - _startPosition.position) * (1 / _moveAmount);

        while (accumTime < time)
        {
            accumTime += Time.deltaTime;
            _slider.fillAmount = 1 - (accumTime / time);

            accumTimeToMove += Time.deltaTime;

            if (accumTimeToMove > timeToMove)
            {
                _heroIcon.position += posOffeset;

                accumTimeToMove = 0f;
            }

            yield return null;
        }

        _heroIcon.position = _endPosition.position;

        _labelSendingView.SetActive(false);
        _labelCompleteView.SetActive(true);

        yield return new WaitForSeconds(_timeToWait);


        callback?.Invoke();

        SetActive(false);
    }
}
