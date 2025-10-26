using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIMissionController : MonoBehaviour
{
    [Header("Available View")]
    [SerializeField] private GameObject _availableView;
    [SerializeField] private Image _imgSliderTime;
    [SerializeField] private TextMeshProUGUI _txtMissionName;
    [SerializeField] private TextMeshProUGUI _txtExp;
    [SerializeField] private TextMeshProUGUI _txtGold;

    [Header("In Progress View")]
    [SerializeField] private GameObject _inProgressView;

    [Header("Completed")]
    [SerializeField] private GameObject _completedView;

    [SerializeField] private Button _btnMission;

    private MissionUnit _missionUnit;

    private void Start()
    {
        _availableView.SetActive(true);
        _inProgressView.SetActive(false);
        _completedView.SetActive(false);
    }

    public void Init(MissionUnit mission, Action<MissionUnit> callback, Action<UIMissionController> handleCallForDeleteMission)
    {
        _missionUnit = mission;

        _txtMissionName.text = mission.Name;
        _txtExp.text = mission.Exp.ToString();
        _txtGold.text = mission.Gold.ToString();

        _imgSliderTime.fillAmount = 1;

        _btnMission.onClick.AddListener(() => callback?.Invoke(MissionUnit));

        mission.OnMissionLose.AddListener(m => handleCallForDeleteMission?.Invoke(this));
        mission.OnMissionAccepted.AddListener(m => SetMissionInProgress());
        mission.OnMissionCompleted.AddListener(m => SetMissionCompleted());
    }


    public void UpdateTime(float elapsedTime)
    {
        if (_missionUnit.IsMissionCompleted()) return;

        var normalizedTime = _missionUnit.IsMissionInProgress() ? _missionUnit.GetTotalTimeFromAcceptMission(elapsedTime) : _missionUnit.GetTotalTimeFromGetMission(elapsedTime);

        _imgSliderTime.fillAmount = 1 - normalizedTime;
    }

    public void SetMissionInProgress()
    {
        _availableView.SetActive(false);

        _inProgressView.SetActive(true);
    }

    public void SetMissionCompleted()
    {
        _inProgressView.SetActive(false);

        _completedView.SetActive(true);
    }

    public MissionUnit MissionUnit => _missionUnit;
}
