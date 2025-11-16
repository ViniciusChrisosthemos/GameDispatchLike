using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICalendarView : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private Image _imgCurrentTip;
    [SerializeField] private Image _imgNextTip;
    [SerializeField] private List<TextMeshProUGUI> _txtDays;

    [Header("Parameters")]
    [SerializeField] private Color _colorNormalDay;
    [SerializeField] private Color _colorWarningDay;
    [SerializeField] private Color _colorDangerDay;

    [Header("Animation")]
    [SerializeField] private Animator _mainAnimator;
    [SerializeField] private string _calendarAnimationTriggerName;

    private Action _callback;

    private void Start()
    {
        _view.SetActive(false);
    }

    public void SetNormalDay(int currentDay, Action callback) => UpdateCalendar(currentDay, _colorNormalDay, callback);
    public void SetWarningDay(int currentDay, Action callback) => UpdateCalendar(currentDay, _colorWarningDay, callback);
    public void SetDangerDay(int currentDay, Action callback) => UpdateCalendar(currentDay, _colorDangerDay, callback);

    private void UpdateCalendar(int currentDay, Color dayTipColor, Action callback)
    {
        _imgCurrentTip.color = _imgNextTip.color;
        _imgNextTip.color = dayTipColor;

        int day;
        for (int i = -2; i < _txtDays.Count-2; i++)
        {
            day = currentDay + i;
            _txtDays[i + 2].text = day >= 0 ? day.ToString() : "";
        }

        _callback = callback;

        StartCoroutine(PlayCalendarAnimationCoroutine());
    }

    private IEnumerator PlayCalendarAnimationCoroutine()
    {
        _view.SetActive(true);

        _mainAnimator.SetTrigger(_calendarAnimationTriggerName);

        var animatorStatInfo = _mainAnimator.GetCurrentAnimatorStateInfo(0);

        yield return new WaitUntil(() => animatorStatInfo.IsName(_calendarAnimationTriggerName));
        
        animatorStatInfo = _mainAnimator.GetCurrentAnimatorStateInfo(0);

        yield return new WaitUntil(() => animatorStatInfo.normalizedTime >= 1f);

        _callback?.Invoke();

        _view.SetActive(false);
    }
}
