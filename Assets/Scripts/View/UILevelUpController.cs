using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevelUpController : MonoBehaviour
{
    private const string LABEL_AVAILABLE_POINTS = "Available points: {0}";

    [SerializeField] private GameObject _view;
    [SerializeField] private UICharacterViewController _uiCharacterViewController;
    [SerializeField] private TextMeshProUGUI _txtAvailablePoints;
    [SerializeField] private GameObject _buttonsView;
    [SerializeField] private Button _btnCloseScreen;

    [Header("Buttons")]
    [SerializeField] private Button _btnStrengh;
    [SerializeField] private Button _btnEndurance;
    [SerializeField] private Button _btnAgility;
    [SerializeField] private Button _btnCharisma;
    [SerializeField] private Button _btnIntelligence;

    private CharacterUnit _characterUnit;

    private void Start()
    {
        _btnStrengh.onClick.AddListener(() => AddBonusToStat(StatManager.StatType.Strengh));
        _btnEndurance.onClick.AddListener(() => AddBonusToStat(StatManager.StatType.Endurance));
        _btnAgility.onClick.AddListener(() => AddBonusToStat(StatManager.StatType.Agility));
        _btnCharisma.onClick.AddListener(() => AddBonusToStat(StatManager.StatType.Charisma));
        _btnIntelligence.onClick.AddListener(() => AddBonusToStat(StatManager.StatType.Intelligence));

        _btnCloseScreen.onClick.AddListener(CloseScreen);

        _view.SetActive(false);
    }

    public void OpenScreen(CharacterUnit characterUnit)
    {
        _view.SetActive(true);

        _characterUnit = characterUnit;

        UpdateScreen();
    }

    public void CloseScreen()
    {
        _view.SetActive(false);
    }

    private void AddBonusToStat(StatManager.StatType stat)
    {
        _characterUnit.AddPointInStat(stat);

        UpdateScreen();
    }

    private void UpdateScreen()
    {
        _buttonsView.SetActive(_characterUnit.AvailablePoints != 0);
        _uiCharacterViewController.UpdateCharacter(_characterUnit);
        _txtAvailablePoints.text = string.Format(LABEL_AVAILABLE_POINTS, _characterUnit.AvailablePoints);
    }
}
