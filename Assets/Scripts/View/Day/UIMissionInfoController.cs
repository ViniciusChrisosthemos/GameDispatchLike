using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionInfoController : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private UIDayManager _manager;
    [SerializeField] private Button _btnCloseScreen;

    [Header("Character View")]
    [SerializeField] private GameObject _characterView;
    [SerializeField] private UICharacterViewController _characterViewController;
    [SerializeField] private Button _btnAddMemberToTeam;
    [SerializeField] private Button _btnRmvMemberToTeam;

    [Header("Team")]
    [SerializeField] private UIRadarChartController _radarChartTeam;
    [SerializeField] private Transform _teamMembersParent;
    [SerializeField] private UITeamMemberController _teamMemberPrefab;

    [Header("Mission Info")]
    [SerializeField] private TextMeshProUGUI _txtMissionName;
    [SerializeField] private TextMeshProUGUI _txtMissionDescription;
    [SerializeField] private TextMeshProUGUI _txtMissionExp;
    [SerializeField] private TextMeshProUGUI _txtMissionGold;
    [SerializeField] private Button _btnSendTeam;

    private Team _currentTeam;
    private MissionUnit _currentMission;
    private CharacterUnit _characterUnit;

    private List<UITeamMemberController> _teamMemberControllers;

    private void Awake()
    {
        _btnSendTeam.onClick.AddListener(() => SendTeam());

        _btnAddMemberToTeam.onClick.AddListener(() => AddMemberToTeam(_characterUnit));
        _btnRmvMemberToTeam.onClick.AddListener(() => RmvMemberFromTeam(_characterUnit));

        _btnCloseScreen.onClick.AddListener(() => CloseScreen());
    }

    public void OpenScreen(MissionUnit mission)
    {
        _view.SetActive(true);

        _currentMission = mission;
        _manager.OnCharacterSelected.AddListener(HandleCharacterSelected);

        UpdateScreen(mission);
    }

    private void UpdateScreen(MissionUnit mission)
    {
        _teamMembersParent.ClearChilds();

        _currentTeam = new Team(mission.MaxTeamSize);

        _characterView.SetActive(false);

        UpdateTeamRadarChart();

        _teamMemberControllers = new List<UITeamMemberController>();
        for (int i = 0; i < mission.MaxTeamSize; i++)
        {
            var teamMemberController = Instantiate(_teamMemberPrefab, _teamMembersParent);

            _teamMemberControllers.Add(teamMemberController);
        }

        _txtMissionName.text = mission.Name;
        _txtMissionDescription.text = mission.Description;
        _txtMissionExp.text = mission.Exp.ToString();
        _txtMissionGold.text = mission.Gold.ToString();
        _btnSendTeam.interactable = false;
    }

    private void HandleCharacterSelected(CharacterUnit characterUnit)
    {
        if (_characterUnit != characterUnit)
        {
            _characterUnit = characterUnit;

            _characterView.SetActive(true);
            _characterViewController.UpdateCharacter(characterUnit);

            bool isMember = _currentTeam.HasMember(characterUnit);
            bool isTeamFull = _currentTeam.Size == _currentTeam.MaxSize;

            _btnAddMemberToTeam.gameObject.SetActive(!isMember && !isTeamFull);
            _btnRmvMemberToTeam.gameObject.SetActive(isMember);
        }
        else
        {
            _characterUnit = null;

            _characterView.SetActive(false);
        }
    }

    private void AddMemberToTeam(CharacterUnit characterUnit)
    {
        _currentTeam.AddMember(characterUnit);

        foreach (var controller in _teamMemberControllers)
        {
            if (controller.CharacterUnit == null)
            {
                controller.AddCharacter(characterUnit);
                break;
            }
        }

        UpdateTeamRadarChart();

        _btnAddMemberToTeam.gameObject.SetActive(false);
        _btnRmvMemberToTeam.gameObject.SetActive(true);

        _btnSendTeam.interactable = true;
    }

    private void RmvMemberFromTeam(CharacterUnit characterUnit)
    {
        _currentTeam.RmvMember(characterUnit);

        var controller = _teamMemberControllers.Find(controller => controller.CharacterUnit == characterUnit);
        controller.RmvCharacter();

        UpdateTeamRadarChart();

        _btnAddMemberToTeam.gameObject.SetActive(true);
        _btnRmvMemberToTeam.gameObject.SetActive(false);

        _btnSendTeam.interactable = _currentTeam.Size != 0;
    }

    private void UpdateTeamRadarChart()
    {
        _radarChartTeam.UpdateStats(_currentTeam.GetTeamStats().GetValues());
    }

    private void HandleCharacterDeselected(CharacterUnit character)
    {
        _currentTeam.RmvMember(character);
    }

    public void CloseScreen()
    {
        _view.SetActive(false);

        _manager.OnCharacterSelected.RemoveListener(HandleCharacterSelected);
        _manager.ResumeDay();
    }

    private void SendTeam()
    {
        _manager.SendTeam(_currentMission, _currentTeam);

        CloseScreen();
    }
}
