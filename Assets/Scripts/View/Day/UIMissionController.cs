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
    [SerializeField] private Color _colorMissionAvailable = Color.red;
    [SerializeField] private Color _colorMissionInProgress = Color.yellow;

    [Header("(Optional)")]
    [SerializeField] private TextMeshProUGUI _txtMissionName;
    [SerializeField] private TextMeshProUGUI _txtExp;
    [SerializeField] private TextMeshProUGUI _txtGold;

    [Header("In Progress View")]
    [SerializeField] private GameObject _inProgressView;
    [SerializeField] private Image _imgInProgress;

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

        if (_txtMissionName != null) _txtMissionName.text = mission.Name;
        if (_txtExp != null) _txtExp.text = mission.Exp.ToString();
        if (_txtGold != null) _txtGold.text = mission.Gold.ToString();

        _imgSliderTime.fillAmount = 1;

        _btnMission.onClick.AddListener(() => callback?.Invoke(MissionUnit));

        mission.OnMissionLose.AddListener(m => handleCallForDeleteMission?.Invoke(this));
        mission.OnMissionAccepted.AddListener(m => SetMissionAccepted());
        mission.OnMissionStarted.AddListener(m => SetMissionInProgress());
        mission.OnMissionCompleted.AddListener(m => SetMissionCompleted());

        _imgSliderTime.color = _colorMissionAvailable;
    }


    public void UpdateTime(float elapsedTime)
    {
        if (_missionUnit.IsMissionCompleted() || _missionUnit.IsAccepted()) return;

        var normalizedTime = _missionUnit.IsMissionInProgress() ? _missionUnit.GetTotalTimeFromAcceptMission(elapsedTime) : _missionUnit.GetTotalTimeFromGetMission(elapsedTime);

        _imgSliderTime.fillAmount = 1 - normalizedTime;
    }

    private void SetMissionAccepted()
    {
        _availableView.SetActive(false);

        _inProgressView.SetActive(true);

        _imgSliderTime.fillAmount = 1;
        _imgSliderTime.color = _colorMissionInProgress;
        _imgInProgress.sprite = _missionUnit.Team.Members[0].FaceArt;

        var color = _imgInProgress.color;
        color.a = 0.5f;

        _imgInProgress.color = color;

        Debug.Log("Accepted");
    }

    public void SetMissionInProgress()
    {
        var color = _imgInProgress.color;
        color.a = 1f;

        _imgInProgress.color = color;
    }

    public void SetMissionCompleted()
    {
        _inProgressView.SetActive(false);

        _completedView.SetActive(true);
    }

    public MissionUnit MissionUnit => _missionUnit;
}
