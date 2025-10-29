using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameStateData
{
    public int CurrentDay;
    public GuildData GuildData;

    public GameStateData(GameState gameState)
    {
        Debug.Log("GameStateData");

        CurrentDay = gameState.Day;
        GuildData = new GuildData(gameState.Guild);
    }

    public GameStateData()
    {
        CurrentDay = 0;
        GuildData = new GuildData();
    }
}
