using UnityEngine;
using UnityEngine.UI;

public class UICharacterArtView : UIItemController
{
    [SerializeField] private Image _imgCharacterArt;
    [SerializeField] private Image _imgBackground;
    [SerializeField] private CharacterArtType _characterArtType;

    protected override void HandleInit(object obj)
    {
        var character = obj as CharacterSO;

        _imgBackground.color = character.ColorBackground;
        _imgCharacterArt.sprite = character.GetArt(_characterArtType);
    }
}
