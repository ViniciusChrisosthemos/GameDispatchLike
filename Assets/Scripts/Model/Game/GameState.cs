using NUnit.Framework;
using System;
using System.Collections.Generic;

public class GameState
{
    public string _saveFile;
    private Company _company;
    private int _currentDay;
    private List<CharacterUnit> _candidates;

    public GameState(string saveFile, int day, Company company, List<CharacterUnit> condidates)
    {
        _saveFile = saveFile;
        _company = company;
        _currentDay = day;
        _candidates = condidates;
    }

    public void IncrementDay()
    {
        _currentDay++;
    }

    public int Day => _currentDay;

    public Company Company => _company;

    public string SaveFile => _saveFile;

    public List<CharacterUnit> Candidates => _candidates;
}
