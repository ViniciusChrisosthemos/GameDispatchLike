using System;

public class GameState
{
    public string _saveFile;
    private Company _company;
    private int _currentDay;

    public GameState(string saveFile, int day, Company company)
    {
        _saveFile = saveFile;
        _company = company;
        _currentDay = day;
    }

    public void IncrementDay()
    {
        _currentDay++;
    }

    public int Day => _currentDay;

    public Company Company => _company;

    public string SaveFile => _saveFile;
}
