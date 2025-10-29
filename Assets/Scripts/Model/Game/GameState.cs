using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    private Guild _guild;
    private int _currentDay;

    public GameState(int day, Guild guild)
    {
        _guild = guild;
        _currentDay = day;
    }

    public void IncrementDay()
    {
        _currentDay++;
    }

    public int Day => _currentDay;

    public Guild Guild => _guild;
}
