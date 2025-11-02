using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionInfoController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _view;
    [SerializeField] private UIDayManager _manager;
    [SerializeField] private Button _btnCloseScreen;

    [Header("Left Side")]
    [SerializeField] private TextMeshProUGUI _txtMissionType;
    [SerializeField] private Image _imgMissionClient;
    [SerializeField] private Image _imgMissionBackground;
    [SerializeField] private TextMeshProUGUI _txtMissionDescription;

    [Header("Middle")]
    [SerializeField] private UIRadarChartController _radarChartTeam;
    [SerializeField] private Transform _teamMembersParent;
    [SerializeField] private UITeamMemberController _teamMemberPrefab;

    [Header("Right Side")]
    [SerializeField] private UIRequirementDescriptionController _uiRequirementDescriptionController;
    [SerializeField] private Button _btnSendTeam;

    [Header("Settings")]
    [SerializeField] private GameSettingsSO _gameSettings;

    private Team _currentTeam;
    private MissionUnit _currentMission;

    private List<UITeamMemberController> _teamMemberControllers;

    private void Awake()
    {
        _btnSendTeam.onClick.AddListener(() => SendTeam());

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

        UpdateTeamRadarChart();

        _teamMemberControllers = new List<UITeamMemberController>();
        for (int i = 0; i < mission.MaxTeamSize; i++)
        {
            var teamMemberController = Instantiate(_teamMemberPrefab, _teamMembersParent);
            teamMemberController.Init(null, HandleMemberTeamSelected);

            _teamMemberControllers.Add(teamMemberController);
        }

        _txtMissionType.text = mission.MissionSO.Type.ToString();
        _imgMissionBackground.sprite = mission.MissionSO.EnvironmentArt;
        _imgMissionClient.sprite = mission.MissionSO.ClientArt;
        _txtMissionDescription.text = mission.MissionSO.Description;

        _uiRequirementDescriptionController.SetDescription(mission.MissionSO.RequirementDescriptionItems, false);

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
