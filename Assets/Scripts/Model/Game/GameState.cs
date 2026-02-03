using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameState
{
    private string _saveFile;
    private Company _company;
    private int _currentDay;
    private ContractManager _contractManager;

    public GameState(string saveFile, int day, Company company, List<ContractMissionRuntime> availableContracts, List<ContractMissionRuntime> ongoingContracts)
    {
        _saveFile = saveFile;
        _company = company;
        _currentDay = day;
        _contractManager = new ContractManager(availableContracts, ongoingContracts);
    }

    public void IncrementDay()
    {
        _currentDay++;

        _contractManager.UpdateContracts(_currentDay);
    }

    public int Day => _currentDay;

    public Company Company => _company;

    public string SaveFile => _saveFile;
    
    public ContractManager ContractManager => _contractManager;
}
