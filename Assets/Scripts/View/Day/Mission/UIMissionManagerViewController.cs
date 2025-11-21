using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using static MissionSO;

public class UIMissionManagerViewController: MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _view;
    [SerializeField] private Animator _animator;

    [Header("Left Side")]
    [SerializeField] private UIMissionInfoViewController _uiMissionInfoViewController;

    [Header("Middle")]
    [SerializeField] private UISelectTeamViewController _uiSelectTeamViewController;
    [SerializeField] private UISelectTeamForBattleView _uiSelecteTeamForBattleController;
    [SerializeField] private UIChoiceSelectionViewController _middleUIChoiceSelectionViewController;
    [SerializeField] private UIChoiceResultViewController _uiChoiceResultViewController;
    [SerializeField] private UIMissionResultViewController _uiMissionResultViewController;

    [Header("Right Side")]
    [SerializeField] private UIRequirementViewController _uiRequirementViewController;
    [SerializeField] private UIChoiceSelectionViewController _rightUIChoiceSelectionViewController;
    [SerializeField] private UIAssignedHeroStatViewController _uiAssignedHeroStatViewController;

    [Header("Animator Events")]
    [SerializeField] private string _clearAnimationTrigger = "Clear";
    [SerializeField] private string _openMissionAnimationTrigger = "OpenMission";
    [SerializeField] private string _openMissionResultAnimationTrigger = "OpenMissionResult";
    [SerializeField] private string _normalChoiceResultAnimationTrigger = "OpenChoiceResult-NormalResult";
    [SerializeField] private string _characterChoiceResultAnimationTrigger = "OpenChoiceResult-CharacterResult";
    [SerializeField] private string _selectChoiceAnimationTrigger = "SelectChoice";


    public void OpenAcceptMissionScreen(MissionUnit mission, Action<MissionUnit, Team> callback, Action closeWindowCallback)
    {
        CloseWindows();
        _view.SetActive(true); 

        //Left window
        _uiMissionInfoViewController.UpdateMissionInfo(mission);

        //Middle window
        _uiSelectTeamViewController.OpenScreen(mission, (selectedMission, team) =>
        {
            callback?.Invoke(selectedMission, team);
            CloseScreen();
            ResetAnimator();
        },() =>
        {
            CloseScreen();
            ResetAnimator();
            closeWindowCallback?.Invoke();
        });

        // Right window
        _uiRequirementViewController.UpdateWindow(mission.MissionSO.RequirementDescriptionItems, false);

        _animator.SetTrigger(_openMissionAnimationTrigger);
    }

    public void OpenSelectionForBattle(MissionUnit mission, Team enemyTeam, Action<Team> onTeamSelected, Action closeCallback)
    {
        Debug.Log($"[{GetType()}][OpenSelectionForBattle]");
        CloseWindows();
        _view.SetActive(true);

        _uiMissionInfoViewController.UpdateMissionInfo(mission);

        _uiSelecteTeamForBattleController.OpenScreen(mission.MaxTeamSize, enemyTeam, (playeTeam) =>
        {
            CloseScreen();
            onTeamSelected?.Invoke(playeTeam);
        }, () =>
        {
            CloseScreen();
            closeCallback?.Invoke();
        });

        _uiRequirementViewController.UpdateWindow(mission.MissionSO.RequirementDescriptionItems, false);
    }

    public void OpenMissionEvent(MissionUnit mission, RandomMissionEvent missionEvent, Action<bool> callback, Action closeCallback)
    {
        CloseWindows();
        _view.SetActive(true);

        //Left window
        _uiMissionInfoViewController.UpdateMissionInfo(mission);

        //Middle window
        _middleUIChoiceSelectionViewController.OpenScreen(mission, missionEvent, false, (choice) =>
        {
            _animator.SetTrigger(_clearAnimationTrigger);

            OpenMissionEventResult(mission, missionEvent, choice, (result) => 
            { 
                callback?.Invoke(result);
                _animator.SetTrigger(_clearAnimationTrigger);
                CloseScreen(); 
            });
        }, () =>
        {
            ResetAnimator();
            CloseScreen();
            closeCallback?.Invoke();
        });

        // Right window
        _uiAssignedHeroStatViewController.OpenScreen(mission.Team);

        _animator.SetTrigger(_selectChoiceAnimationTrigger);
    }

    private void OpenMissionEventResult(MissionUnit mission, RandomMissionEvent missionEvent, MissionChoice missionChoice, Action<bool> callback)
    {
        //CloseWindows();
        _view.SetActive(true);

        //_uiMissionInfoViewController.UpdateMissionInfo(mission);

        _uiChoiceResultViewController.OpenScreen(mission, missionEvent, missionChoice, (result) =>
        {
            callback?.Invoke(result);
            CloseScreen();
        });

        _rightUIChoiceSelectionViewController.OpenScreen(mission, missionEvent, true, null, null);

        if (missionChoice.Character == null)
        {
            _animator.SetTrigger(_normalChoiceResultAnimationTrigger);
        }
        else
        {
            _animator.SetTrigger(_characterChoiceResultAnimationTrigger);
        }
    }

    public void OpenMissionResult(MissionUnit mission, Action<bool> callback)
    {
        CloseWindows();
        _view.SetActive(true);

        //Left window
        _uiMissionInfoViewController.UpdateMissionInfo(mission);

        //Middle window
        _uiMissionResultViewController.OpenResultScreen(mission, (result) =>
        {
            callback?.Invoke(result);
            CloseScreen();
            ResetAnimator();
        });

        // Right window
        _uiRequirementViewController.UpdateWindow(mission.MissionSO.RequirementDescriptionItems, true);


        _animator.SetTrigger(_openMissionResultAnimationTrigger);
    }

    public void CloseScreen()
    {
        _view.SetActive(false);
    }

    private void CloseWindows()
    {
        _uiMissionInfoViewController.CloseWithoutNotify();

        _uiSelectTeamViewController.CloseWithoutNotify();
        _uiSelecteTeamForBattleController.CloseWithoutNotify();
        _middleUIChoiceSelectionViewController.CloseWithoutNotify();
        _uiChoiceResultViewController.CloseWithoutNotify();
        _uiMissionResultViewController.CloseWithoutNotify();

        _uiRequirementViewController.CloseWithoutNotify();
        _rightUIChoiceSelectionViewController.CloseWithoutNotify();
        _uiAssignedHeroStatViewController.CloseWithoutNotify();
    }

    public void ResetAnimator()
    {
        _animator.SetTrigger(_clearAnimationTrigger);
    }
}
