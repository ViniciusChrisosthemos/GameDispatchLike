using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroPathIconController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteHero;

    public void Init(Team team)
    {
        _spriteHero.sprite = team.Members[0].FaceArt;
    }
}
