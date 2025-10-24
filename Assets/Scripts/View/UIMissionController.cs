using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionController : MonoBehaviour
{
    [SerializeField] private Image _imgSliderTime;
    [SerializeField] private TextMeshProUGUI _txtMissionName;
    [SerializeField] private TextMeshProUGUI _txtExp;
    [SerializeField] private TextMeshProUGUI _txtGold;

    [SerializeField] private Button _btnMission;

    private MissionUnit _missionUnit;

    public void Init(MissionUnit mission, Action<MissionUnit> callback)
    {
        _missionUnit = mission;

        _txtMissionName.text = mission.Name;
        _txtExp.text = mission.Exp.ToString();
        _txtGold.text = mission.Gold.ToString();

        _imgSliderTime.fillAmount = 1;

        _btnMission.onClick.AddListener(() => callback?.Invoke(MissionUnit));
    }

    public void UpdateTime(float elapsedTime)
    {
        var normalizedTime = _missionUnit.IsMissionInProgress() ? _missionUnit.GetTotalTimeFromAcceptMission(elapsedTime) : _missionUnit.GetTotalTimeFromGetMission(elapsedTime);

        _imgSliderTime.fillAmount = 1 - normalizedTime;
    }

    public MissionUnit MissionUnit => _missionUnit;
}
