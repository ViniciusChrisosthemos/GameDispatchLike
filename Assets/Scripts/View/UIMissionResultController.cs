using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionResultController : MonoBehaviour
{
    [SerializeField] private UICompareStatsController _uiCompareStatsController;

    [SerializeField] private GameObject _view;
    [SerializeField] private Button _btnCloseScreen;
    [SerializeField] private GameObject _successText;
    [SerializeField] private GameObject _failText;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _createRadarChartTrigger = "CreateRadarCharts";


    private void Start()
    {
        _btnCloseScreen.onClick.AddListener(HandleCloseScreen);

        _view.SetActive(false);
    }

    public void OpenResultScreen(MissionUnit mission, Action<bool> onResult)
    {
        _view.SetActive(true);

        StartCoroutine(AnimateResultCoroutine(mission, onResult));
    }

    private IEnumerator AnimateResultCoroutine(MissionUnit mission, Action<bool> onResult)
    {
        _successText.SetActive(false);
        _failText.SetActive(false);

        var teamStats = mission.Team.GetTeamStats().GetValues();
        var requiredStats = mission.GetRequiredStats().GetValues();

        _uiCompareStatsController.CreateRadarChartForStats(teamStats, requiredStats);

        _animator.SetTrigger(_createRadarChartTrigger);

        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName(_createRadarChartTrigger));

        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        _uiCompareStatsController.CompareStatAnimation(requiredStats, teamStats, (result) =>
        {
            _successText.SetActive(result);
            _failText.SetActive(!result);

            WaitForSeconds(3, () =>
            {
                onResult?.Invoke(result);
                HandleCloseScreen();
            });
        });
    }

    private void WaitForSeconds(float seconds, Action callback)
    {
        StartCoroutine(WaitForSecondsCoroutine(seconds, callback));
    }

    private IEnumerator WaitForSecondsCoroutine(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);

        callback?.Invoke();
    }

    private void HandleCloseScreen()
    {
        _view.SetActive(false);
    }
}
