
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterViewController : MonoBehaviour
{
    public enum CharacterArtType { Face, Body, Full }

    [SerializeField] private Image _imgCharacterView;
    [SerializeField] private CharacterArtType _characterArtType;

    [Header("(optional)")]
    [SerializeField] private UIRadarChartController _radarChartStatController;
    [SerializeField] private Button _btnButton;
    [SerializeField] private TextMeshProUGUI _txtName;

    private CharacterUnit _characterUnit;

    public void UpdateCharacter(CharacterUnit characterUnit)
    {
        UpdateCharacter(characterUnit, null);
    }

    public void UpdateCharacter(CharacterUnit characterUnit, Action<UICharacterViewController> callback)
    {
        _characterUnit = characterUnit;

        switch (_characterArtType)
        {
            case CharacterArtType.Face: _imgCharacterView.sprite = characterUnit.FaceArt; break;
            case CharacterArtType.Body: _imgCharacterView.sprite = characterUnit.BodyArt; break;
            case CharacterArtType.Full: _imgCharacterView.sprite = characterUnit.FullArt; break;
            default: _imgCharacterView.sprite = null; break;
        }

        if (_txtName != null)
        {
            _txtName.text = _characterUnit.Name;
        }

        if (_radarChartStatController != null)
        {
            var values = characterUnit.StatManager.GetValues();
            _radarChartStatController.UpdateStats(values);
        }

        if (callback != null && _btnButton != null)
        {
            _btnButton.onClick.RemoveAllListeners();
            _btnButton.onClick.AddListener(() => callback?.Invoke(this));
        }
    }

    public CharacterUnit CharacterUnit => _characterUnit;
}
