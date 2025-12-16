using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UISelectTeamForBattleView : MonoBehaviour
{
    [SerializeField] private UIDayManager _uiDayManager;

    [SerializeField] private GameObject _view;
    [SerializeField] private UIListDisplay _enemyTeamListDisplay;
    [SerializeField] private UIListDisplay _playerTeamListDisplay;
    [SerializeField] private Button _btnSendTeam;
    [SerializeField] private Button _btnCloseScreen;

    private Team _currentTeam;
    private Action<Team> _onSendTeamCallback;
    private Action _onCloseScreenCallback;

    public void OpenScreen(int maxTeamSize, Team enemyTeam, Action<Team> onSendTeamCallback, Action onCloseScreenCallback)
    {
        _view.SetActive(true);

        _currentTeam = new Team(maxTeamSize);
        var emptyTeam = new List<object>();
        for (int i = 0; i < maxTeamSize; i++) emptyTeam.Add(null);

        _playerTeamListDisplay.SetItems(emptyTeam, HandleTeamCharacterSelected);

        _enemyTeamListDisplay.SetItems(enemyTeam.Members, null);

        _btnSendTeam.onClick.RemoveAllListeners();
        _btnSendTeam.onClick.AddListener(SendTeam);

        _btnCloseScreen.onClick.RemoveAllListeners();
        _btnCloseScreen.onClick.AddListener(CloseScreen);

        _btnSendTeam.interactable = false;

        _onSendTeamCallback = onSendTeamCallback;
        _onCloseScreenCallback = onCloseScreenCallback;

        _uiDayManager.OnCharacterSelected.RemoveListener(HandleCharacterSelected);
        _uiDayManager.OnCharacterSelected.AddListener(HandleCharacterSelected);
    }

    private void HandleTeamCharacterSelected(UIItemController controller)
    {
        var character = controller.GetItem<CharacterUnit>();

        _currentTeam.RmvMember(character);
        UpdatePlayeTeam();
    }

    private void HandleCharacterSelected(CharacterUnit character)
    {
        _currentTeam.AddMember(character);
        UpdatePlayeTeam();

        _btnSendTeam.interactable = true;
    }

    private void UpdatePlayeTeam()
    {
        var controllers = _playerTeamListDisplay.GetControllers().Select(c => c.GetComponent<UITeamMemberController>()).ToList();

        controllers.ForEach(c => c.RmvCharacter());

        for (int i = 0; i < _currentTeam.Members.Count; i++)
        {
            controllers[i].AddCharacter(_currentTeam.Members[i]);
        }
    }

    private void SendTeam()
    {
        _uiDayManager.OnCharacterSelected.RemoveListener(HandleCharacterSelected);

        _onSendTeamCallback?.Invoke(_currentTeam);
    }

    public void CloseScreen()
    {
        _view.SetActive(false); 
        _onCloseScreenCallback?.Invoke();
    }

    public void Close()
    {
        _view.SetActive(false);
    }
    public void CloseWithoutNotify()
    {
        _view.SetActive(false);
    }
}
