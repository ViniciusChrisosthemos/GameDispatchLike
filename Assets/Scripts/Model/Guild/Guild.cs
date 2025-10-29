using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guild
{
    private string _name;
    private int _balance;
    private int _reputation;
    private List<CharacterUnit> _hiredCharacters;
    private List<CharacterUnit> _scheduledCharacters;

    public Guild(string name, int balance, int popularity, List<CharacterUnit> characters)
    {
        _name = name;
        _balance = balance;
        _reputation = popularity;
        _hiredCharacters = characters;
        _scheduledCharacters = new List<CharacterUnit>();
    }

    public void SetScheduledCharacters(List<CharacterUnit> scheduledCharacters)
    {
        _scheduledCharacters = scheduledCharacters;
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
    public List<CharacterUnit> HiredCharacters => _hiredCharacters;
    public List<CharacterUnit> ScheduledCharacters => _scheduledCharacters;
}
