using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class UIDayManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _view;
    [SerializeField] private DayManager _dayManager;
    [SerializeField] private UIMissionManagerViewController _missionInfoController;
    [SerializeField] private AnimatePathController _animatePathController;

    [Header("UI/Missions")]
    [SerializeField] private Transform _baseTransform;
    [SerializeField] private Transform _missionParent;
    [SerializeField] private UIMissionController _missionPrefab;
    [SerializeField] private UIMissionManagerViewController _uiMissionManagerViewController;

    [Header("UI/Character")]
    [SerializeField] private Transform _dayCharacterParent;
    [SerializeField] private UIDayCharacterViewController _uiDayCharacterViewControllerPrefab;

    [Header("UI/Level Up")]
    [SerializeField] private UILevelUpController _uiLevelUpController;

    [Header("Screens")]
    [SerializeField] private UIDayReportController _uiDayReportController;
    [SerializeField] private UIGuildViewManager _uiGuildViewManager;

    [Header("Events")]
    public UnityEvent<CharacterUnit> OnCharacterSelected;
    public UnityEvent OnScreenOpened;

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
        var levelUpDescription = GameManager.Instance.CompleteDay(report);
        
        _uiDayReportController.OpenScreen(report, levelUpDescription, () =>
        {
            CloseScreen();
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
        _uiDayCharacterControllers.ForEach(controllers => controllers.UpdateTime(currentTime));
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

        _uiMissionManagerViewController.CloseScreen();
        OnScreenOpened?.Invoke();
    }

    private void HandleMissionSelected(MissionUnit missionSelected)
    {
        Debug.Log($"{missionSelected.Name} {missionSelected.IsMissionCompletedTheEvent()}");

        if (missionSelected.HasRandomEvent())
        {
            PauseDay();

            var herosSO = missionSelected.Team.Members.Select(m => m.BaseCharacterSO).ToList();

            _uiMissionManagerViewController.OpenMissionEvent(missionSelected, missionSelected.MissionEvent, (result) =>
            {
                HandleMissionCompleted(missionSelected, result);
                ResumeDay();
            });
        }
        else if (missionSelected.IsMissionCompleted())
        {
            HandleMissionCompletedByStats(missionSelected);
        }
        else if (missionSelected.IsMissionWaitingToBeAccepted())
        {
            PauseDay();

            _uiMissionManagerViewController.OpenAcceptMissionScreen(missionSelected, (mission, team) =>
            {
                if (mission != null || team != null) SendTeam(mission, team);

                ResumeDay();
            });

        }
    }

    private void HandleMissionCompletedByStats(MissionUnit missionUnit)
    {
        PauseDay();
        
        _uiMissionManagerViewController.OpenMissionResult(missionUnit, result =>
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
