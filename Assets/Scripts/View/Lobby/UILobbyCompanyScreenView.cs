using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyCompanyScreenView : AbstractScreen
{
    [SerializeField] private UIListDisplay _companyStatisticsListDisplay;
    [SerializeField] private UICharacterUnitView _selectedCharacterView;
    [SerializeField] private UIListDisplay _allCharactersListDisplay;
    [SerializeField] private Button _btnDismiss;
    [SerializeField] private GameObject _characterInfoPlaceHolder;

    protected override void InitScreen()
    {
        var gameState = GameManager.Instance.GameState;

        var statistics = new List<string>();

        statistics.Add($"Total Missions Accepted: {gameState.Company.TotalMissionsAccepted}");
        statistics.Add($"Total Missions Declined: {gameState.Company.TotalMissionsDeclined}");
        statistics.Add($"Total Missions Completed: {gameState.Company.TotalMissionsCompleted}");
        statistics.Add($"Total Missions Failed: {gameState.Company.TotalMissionsFailed}");

        _companyStatisticsListDisplay.SetItems(statistics, null);

        UpdateCharacterList();

        _btnDismiss.interactable = false;

        _characterInfoPlaceHolder.SetActive(true);
    }

    private void HandleCharacterSelected(UIItemController controller)
    {
        var character = controller.GetItem<CharacterUnit>();

        _selectedCharacterView.SetItem(character);

        _characterInfoPlaceHolder.SetActive(false);
    }

    private void UpdateCharacterList()
    {
        _allCharactersListDisplay.SetItems(GameManager.Instance.GameState.Company.AllCharacters, HandleCharacterSelected);
    }

    public void DismissCharacter()
    {
        var character = _selectedCharacterView.CharacterUnit.BaseCharacterSO;

        GameManager.Instance.DismissCharacter(character);

        _characterInfoPlaceHolder.SetActive(true);

        UpdateCharacterList();

        UpdateMainScreen();
    }
}
