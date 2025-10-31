using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITeamMemberController : UIItemController
{
    [SerializeField] private Image _imgCharacter;
    [SerializeField] private Button _btnCLick;

    private void Awake()
    {
        _btnCLick.onClick.AddListener(SelectItem);
    }

    public void AddCharacter(CharacterUnit character)
    {
        _item = character;

        _imgCharacter.sprite = character.FaceArt;
    }

    public void RmvCharacter()
    {
        _imgCharacter.sprite = null;
        _item = null;
    }

    protected override void HandleInit(object obj)
    {
        _item = obj as CharacterUnit;

        RmvCharacter();
    }

}
