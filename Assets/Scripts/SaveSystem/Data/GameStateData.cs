using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameStateData
{
    public string SaveFile;
    public int CurrentDay;
    public GuildData GuildData;

    public GameStateData(GameState gameState)
    {
        SaveFile = gameState.SaveFile;
        CurrentDay = gameState.Day;
        GuildData = new GuildData(gameState.Guild);
    }

    public GameStateData()
    {
        SaveFile = "defaultSaveFile.json";
        CurrentDay = 0;
        GuildData = new GuildData();
    }
}
