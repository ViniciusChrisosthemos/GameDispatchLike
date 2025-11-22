using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static StatManager;

public class UILevelUpController : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private Button _btnCloseScreen;

    [Header("Left Side")]
    [SerializeField] private Image _imgCharacter;
    [SerializeField] private Image _imgCharacterBackground;
    [SerializeField] private CharacterArtType _characterArtType;
    [SerializeField] private TextMeshProUGUI _txtCharacterName;
    [SerializeField] private TextMeshProUGUI _txtAvailablePoints;
    [SerializeField] private List<UIStatLevelUpViewController> _uiStatLevelUpViewControllers;

    [Header("Left Side/Hero Background")]
    [SerializeField] private Image _imgRankBackground;
    [SerializeField] private TextMeshProUGUI _txtRandType;
    [SerializeField] private Image _imgHeroBackground;

    [Header("Right Side")]
    [SerializeField] private UIRadarChartController _uiRadarChartController;

    private CharacterUnit _characterUnit;
    private Action _callback;

    private void Start()
    {
        _btnCloseScreen.onClick.AddListener(CloseScreen);

        _view.SetActive(false);

        foreach (var controller in _uiStatLevelUpViewControllers)
        {
            controller.Init(HandleDecreaseStat, HandleIncreaseStat);
        }
    }

    public void OpenScreen(CharacterUnit characterUnit, Action callback)
    {
        _view.SetActive(true);

        _characterUnit = characterUnit;

        _imgCharacterBackground.sprite = characterUnit.GetArt(_characterArtType);
        _imgCharacter.sprite = _imgCharacterBackground.sprite;
        _txtCharacterName.text = $"{characterUnit.Name} <color=yellow>Lvl.{characterUnit.Level}</color>";

        _imgRankBackground.color = characterUnit.Rank.BackgroundColor;
        _txtRandType.text = characterUnit.Rank.Description;
        _imgHeroBackground.color = characterUnit.HeroBackgroundColor;

        _callback = callback;

        UpdateScreen();
    }

    private void HandleIncreaseStat(StatType statType)
    {
        _characterUnit.AddPointInStat(statType);

        UpdateScreen();
    }

    private void HandleDecreaseStat(StatType statType)
    {

    }

    public void CloseScreen()
    {
        _view.SetActive(false);
        _callback?.Invoke();
    }
    private void UpdateScreen()
    {
        _txtAvailablePoints.text = _characterUnit.AvailablePoints.ToString();

        foreach (var controller in _uiStatLevelUpViewControllers)
        {
            var statValue = _characterUnit.StatManager.GetStat(controller.Type).GetValue();
            controller.UpdateStatInfo(statValue, _characterUnit.AvailablePoints != 0);
        }

        _uiRadarChartController.UpdateStats(_characterUnit.StatManager.GetValues());
    }
}
