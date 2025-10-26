using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIDayManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DayManager _dayManager;
    [SerializeField] private UIMissionInfoController _missionInfoController;

    [Header("UI/Missions")]
    [SerializeField] private Transform _missionParent;
    [SerializeField] private UIMissionController _missionPrefab;

    [Header("UI/Character")]
    [SerializeField] private Transform _dayCharacterParent;
    [SerializeField] private UIDayCharacterViewController _uiDayCharacterViewControllerPrefab;

    [Header("UI/Day Timer")]
    [SerializeField] private Slider _sliderDayTimer;

    [Header("UI/Level Up")]
    [SerializeField] private UILevelUpController _uiLevelUpController;

    [Header("Events")]
    public UnityEvent<CharacterUnit> OnCharacterSelected;

    private List<UIMissionController> _uiMissionControllers;
    private List<UIDayCharacterViewController> _uiDayCharacterControllers;

    private void Awake()
    {
        _dayManager.OnDayStart.AddListener(HandleDayStarted);
        _dayManager.OnMissionAvailable.AddListener(HandleMissionAvailableEvent);
        _dayManager.OnTimeUpdated.AddListener(HandleTimeUpdatedEvet);
    }

    private void Start()
    {
        _missionInfoController.CloseScreen();

        _missionParent.ClearChilds();

        _uiMissionControllers = new List<UIMissionController>();

        _sliderDayTimer.value = 1;
    }

    private void HandleTimeUpdatedEvet(float currentTime)
    {
        _uiMissionControllers.ForEach(controller => controller.UpdateTime(currentTime));

        _sliderDayTimer.value = 1 - (currentTime / _dayManager.TotalDayTime);
    }

    private void HandleMissionAvailableEvent(List<MissionUnit> missions)
    {
        foreach(var mission in missions)
        {
            var instance = Instantiate(_missionPrefab, _missionParent);

            instance.Init(mission, HandleMissionSelected, HandleCallForDeleteMission);

            _uiMissionControllers.Add(instance);
        }
    }

    private void HandleCallForDeleteMission(UIMissionController controller)
    {
        _uiMissionControllers.Remove(controller);
        Destroy(controller.gameObject);
    }

    private void HandleDayStarted(List<CharacterUnit> characters)
    {
        _dayCharacterParent.ClearChilds();
        _uiDayCharacterControllers = new List<UIDayCharacterViewController>();

        foreach (var character in characters)
        {
            var controller = Instantiate(_uiDayCharacterViewControllerPrefab, _dayCharacterParent);

            controller.Init(this, character, c => OnCharacterSelected?.Invoke(c));

            _uiDayCharacterControllers.Add(controller);
        }
    }

    private void HandleMissionSelected(MissionUnit missionSelected)
    {
        if (missionSelected.IsMissionCompleted())
        {
            _dayManager.ClaimMission(missionSelected);

            var controller = _uiMissionControllers.Find(m => m.MissionUnit.ID == missionSelected.ID);
            HandleCallForDeleteMission(controller);
        }
        else if (missionSelected.IsMissionWaitingToBeAccepted())
        {
            _dayManager.PauseDay();

            _missionInfoController.OpenScreen(missionSelected);
        }
    }

    public void ResumeDay()
    {
        _dayManager.ResumoDay();
    }

    public void SendTeam(MissionUnit mission, Team currentTeam)
    {
        _dayManager.AcceptMission(mission, currentTeam);
    }

    public void OpenLevelUpScreen(CharacterUnit characterUnit)
    {
        _uiLevelUpController.OpenScreen(characterUnit);
    }
}
