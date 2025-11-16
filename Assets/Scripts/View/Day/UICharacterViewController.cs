
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterViewController : UIItemController
{
    public enum CharacterArtType { Face, Body, Full }

    [SerializeField] private Image _imgCharacterView;
    [SerializeField] private CharacterArtType _characterArtType;

    [Header("(optional)")]
    [SerializeField] private UIRadarChartController _radarChartStatController;
    [SerializeField] private Button _btnButton;
    [SerializeField] private TextMeshProUGUI _txtName;

    private CharacterUnit _characterUnit;

    protected override void HandleInit(object obj)
    {
        _characterUnit = obj as CharacterUnit;

        switch (_characterArtType)
        {
            case CharacterArtType.Face: _imgCharacterView.sprite = _characterUnit.FaceArt; break;
            case CharacterArtType.Body: _imgCharacterView.sprite = _characterUnit.BodyArt; break;
            case CharacterArtType.Full: _imgCharacterView.sprite = _characterUnit.FullArt; break;
            default: _imgCharacterView.sprite = null; break;
        }

        if (_txtName != null)
        {
            _txtName.text = _characterUnit.Name;
        }

        if (_radarChartStatController != null)
        {
            var values = _characterUnit.StatManager.GetValues();
            _radarChartStatController.UpdateStats(values);
        }
    }

    public CharacterUnit CharacterUnit => _characterUnit;
}
