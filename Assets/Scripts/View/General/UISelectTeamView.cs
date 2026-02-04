using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UISelectTeamView : MonoBehaviour
{
    [SerializeField] private UIListDisplay _membersListDisplay;

    private Team _currentTeam;

    private Action<CharacterUnit> _onCharacterSelectedCallback;

    public void Setup(int teamSize, Action<CharacterUnit> OnCharacterSelectedCallback)
    {
        _currentTeam = new Team(teamSize);
        _onCharacterSelectedCallback = OnCharacterSelectedCallback;

        var placeHolders = new List<CharacterUnit>();

        for (int i = 0; i < teamSize; i++) placeHolders.Add(null);

        _membersListDisplay.SetItems(placeHolders, HandleMemberSelected);
    }

    private void HandleMemberSelected(UIItemController controller)
    {
        var selectMemberController = controller as UISelectTeamMemberItemView;

        if (selectMemberController.CharacterUnit != null)
        {
            _currentTeam.RmvMember(selectMemberController.CharacterUnit);
            _onCharacterSelectedCallback?.Invoke(selectMemberController.CharacterUnit);

            UpdateMembersList();
        }
    }

    public void AddMember(CharacterUnit character)
    {
        if (_currentTeam.Members.Contains(character)) return;

        _currentTeam.AddMember(character);

        UpdateMembersList();
    }

    private void UpdateMembersList()
    {
        var controllers = _membersListDisplay.GetControllers();

        for(int i = 0; i < controllers.Count; i++)
        {
            if (i < _currentTeam.Members.Count)
            {
                controllers[i].SetItem(_currentTeam.Members[i]);
            }
            else
            {
                controllers[i].SetItem(null);
            }
        }
    }

    public Team Team => _currentTeam;
}
