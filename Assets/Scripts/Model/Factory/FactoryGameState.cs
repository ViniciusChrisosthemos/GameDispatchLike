using System;
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

    public GameState CreateGameState(string saveFile, GameStateData gameStateData)
    {
        var guildName = gameStateData.GuildData.Name;
        var balance = gameStateData.GuildData.Balance;
        var reputation = gameStateData.GuildData.Reputation;
        var currentLevel = gameStateData.GuildData.CurrentLevel;
        var currentExperience = gameStateData.GuildData.CurrentExperience;

        var allCharacters = new List<CharacterUnit>();

        allCharacters = gameStateData.GuildData.AllCharacters.Select(c => _factoryCharacterUnit.CreateCharacterUnit(c)).ToList();

        var guild = new Guild(guildName, balance, reputation, currentLevel, currentExperience, allCharacters);

        return new GameState(saveFile, gameStateData.CurrentDay, guild);
    }

    public GameState CreateGameState(string saveFile, string guildName, GameStateSO defaultGameState)
    {
        var balance = defaultGameState.Balance;
        var reputation = defaultGameState.Reputation;
        var currentLevel = defaultGameState.CurrentLevel;
        var currentExperience = defaultGameState.CurrentExperience;

        var allcharacters = defaultGameState.AvailableCharacters.Select(c => _factoryCharacterUnit.CreateCharacterUnit(c)).ToList();

        var guild = new Guild(guildName, balance, reputation, currentLevel, currentExperience, allcharacters);

        return new GameState(saveFile, defaultGameState.CurrentDay, guild);
    }
}
