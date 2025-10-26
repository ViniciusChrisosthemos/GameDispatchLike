
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterViewController : MonoBehaviour
{
    public enum CharacterArtType { Face, Body, Full }

    [SerializeField] private Image _imgCharacterView;
    [SerializeField] private UIRadarChartController _radarChartStatController;
    [SerializeField] private CharacterArtType _characterArtType;

    public void UpdateCharacter(CharacterUnit characterUnit)
    {
        switch (_characterArtType)
        {
            case CharacterArtType.Face: _imgCharacterView.sprite = characterUnit.FaceArt; break;
            case CharacterArtType.Body: _imgCharacterView.sprite = characterUnit.BodyArt; break;
            case CharacterArtType.Full: _imgCharacterView.sprite = characterUnit.FullArt; break;
            default: _imgCharacterView.sprite = null; break;
        }

        var values = characterUnit.StatManager.GetValues();
        _radarChartStatController.UpdateStats(values);
    }
}
