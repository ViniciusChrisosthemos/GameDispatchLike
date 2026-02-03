using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyCompanyScreenView : AbstractScreen
{
    [SerializeField] private UIListDisplay _companyStatisticsListDisplay;
    [SerializeField] private UICharacterViewController _selectedCharacterView;
    [SerializeField] private UIListDisplay _allCharactersListDisplay;
    [SerializeField] private Button _btnDismiss;

    protected override void InitScreen()
    {
        var gameState = GameManager.Instance.GameState;

        var statistics = new List<string>();

        statistics.Add($"Total Missions Accepted: {gameState.Company.TotalMissionsAccepted}");
        statistics.Add($"Total Missions Declined: {gameState.Company.TotalMissionsDeclined}");
        statistics.Add($"Total Missions Completed: {gameState.Company.TotalMissionsCompleted}");
        statistics.Add($"Total Missions Failed: {gameState.Company.TotalMissionsFailed}");

        _companyStatisticsListDisplay.SetItems(statistics, null);

        _allCharactersListDisplay.SetItems(gameState.Company.AllCharacters, HandleCharacterSelected);

        _btnDismiss.interactable = false;
    }

    private void HandleCharacterSelected(UIItemController controller)
    {
        var character = controller.GetItem<CharacterUnit>();

        _selectedCharacterView.SetItem(character);
    }
}
