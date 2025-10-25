using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITeamMemberController : MonoBehaviour
{
    [SerializeField] private Image _imgCharacter;
    [SerializeField] private Button _btnRemoveCharacter;

    private CharacterUnit _characterUnit;

    public void Init(Action<CharacterUnit> onRemoved)
    {
        _btnRemoveCharacter.onClick.AddListener(() =>
        {
            _imgCharacter.sprite = null;
            onRemoved?.Invoke(_characterUnit);
        });
    }

    public void AddCharacter(CharacterUnit character)
    {
        _characterUnit = character;

        _imgCharacter.sprite = character.Sprite;
    }

    public CharacterUnit CharacterUnit => _characterUnit;
}
