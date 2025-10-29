using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FactoryGameState
{
    private FactoryCharacterUnit _factoryCharacterUnit;

    public FactoryGameState(FactoryCharacterUnit factoryCharacterUnit)
    {
        _factoryCharacterUnit = factoryCharacterUnit;
    }

    public GameState CreateGameState(GameStateData gameStateData)
    {
        var guildName = gameStateData.GuildData.Name;
        var balance = gameStateData.GuildData.Balance;
        var reputation = gameStateData.GuildData.Reputation;

        var hiredCharacter = gameStateData.GuildData.HiredCharacters.Select(c => _factoryCharacterUnit.CreateCharacterUnit(c)).ToList();
        var scheduledCharacters = hiredCharacter.Where(c => gameStateData.GuildData.ScheduledCharacters.Contains(c.BaseCharacterSO.ID)).ToList();
        
        scheduledCharacters.ForEach(c => hiredCharacter.Remove(c));

        var guild = new Guild(guildName, balance, reputation, hiredCharacter, scheduledCharacters);

        return new GameState(gameStateData.CurrentDay, guild);
    }

    public GameState CreateGameState(string guildName, GameStateSO defaultGameState)
    {
        var balance = defaultGameState.Balance;
        var reputation = defaultGameState.Reputation;

        var hiredCharacter = defaultGameState.AvailableCharacters.Select(c => _factoryCharacterUnit.CreateCharacterUnit(c)).ToList();

        var guild = new Guild(guildName, balance, reputation, hiredCharacter, new List<CharacterUnit>());

        return new GameState(defaultGameState.CurrentDay, guild);
    }
}
