using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnit
{
    public enum CharacterStatus { Available, InMission, Resting  } 

    private CharacterSO _baseCharacter;
    private StatManager _statManager;
    private CharacterStatus _status;

    public CharacterUnit(CharacterSO baseCharacter)
    {
        _baseCharacter = baseCharacter;

        _status = CharacterStatus.Available;
        _statManager = baseCharacter.BaseStats;
    }

    public string Name => _baseCharacter.Name;
    public Sprite Sprite => _baseCharacter.Sprite;

    public StatManager StatManager { get { return _statManager; } }

    public void SetStatusToAvailable()
    {
        _status = CharacterStatus.Available;
    }

    public void SetStatusToInMission()
    {
        _status = CharacterStatus.InMission;
    }

    public void SetStatusToResting()
    {
        _status = CharacterStatus.Resting;
    }

    public CharacterStatus Status { get { return _status; } }
}
