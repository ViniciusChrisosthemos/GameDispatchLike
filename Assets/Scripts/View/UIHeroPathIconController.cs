using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroPathIconController : MonoBehaviour
{
    [SerializeField] private Image _imgHero;

    public void Init(Team team)
    {
        _imgHero.sprite = team.Members[0].FaceArt;
    }
}
