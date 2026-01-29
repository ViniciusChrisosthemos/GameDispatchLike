using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

[Serializable]
public class GameStateData
{
    public string SaveFile;
    public int CurrentDay;
    public GuildData GuildData;
    public List<CharacterUnitData> Candidates;

    public GameStateData(GameState gameState)
    {
        SaveFile = gameState.SaveFile;
        CurrentDay = gameState.Day;
        GuildData = new GuildData(gameState.Company);
        Candidates = gameState.Candidates.ConvertAll(c => new CharacterUnitData(c));
    }

    public GameStateData()
    {
        SaveFile = "defaultSaveFile.json";
        CurrentDay = 0;
        GuildData = new GuildData();
        Candidates = new List<CharacterUnitData>();
    }
}
