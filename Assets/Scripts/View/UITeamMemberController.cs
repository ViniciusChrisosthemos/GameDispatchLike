using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITeamMemberController : MonoBehaviour
{
    [SerializeField] private Image _imgCharacter;

    private CharacterUnit _characterUnit;

    public void AddCharacter(CharacterUnit character)
    {
        _characterUnit = character;

        _imgCharacter.sprite = character.FaceArt;
    }

    public void RmvCharacter()
    {
        _imgCharacter.sprite = null;
        _characterUnit = null;
    }

    public CharacterUnit CharacterUnit => _characterUnit;
}
