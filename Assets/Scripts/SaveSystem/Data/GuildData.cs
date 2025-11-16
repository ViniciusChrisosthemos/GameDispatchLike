using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GuildData
{
    public string Name;
    public int CurrentLevel;
    public int CurrentExperience;
    public int Balance;
    public int Reputation;
    public List<CharacterUnitData> AllCharacters;

    public GuildData(Guild guild)
    {
        Name = guild.PlayerName;
        Balance = guild.Balance;
        Reputation = guild.Reputation;
        CurrentLevel = guild.CurrentLevel;
        CurrentExperience = guild.CurrentExperience;
        AllCharacters = guild.AllCharacters.Select(c => new CharacterUnitData(c)).ToList();
    }

    public GuildData()
    {
        Name = "ERROR";
        Balance = 0;
        Reputation = 0;
        CurrentLevel = 1;
        CurrentExperience = 0;
        AllCharacters = new List<CharacterUnitData>();
    }
}
