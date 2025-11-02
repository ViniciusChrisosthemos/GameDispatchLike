using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionResultViewController : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private Button _btnOK;
    [SerializeField] private Animator _animator;
    [SerializeField] private UICompareStatsController _uiCompareStatsController;
    [SerializeField] private string _createRadarChartTrigger = "CreateRadarCharts";


    private void Start()
    {
        _view.SetActive(false);
    }

    public void OpenResultScreen(MissionUnit mission, Action<bool> onResult)
    {
        _view.SetActive(true);

        StartCoroutine(AnimateResultCoroutine(mission, onResult));
    }

    private IEnumerator AnimateResultCoroutine(MissionUnit mission, Action<bool> onResult)
    {
        var teamStats = mission.Team.GetTeamStats().GetValues();
        var requiredStats = mission.GetRequiredStats().GetValues();

        _uiCompareStatsController.CreateRadarChartForStats(requiredStats, teamStats);

        _animator.SetTrigger(_createRadarChartTrigger);

        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName(_createRadarChartTrigger));

        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        _uiCompareStatsController.CompareStatAnimation(requiredStats, teamStats, (result) =>
        {
            WaitForSeconds(3, () =>
            {
                onResult?.Invoke(result);
                Close();
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

    public void Close()
    {
        _view.SetActive(false);
    }
}
