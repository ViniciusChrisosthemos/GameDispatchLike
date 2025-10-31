using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static MissionSO;

public class UIDayManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _view;
    [SerializeField] private DayManager _dayManager;
    [SerializeField] private UIMissionInfoController _missionInfoController;
    [SerializeField] private AnimatePathController _animatePathController;

    [Header("UI/Missions")]
    [SerializeField] private Transform _baseTransform;
    [SerializeField] private Transform _missionParent;
    [SerializeField] private UIMissionController _missionPrefab;
    [SerializeField] private UIMissionResultController _uiMissionResultController;
    [SerializeField] private UIMissionEventController _uiMissionEventController;
    [SerializeField] private UIEventCompleteController _uiEventCompleteController;

    [Header("UI/Character")]
    [SerializeField] private Transform _dayCharacterParent;
    [SerializeField] private UIDayCharacterViewController _uiDayCharacterViewControllerPrefab;

    [Header("UI/Day Timer")]
    [SerializeField] private Slider _sliderDayTimer;

    [Header("UI/Level Up")]
    [SerializeField] private UILevelUpController _uiLevelUpController;

    [Header("Screens")]
    [SerializeField] private UIDayReportController _uiDayReportController;
    [SerializeField] private UIGuildViewManager _uiGuildViewManager;


    [Header("Events")]
    public UnityEvent<CharacterUnit> OnCharacterSelected;

    private List<UIMissionController> _uiMissionControllers;
    private List<UIDayCharacterViewController> _uiDayCharacterControllers;

    private void Awake()
    {
        _dayManager.OnDayStart.AddListener(HandleDayStarted);
        _dayManager.OnMissionAvailable.AddListener(HandleMissionAvailableEvent);
        _dayManager.OnTimeUpdated.AddListener(HandleTimeUpdatedEvet);
        _dayManager.OnDayEnd.AddListener(HandleDayEnded);
    }

    private void HandleDayEnded(DayReport report)
    {
        _uiDayReportController.OpenScreen(report, () =>
        {
            CloseScreen();

            GameManager.Instance.CompleteDay();

            _uiGuildViewManager.OpenScreen();
        });
    }

    private void CloseScreen()
    {
        _view.SetActive(false);
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
            instance.transform.position = mission.Location.position;

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
        _view.SetActive(true);

        _sliderDayTimer.value = 1;

        _missionParent.ClearChilds();
        _dayCharacterParent.ClearChilds();

        _uiMissionControllers = new List<UIMissionController>();
        _uiDayCharacterControllers = new List<UIDayCharacterViewController>();

        foreach (var character in characters)
        {
            var controller = Instantiate(_uiDayCharacterViewControllerPrefab, _dayCharacterParent);

            controller.Init(this, character, c => OnCharacterSelected?.Invoke(c));

            _uiDayCharacterControllers.Add(controller);
        }

        _missionInfoController.CloseScreen();
    }

    private void HandleMissionSelected(MissionUnit missionSelected)
    {
        Debug.Log($"{missionSelected.Name} {missionSelected.IsMissionCompletedTheEvent()}");

        if (missionSelected.HasRandomEvent())
        {
            _dayManager.PauseDay();

            var herosSO = missionSelected.Team.Members.Select(m => m.BaseCharacterSO).ToList();

            _uiMissionEventController.OpenScreen(herosSO, missionSelected.MissionEvent, choice => HandleChoiceMade(missionSelected, choice));
        }
        else if (missionSelected.IsMissionCompleted())
        {
            HandleMissionCompletedByStats(missionSelected);
        }
        else if (missionSelected.IsMissionWaitingToBeAccepted())
        {
            _dayManager.PauseDay();

            _missionInfoController.OpenScreen(missionSelected);

        }else if (missionSelected.IsMissionCompletedTheEvent())
        {
            _uiEventCompleteController.OpenScreen(missionSelected, () => HandleMissionCompletedByChoices(missionSelected));
        }
    }

    private void HandleChoiceMade(MissionUnit mission, MissionChoice choice)
    {
        mission.MakeChoice(_dayManager.CurrentTime, choice);

        ResumeDay();
    }

    private void HandleMissionCompletedByChoices(MissionUnit missionUnit)
    {
        HandleMissionCompleted(missionUnit, true);
    }

    private void HandleMissionCompletedByStats(MissionUnit missionUnit)
    {
        _dayManager.PauseDay();
        
        _uiMissionResultController.OpenResultScreen(missionUnit, result =>
        {
            ResumeDay();

            HandleMissionCompleted(missionUnit, result);
        });
    }

    private void HandleMissionCompleted(MissionUnit missionUnit, bool result)
    {
        _dayManager.ClaimMission(missionUnit, result);

        var controller = _uiMissionControllers.Find(m => m.MissionUnit.ID == missionUnit.ID);
        HandleCallForDeleteMission(controller);

        _animatePathController.AnimatePath(missionUnit.Team, missionUnit.Location, _baseTransform, () =>
        {
            _dayManager.HandleCharacterArriveBase(missionUnit.Team, _dayManager.CurrentTime);
        });
    }

    public void ResumeDay()
    {
        _dayManager.ResumeDay();
    }

    public void PauseDay()
    {
        _dayManager.PauseDay();
    }

    public void SendTeam(MissionUnit mission, Team currentTeam)
    {
        _dayManager.AcceptMission(mission, currentTeam);

        _animatePathController.AnimatePath(currentTeam, _baseTransform, mission.Location, () =>
        {
            _dayManager.StartMission(mission);
        });
    }

    public void OpenLevelUpScreen(CharacterUnit characterUnit)
    {
        _uiLevelUpController.OpenScreen(characterUnit);
    }
}
