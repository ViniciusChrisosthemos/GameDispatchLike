using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Guild
{
    private string _name;
    private int _balance;
    private int _reputation;
    private List<CharacterUnit> _allCharacters;

    public Guild(string name, int balance, int popularity, List<CharacterUnit> characters)
    {
        _name = name;
        _balance = balance;
        _reputation = popularity;
        _allCharacters = characters;
    }

    public void SetScheduledCharacter(CharacterUnit character, bool isScheduled)
    {
        character.SetScheduledCharater(isScheduled);
    }

    public void AddGold(int gold)
    {
        _balance += gold;
    }

    public void AddReputation(int reputation)
    {
        _reputation += reputation;
    }

    public void RmvReputation(int reputation)
    {
        _reputation -= reputation;
    }

    public string Name => _name;
    public int Balance => _balance;
    public int Reputation => _reputation;
    public List<CharacterUnit> AllCharacters => _allCharacters;
    public List<CharacterUnit> AvailableCharacters => _allCharacters.Where(c => !c.IsScheduled).ToList();
    public List<CharacterUnit> ScheduledCharacters => _allCharacters.Where(c => c.IsScheduled).ToList();
}
