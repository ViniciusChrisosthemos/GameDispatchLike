using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMissionInfoController : MonoBehaviour
{
    [SerializeField] private GameObject _view;

    [Header("Team")]
    [SerializeField] private Transform _teamMembersParent;
    [SerializeField] private UITeamMemberController _teamMemberPrefab;

    private Team _currentTeam;

    public void OpenScreen(MissionUnit mission)
    {
        _view.SetActive(true);

        _teamMembersParent.ClearChilds();

        _currentTeam = new Team(mission.MaxTeamSize);

        for (int i = 0; i < mission.MaxTeamSize; i++)
        {
            var teamMemberController = Instantiate(_teamMemberPrefab, _teamMembersParent);
            teamMemberController.Init(HandleCharacterDeselected);
        }
    }

    private void HandleCharacterDeselected(CharacterUnit character)
    {
        _currentTeam.RmvMember(character);
    }

    public void CloseScreen()
    {
        _view.SetActive(false);
    }
}
