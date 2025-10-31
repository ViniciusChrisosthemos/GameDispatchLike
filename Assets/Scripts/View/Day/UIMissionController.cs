using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMissionController : MonoBehaviour, IPointerClickHandler
{
    [Header("Available View")]
    [SerializeField] private GameObject _availableView;
    [SerializeField] private SpriteFill _spriteSliderTime;
    [SerializeField] private Color _colorMissionAvailable = Color.red;
    [SerializeField] private Color _colorMissionInProgress = Color.yellow;

    [Header("(Optional)")]
    [SerializeField] private TextMeshProUGUI _txtMissionName;
    [SerializeField] private TextMeshProUGUI _txtExp;
    [SerializeField] private TextMeshProUGUI _txtGold;

    [Header("In Progress View")]
    [SerializeField] private GameObject _inProgressView;
    [SerializeField] private SpriteRenderer _spriteInProgress;

    [Header("Completed")]
    [SerializeField] private GameObject _completedView;

    private MissionUnit _missionUnit;

    private UnityEvent OnClickCallback = new UnityEvent();

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

        _spriteSliderTime.fillAmount = 1;

        OnClickCallback.AddListener(() => callback?.Invoke(MissionUnit));

        mission.OnMissionLose.AddListener(m => handleCallForDeleteMission?.Invoke(this));
        mission.OnMissionAccepted.AddListener(m => SetMissionAccepted());
        mission.OnMissionStarted.AddListener(m => SetMissionInProgress());
        mission.OnMissionCompleted.AddListener(m => SetMissionCompleted());

        _spriteSliderTime.Color = _colorMissionAvailable;
    }


    public void UpdateTime(float elapsedTime)
    {
        if (_missionUnit.IsMissionCompleted() || _missionUnit.IsAccepted()) return;

        var normalizedTime = _missionUnit.IsMissionInProgress() ? _missionUnit.GetTotalTimeFromAcceptMission(elapsedTime) : _missionUnit.GetTotalTimeFromGetMission(elapsedTime);

        _spriteSliderTime.fillAmount = 1 - normalizedTime;
    }

    private void SetMissionAccepted()
    {
        _availableView.SetActive(false);

        _inProgressView.SetActive(true);

        _spriteSliderTime.fillAmount = 1;
        _spriteSliderTime.Color = _colorMissionInProgress;
        _spriteInProgress.sprite = _missionUnit.Team.Members[0].FaceArt;

        var color = _spriteInProgress.color;
        color.a = 0.5f;

        _spriteInProgress.color = color;

        Debug.Log("Accepted");
    }

    public void SetMissionInProgress()
    {
        var color = _spriteInProgress.color;
        color.a = 1f;

        _spriteInProgress.color = color;
    }

    public void SetMissionCompleted()
    {
        _inProgressView.SetActive(false);

        _completedView.SetActive(true);
    }

    /*
    private void OnMouseUp()
    {
        Debug.Log($"Mouse UP {name}");
        OnClickCallback?.Invoke();
    }
    */
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Point Click Handler");
        OnClickCallback?.Invoke();
    }

    public MissionUnit MissionUnit => _missionUnit;
}
