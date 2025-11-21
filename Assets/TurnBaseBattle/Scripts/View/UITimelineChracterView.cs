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
    [SerializeField] private GameObject _inativeOverlay;

    private void Awake()
    {
        Clear();
    }

    public void SetUnavailable()
    {
        _unavailableOverlay.SetActive(true);
        _selectedOverlay.SetActive(false);
        _inativeOverlay.SetActive(false);
    }

    public void SetInative()
    {
        _unavailableOverlay.SetActive(false);
        _selectedOverlay.SetActive(false);
        _inativeOverlay.SetActive(true);
    }

    public void SetSelected()
    {
        _unavailableOverlay.SetActive(false);
        _selectedOverlay.SetActive(true);
        _inativeOverlay.SetActive(false);
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
        _inativeOverlay.SetActive(false);
    }
}
