using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyCompanyScreenView : AbstractScreen
{
    [SerializeField] private UIListDisplay _companyStatisticsListDisplay;
    [SerializeField] private AbstractUICharacterView _selectedCharacterView;
    [SerializeField] private UIListDisplay _allCharactersListDisplay;
    [SerializeField] private Button _btnDismiss;
    [SerializeField] private GameObject _selectedCharacterPlaceHolder;

    private void Awake()
    {
        _btnDismiss.onClick.AddListener(DismissCharacter);
    }

    protected override void InitScreen()
    {
        var gameState = GameManager.Instance.GameState;

        var statistics = new List<string>();

        statistics.Add($"Total Missions Accepted: {gameState.Company.TotalMissionsAccepted}");
        statistics.Add($"Total Missions Declined: {gameState.Company.TotalMissionsDeclined}");
        statistics.Add($"Total Missions Completed: {gameState.Company.TotalMissionsCompleted}");
        statistics.Add($"Total Missions Failed: {gameState.Company.TotalMissionsFailed}");

        _companyStatisticsListDisplay.SetItems(statistics, null);

        _selectedCharacterPlaceHolder.SetActive(true);
        
        UpdateList();
    }

    private void HandleCharacterSelected(UIItemController controller)
    {
        var character = controller.GetItem<CharacterUnit>();

        _selectedCharacterView.SetItem(character);

        _selectedCharacterPlaceHolder.SetActive(false);
    }

    private void DismissCharacter()
    {
        var character = _selectedCharacterView.GetItem<CharacterUnit>();
        
        GameManager.Instance.DismissCharacter(character);
        UpdateMainScreen();
        _selectedCharacterPlaceHolder.SetActive(true);
    }

    private void UpdateList()
    {
        var gameState = GameManager.Instance.GameState;
        _allCharactersListDisplay.SetItems(gameState.Company.AllCharacters, HandleCharacterSelected);
    }
}
