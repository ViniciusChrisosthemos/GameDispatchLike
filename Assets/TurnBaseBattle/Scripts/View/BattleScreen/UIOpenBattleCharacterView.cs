using UnityEngine;
using UnityEngine.UI;

public class UIOpenBattleCharacterView : MonoBehaviour
{
    [SerializeField] private Image _imgCharacter;
    [SerializeField] private Image _imgCharacterBackground;
    [SerializeField] private Image _imgBackground;

    public void SetCharacter(Sprite characterArt, Color backgroundColor)
    {
        _imgCharacter.sprite = characterArt;
        _imgCharacterBackground.sprite = characterArt;
        _imgBackground.color = backgroundColor;
    }
}
