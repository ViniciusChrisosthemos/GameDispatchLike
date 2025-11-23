using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroPathIconController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteHero;
    [SerializeField] private SpriteRenderer _spriteBackground;
    [SerializeField] private TrailRenderer _trailRendererPath;
    [SerializeField] private CharacterArtType _characterArtType;

    public void Init(Team team)
    {
        _spriteHero.sprite = team.Members[0].GetArt(_characterArtType);
        _spriteBackground.color = team.Members[0].HeroBackgroundColor;
        _trailRendererPath.startColor = team.Members[0].HeroBackgroundColor;
    }
}
