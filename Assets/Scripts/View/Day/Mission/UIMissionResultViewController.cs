using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIMissionResultViewController : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private Button _btnOK;
    [SerializeField] private UICompareStatsController _uiCompareStatsController;
    [SerializeField] private UIRadarChartController _intersectionSuccessStatRadarController;
    [SerializeField] private UIRadarChartController _intersectionFailedStatRadarController;

    [Header("UI")]
    [SerializeField] private GameObject _successIconPivot;
    [SerializeField] private GameObject _failIconPivot;

    [Header("Events")]
    public UnityEvent OnMissionSuccessEvent;
    public UnityEvent OnMissionFailEvent;

    private MissionUnit _missionUnit;
    private Action<bool> _callback;
    private bool _result;

    private void Start()
    {
        _view.SetActive(false);
    }

    public void OpenResultScreen(MissionUnit mission, Action<bool> onResult)
    {
        _view.SetActive(true);
        _btnOK.gameObject.SetActive(false);

        _successIconPivot.SetActive(false);
        _failIconPivot.SetActive(false);

        _intersectionFailedStatRadarController.gameObject.SetActive(false);
        _intersectionSuccessStatRadarController.gameObject.SetActive(false);

        _missionUnit = mission;
        _callback = onResult;

        CreateRadarCharts(mission);
    }

    public void CreateRadarCharts(MissionUnit mission)
    {
        var teamStats = mission.Team.GetTeamStats().GetValues();
        var requiredStats = mission.GetRequiredStats().GetValues();

        _uiCompareStatsController.CreateRadarChartForStats(requiredStats, teamStats);
    }

    public Transform _temp;
    public GameObject prefab;

    public void StartAnimateResult()
    {
        var teamStats = _missionUnit.Team.GetTeamStats().GetValues();
        var requiredStats = _missionUnit.GetRequiredStats().GetValues();

        _uiCompareStatsController.CompareStatAnimation(requiredStats, teamStats, (result) =>
        {
            _successIconPivot.SetActive(result);
            _failIconPivot.SetActive(!result);
            _result = result;

            WaitForSeconds(1f, () =>
            {
                var expectedStatPolygon = _uiCompareStatsController.ExpectedStatPolygon;
                var teamStatPolygon = _uiCompareStatsController.TeamStatPolygon;

                var intersectionPolygon = PolygonIntersection.IntersectPolygons(expectedStatPolygon, teamStatPolygon);
                var formattedPolygon = intersectionPolygon.Select(p => new Vector2(p.x, p.y)).ToList();

                if (result)
                {
                    _intersectionSuccessStatRadarController.gameObject.SetActive(true);
                    _intersectionSuccessStatRadarController.UpdateStats(formattedPolygon);

                    OnMissionSuccessEvent?.Invoke();
                }
                else
                {
                    _intersectionFailedStatRadarController.gameObject.SetActive(true);
                    _intersectionFailedStatRadarController.UpdateStats(formattedPolygon);

                    OnMissionFailEvent?.Invoke();
                }
            });

            WaitForSeconds(1.5f, () =>
            {
                _btnOK.onClick.RemoveAllListeners();
                _btnOK.onClick.AddListener(() =>
                {
                    _callback?.Invoke(result);
                    Close();
                });

                _btnOK.gameObject.SetActive(true);
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
