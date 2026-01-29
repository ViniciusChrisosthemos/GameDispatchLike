using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UILobbyHomeScreenView : AbstractScreen
{
    [SerializeField] private UIListDisplay _lineupListDisplay;
    [SerializeField] private UIListDisplay _availableHerosListDisplay;
    [SerializeField] private UIListDisplay _dayResumeListDisplay;
    [SerializeField] private UIRadarChartController _radarChartController;

    protected override void InitScreen()
    {
        UpdateLists();
        UpdateDayResume();
    }

    private void HandleLineupHeroSelected(UIItemController controller)
    {
        controller.GetItem<CharacterUnit>().SetScheduledCharater(false);

        UpdateLists();
    }

    private void HandleAvailableHerosSelected(UIItemController controller)
    {
        var gameState = GameManager.Instance.GameState;

        if (gameState.Company.ScheduledCharacters.Count >= gameState.Company.MaxScheduledCharacters) return;

        controller.GetItem<CharacterUnit>().SetScheduledCharater(true);

        UpdateLists();
    }

    private void UpdateLists()
    {
        var gameState = GameManager.Instance.GameState;

        var lineup = gameState.Company.ScheduledCharacters;
        _lineupListDisplay.SetItems(lineup, HandleLineupHeroSelected);

        var availableHeros = gameState.Company.AvailableCharacters;
        _availableHerosListDisplay.SetItems(availableHeros, HandleAvailableHerosSelected);

        UpdateTeamStats(lineup);
    }

    private void UpdateTeamStats(List<CharacterUnit> characters)
    {
        var team = new Team(characters);

        if (team.Members.Count == 0)
        {
            _radarChartController.Clear();
        }
        else
        {
            _radarChartController.UpdateStats(team.GetTeamStats().GetAverageValues(team.Members.Count));
        }
    }

    private void UpdateDayResume()
    {
        var daySO = GameManager.Instance.GetCurrentDay();

        _dayResumeListDisplay.SetItems(daySO.Tips, null);
    }
}
