using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISelectTeamViewController : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private UIDayManager _manager;

    [Header("References")]
    [SerializeField] private GameObject _view;
    [SerializeField] private Button _btnCloseScreen;
    [SerializeField] private Transform _teamMembersParent;
    [SerializeField] private UITeamMemberController _teamMemberPrefab;
    [SerializeField] private UIRadarChartController _radarChartTeam;
    [SerializeField] private Button _btnSendTeam;


    [Header("Settings")]
    [SerializeField] private GameSettingsSO _gameSettings;

    private Team _currentTeam;
    private MissionUnit _currentMission;
    private List<UITeamMemberController> _teamMemberControllers;

    private Action<MissionUnit, Team> _callback;

    private void Start()
    {
        _btnSendTeam.onClick.AddListener(() => SendTeam());
        _btnCloseScreen.onClick.AddListener(() => SendTeam());
    }

    public void OpenScreen(MissionUnit mission, Action<MissionUnit, Team> callback)
    {
        _view.SetActive(true);

        _currentMission = mission;
        _manager.OnCharacterSelected.AddListener(HandleCharacterSelected);

        _callback = callback;

        UpdateScreen(mission);
    }

    private void UpdateScreen(MissionUnit mission)
    {
        _teamMembersParent.ClearChilds();

        _currentTeam = new Team(mission.MaxTeamSize);

        UpdateTeamRadarChart();

        _teamMemberControllers = new List<UITeamMemberController>();
        for (int i = 0; i < mission.MaxTeamSize; i++)
        {
            var teamMemberController = Instantiate(_teamMemberPrefab, _teamMembersParent);
            teamMemberController.Init(null, HandleMemberTeamSelected);

            _teamMemberControllers.Add(teamMemberController);
        }

        _btnSendTeam.interactable = false;
    }

    private void HandleMemberTeamSelected(UIItemController controller)
    {
        var character = controller.GetItem<CharacterUnit>();

        if (character != null)
        {
            RmvMemberFromTeam(character);
        }
    }

    private void HandleCharacterSelected(CharacterUnit characterUnit)
    {
        if (_currentTeam.Members.Contains(characterUnit)) return;

        AddMemberToTeam(characterUnit);
    }

    private void AddMemberToTeam(CharacterUnit characterUnit)
    {
        _currentTeam.AddMember(characterUnit);

        foreach (var controller in _teamMemberControllers)
        {
            if (controller.GetItem<CharacterUnit>() == null)
            {
                controller.AddCharacter(characterUnit);
                break;
            }
        }

        UpdateTeamRadarChart();

        _btnSendTeam.interactable = true;
    }

    private void RmvMemberFromTeam(CharacterUnit characterUnit)
    {
        _currentTeam.RmvMember(characterUnit);

        var controller = _teamMemberControllers.Find(controller => controller.GetItem<CharacterUnit>() == characterUnit);
        controller.RmvCharacter();

        UpdateTeamRadarChart();

        _btnSendTeam.interactable = _currentTeam.Size != 0;
    }

    private void UpdateTeamRadarChart()
    {
        _radarChartTeam.UpdateStats(_currentTeam.GetTeamStats().GetValues());
    }

    public void Close()
    {
        _view.SetActive(false);

        _manager.OnCharacterSelected.RemoveListener(HandleCharacterSelected);
    }

    private void SendTeam()
    {
        if (_currentTeam != null && _currentTeam.Members.Count != 0)
        {
            _callback?.Invoke(_currentMission, _currentTeam);
        }
        else
        {
            _callback?.Invoke(null, null);
        }
        
        Close();
    }
}
