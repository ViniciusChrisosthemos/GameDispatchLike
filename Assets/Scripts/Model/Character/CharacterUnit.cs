using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnit
{
    private CharacterSO _baseCharacter;
    private StatManager _statManager;

    public CharacterUnit(CharacterSO baseCharacter)
    {
        _baseCharacter = baseCharacter;

        _statManager = baseCharacter.BaseStats;
    }

    public string Name => _baseCharacter.Name;
    public Sprite Sprite => _baseCharacter.Sprite;

    public StatManager StatManager { get { return _statManager; } }
}
