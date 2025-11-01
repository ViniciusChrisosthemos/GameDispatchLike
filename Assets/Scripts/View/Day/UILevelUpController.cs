using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static StatManager;

public class UILevelUpController : MonoBehaviour
{
    private readonly string STRING_LEVEL = "Lvl. ";

    [SerializeField] private GameObject _view;
    [SerializeField] private Button _btnCloseScreen;

    [Header("Left Side")]
    [SerializeField] private Image _imgCharacter;
    [SerializeField] private TextMeshProUGUI _txtCharacterName;
    [SerializeField] private TextMeshProUGUI _txtCharacterLevel;
    [SerializeField] private TextMeshProUGUI _txtAvailablePoints;
    [SerializeField] private List<UIStatLevelUpViewController> _uiStatLevelUpViewControllers;

    [Header("Right Side")]
    [SerializeField] private UIRadarChartController _uiRadarChartController;

    private CharacterUnit _characterUnit;

    private void Start()
    {
        _btnCloseScreen.onClick.AddListener(CloseScreen);

        _view.SetActive(false);

        foreach (var controller in _uiStatLevelUpViewControllers)
        {
            controller.Init(HandleDecreaseStat, HandleIncreaseStat);
        }
    }

    public void OpenScreen(CharacterUnit characterUnit)
    {
        _view.SetActive(true);

        _characterUnit = characterUnit;

        _imgCharacter.sprite = characterUnit.FullArt;
        _txtCharacterName.text = characterUnit.Name;
        _txtCharacterLevel.text = $"{STRING_LEVEL}{characterUnit.Level}";

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
