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
    public List<CharacterUnitData> HiredCharacters;
    public List<string> ScheduledCharacters;

    public GuildData(Guild guild)
    {
        Name = guild.Name;
        Balance = guild.Balance;
        Reputation = guild.Reputation;
        HiredCharacters = guild.HiredCharacters.Select(c => new CharacterUnitData(c)).ToList();
        ScheduledCharacters = guild.ScheduledCharacters.Select(c => c.BaseCharacterSO.ID).ToList();
    }

    public GuildData()
    {
        Name = "ERROR";
        Balance = 0;
        Reputation = 0;
        HiredCharacters = new List<CharacterUnitData>();
        ScheduledCharacters = new List<string>();
    }
}
