using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyView : UIItemController
{
    [SerializeField] private Image _imgEnemyArt;
    [SerializeField] private CharacterArtType _artType;

    protected override void HandleInit(object obj)
    {
        var character = obj as CharacterUnit;
        Sprite sprite;

        switch (_artType)
        {
            case CharacterArtType.Face: sprite = character.FaceArt; break;
            case CharacterArtType.Body: sprite = character.BodyArt; break;
            case CharacterArtType.FullBody: sprite = character.FullArt; break;
            default: sprite = character.FullArt; break;
        }

        _imgEnemyArt.sprite = sprite;
    }
}
