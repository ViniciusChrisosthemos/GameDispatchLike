using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimelineChracterView : UIItemController
{
    [SerializeField] private Image _imgCharacterFace;

    [SerializeField] private GameObject _unavailableOverlay;
    [SerializeField] private GameObject _selectedOverlay;

    private void Awake()
    {
        Clear();
    }

    public void SetUnavailable()
    {
        _unavailableOverlay.SetActive(true);
        _selectedOverlay.SetActive(false);
    }

    public void SetSelected()
    {
        _unavailableOverlay.SetActive(false);
        _selectedOverlay.SetActive(true);
    }

    protected override void HandleInit(object obj)
    {
        var characterData = obj as BattleCharacter;

        _imgCharacterFace.sprite = characterData.BaseCharacter.FaceArt;
    }

    public void Clear()
    {
        _unavailableOverlay.SetActive(false);
        _selectedOverlay.SetActive(false);
    }
}
