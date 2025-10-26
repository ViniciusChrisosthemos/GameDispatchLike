using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDayCharacterViewController : MonoBehaviour
{
    private const string STATUS_BUSY = "Busy";
    private const string STATUS_REST = "Resting";
    private const string STATUS_AVAILABLE = "Available";

    [SerializeField] private UICharacterViewController _uiCharacterViewController;

    [SerializeField] private GameObject _statusView;
    [SerializeField] private TextMeshProUGUI _txtStatus;
    [SerializeField] private Button _btnCharacter;
    [SerializeField] private GameObject _unavailableOverlay;

    [Header("Level")]
    [SerializeField] private Image _imgLevelSlider;
    [SerializeField] private GameObject _levelUpOverlay;
    [SerializeField] private TextMeshProUGUI _txtAvailablePoints;
    [SerializeField] private Button _btnLevelUp;

    private CharacterUnit _characterUnit;
    private Action<CharacterUnit> _onSelected;

    private UIDayManager _manager;

    private void Start()
    {
        _levelUpOverlay.SetActive(false);

        SetStatusAvailable();
    }

    public void Init(UIDayManager manager, CharacterUnit characterUnit, Action<CharacterUnit> onSelected)
    {
        _manager = manager;
        _characterUnit = characterUnit;
        _onSelected = onSelected;

        _uiCharacterViewController.UpdateCharacter(characterUnit);

        _btnCharacter.onClick.AddListener(HandleCharacterSelected);

        _characterUnit.OnCharacterInMission.AddListener(_ => SetStatusInMission());
        _characterUnit.OnCharacterInResting.AddListener(_ => SetStatusInResting());
        _characterUnit.OnCharacterInAvailable.AddListener(_ => SetStatusAvailable());
        _characterUnit.OnCharacterLevelUP.AddListener(HandleStatChangedEvent);
        _characterUnit.OnCharacterEXPChanged.AddListener(HandleExpChangedEvent);
        _characterUnit.OnCharacterStatChanged.AddListener(HandleStatChangedEvent);

        _btnLevelUp.onClick.AddListener(HandleBTNLevelUPEvent);
    }

    private void HandleStatChangedEvent(CharacterUnit character)
    {
        _levelUpOverlay.SetActive(character.AvailablePoints != 0);
        _txtAvailablePoints.text = $"+{character.AvailablePoints}";
    }

    private void HandleBTNLevelUPEvent()
    {
        _manager.OpenLevelUpScreen(_characterUnit);
    }

    private void HandleExpChangedEvent(CharacterUnit character)
    {
        _imgLevelSlider.fillAmount = character.NormalizedExp;
    }

    private void HandleCharacterSelected()
    {
        if (!_characterUnit.IsAvailable()) return;

        _onSelected?.Invoke(_characterUnit);
    }

    public void SetStatusInMission()
    {
        _txtStatus.text = STATUS_BUSY;

        _unavailableOverlay.SetActive(true);
    }

    public void SetStatusAvailable()
    {
        _txtStatus.text = STATUS_AVAILABLE;

        _unavailableOverlay.SetActive(false);
    }

    public void SetStatusInResting()
    {
        _txtStatus.text = STATUS_REST;

        _unavailableOverlay.SetActive(true);
    }
}
