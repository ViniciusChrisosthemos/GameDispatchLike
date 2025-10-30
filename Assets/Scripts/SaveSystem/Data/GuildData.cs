using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GuildData
{
    public string Name;
    public int Balance;
    public int Reputation;
    public List<CharacterUnitData> AllCharacters;

    public GuildData(Guild guild)
    {
        Name = guild.Name;
        Balance = guild.Balance;
        Reputation = guild.Reputation;
        AllCharacters = guild.AllCharacters.Select(c => new CharacterUnitData(c)).ToList();
    }

    public GuildData()
    {
        Name = "ERROR";
        Balance = 0;
        Reputation = 0;
        AllCharacters = new List<CharacterUnitData>();
    }
}
