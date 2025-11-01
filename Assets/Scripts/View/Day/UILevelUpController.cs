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
    [SerializeField] private Transform _statButtonParent;
    [SerializeField] private UIStatLevelUpViewController _uiStatLevelUpViewControllerPrefab;

    [Header("Right Side")]
    [SerializeField] private UIRadarChartController _uiRadarChartController;

    [Header("Parameters")]
    [SerializeField] private List<StatInfoSO> _statInfos;

    private CharacterUnit _characterUnit;
    private List<UIStatLevelUpViewController> _statControllers;

    private void Start()
    {
        _btnCloseScreen.onClick.AddListener(CloseScreen);

        _view.SetActive(false);
    }

    public void OpenScreen(CharacterUnit characterUnit)
    {
        _view.SetActive(true);

        _characterUnit = characterUnit;

        _imgCharacter.sprite = characterUnit.FullArt;
        _txtCharacterName.text = characterUnit.Name;
        _txtCharacterLevel.text = $"{STRING_LEVEL}{characterUnit.Level}";

        _statButtonParent.ClearChilds();
        _statControllers = new List<UIStatLevelUpViewController>();

        foreach (StatType type in Enum.GetValues(typeof(StatType)))
        {
            var stat = characterUnit.StatManager.GetStat(type);
            var statInfo = _statInfos.Find(s => s.Type == type);

            var controller = Instantiate(_uiStatLevelUpViewControllerPrefab, _statButtonParent);
            controller.Init(statInfo, stat, HandleDecreaseStat, HandleIncreaseStat);
        }

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

        foreach (var controller in _statControllers)
        {
            controller.SetBTNIncrease(_characterUnit.AvailablePoints == 0);
            controller.SetStatAmount(controller.Stat.GetValue());
        }

        _uiRadarChartController.UpdateStats(_characterUnit.StatManager.GetValues());
    }
}
