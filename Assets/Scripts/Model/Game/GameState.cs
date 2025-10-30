using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public string _saveFile;
    private Guild _guild;
    private int _currentDay;

    public GameState(string saveFile, int day, Guild guild)
    {
        _saveFile = saveFile;
        _guild = guild;
        _currentDay = day;
    }

    public void IncrementDay()
    {
        _currentDay++;
    }

    public int Day => _currentDay;

    public Guild Guild => _guild;

    public string SaveFile => _saveFile;
}
