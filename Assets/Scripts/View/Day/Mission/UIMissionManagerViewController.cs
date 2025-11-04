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


    public void OpenAcceptMissionScreen(MissionUnit mission, Action<MissionUnit, Team> callback)
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
            _animator.SetTrigger(_clearAnimationTrigger);
        });

        // Right window
        _uiRequirementViewController.UpdateWindow(mission.MissionSO.RequirementDescriptionItems, false);

        _animator.SetTrigger(_openMissionAnimationTrigger);
    }

    public void OpenMissionEvent(MissionUnit mission, RandomMissionEvent missionEvent, Action<bool> callback)
    {
        CloseWindows();
        _view.SetActive(true);

        //Left window
        _uiMissionInfoViewController.UpdateMissionInfo(mission);

        //Middle window
        _middleUIChoiceSelectionViewController.OpenScreen(mission, missionEvent, false, (choice) =>
        {
            OpenMissionEventResult(mission, missionEvent, choice, (result) => 
            { 
                callback?.Invoke(result); 
                CloseScreen(); 
            });
        });

        // Right window
        _uiAssignedHeroStatViewController.OpenScreen(mission.Team);
    }

    private void OpenMissionEventResult(MissionUnit mission, RandomMissionEvent missionEvent, MissionChoice missionChoice, Action<bool> callback)
    {
        CloseWindows();

        _uiMissionInfoViewController.UpdateMissionInfo(mission);

        _uiChoiceResultViewController.OpenScreen(mission, missionEvent, missionChoice, callback);

        _rightUIChoiceSelectionViewController.OpenScreen(mission, missionEvent, true, null);
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
            _animator.SetTrigger(_clearAnimationTrigger);
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
        _uiMissionInfoViewController.Close();

        _uiSelectTeamViewController.Close();
        _middleUIChoiceSelectionViewController.Close();
        _uiChoiceResultViewController.Close();
        _uiMissionResultViewController.Close();

        _uiRequirementViewController.Close();
        _rightUIChoiceSelectionViewController.Close();
        _uiAssignedHeroStatViewController.Close();
    }
}
