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
    private const string STATUS_MOVING = "Moving";
    private const string STATUS_RETURNING = "Returning";

    [SerializeField] private UICharacterViewController _uiCharacterViewController;

    [SerializeField] private GameObject _statusView;
    [SerializeField] private TextMeshProUGUI _txtStatus;
    [SerializeField] private Button _btnCharacter;
    [SerializeField] private GameObject _unavailableOverlay;
    [SerializeField] private Slider _sliderResting;
    [SerializeField] private Image _imgStatus;

    [Header("Level")]
    [SerializeField] private Image _imgLevelSlider;
    [SerializeField] private GameObject _levelUpOverlay;
    [SerializeField] private TextMeshProUGUI _txtAvailablePoints;
    [SerializeField] private Button _btnLevelUp;

    [Header("Status")]
    [SerializeField] private Color _colorMovingStatus;
    [SerializeField] private Color _colorBusyStatus;
    [SerializeField] private Color _colorReturningStatus;

    private CharacterUnit _characterUnit;
    private Action<CharacterUnit> _onSelected;

    private UIDayManager _manager;

    private void Start()
    {
        SetStatusAvailable();
    }

    public void Init(UIDayManager manager, CharacterUnit characterUnit, Action<CharacterUnit> onSelected)
    {
        _manager = manager;
        _characterUnit = characterUnit;
        _onSelected = onSelected;

        _uiCharacterViewController.SetItem(characterUnit);

        _btnCharacter.onClick.RemoveAllListeners();
        _btnCharacter.onClick.AddListener(HandleCharacterSelected);

        _characterUnit.OnCharacterGoingToMission.AddListener(_ => SetStatusInMoving());
        _characterUnit.OnCharacterReturning.AddListener(_ => SetCharacterReturning());
        _characterUnit.OnCharacterInMission.AddListener(_ => SetStatusInMission());
        _characterUnit.OnCharacterInResting.AddListener(_ => SetStatusInResting());
        _characterUnit.OnCharacterInAvailable.AddListener(_ => SetStatusAvailable());
        _characterUnit.OnCharacterLevelUP.AddListener(HandleStatChangedEvent);
        _characterUnit.OnCharacterEXPChanged.AddListener(HandleExpChangedEvent);
        _characterUnit.OnCharacterStatChanged.AddListener(HandleStatChangedEvent);

        HandleExpChangedEvent(characterUnit);
        HandleStatChangedEvent(characterUnit);

        _btnLevelUp.onClick.AddListener(HandleBTNLevelUPEvent);
    }

    public void UpdateTime(float currentTime)
    {
        if (!_characterUnit.IsResting()) return;

        _sliderResting.value = 1 - _characterUnit.GetNormalizedTimeToBeAvailable(currentTime);
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
        _statusView.SetActive(true);

        _imgStatus.color = _colorBusyStatus;
    }

    public void SetStatusAvailable()
    {
        _txtStatus.text = STATUS_AVAILABLE;

        _unavailableOverlay.SetActive(false);
        _sliderResting.value = 0f;
        _statusView.SetActive(false);
        _sliderResting.gameObject.SetActive(false);
    }

    public void SetStatusInResting()
    {
        _txtStatus.text = STATUS_REST;

        _unavailableOverlay.SetActive(true);
        _statusView.SetActive(true);

        _sliderResting.gameObject.SetActive(true);
    }

    private void SetStatusInMoving()
    {
        _txtStatus.text = STATUS_MOVING;

        _unavailableOverlay.SetActive(true);
        _statusView.SetActive(true);

        _imgStatus.color = _colorMovingStatus;
    }

    private void SetCharacterReturning()
    {
        _txtStatus.text = STATUS_RETURNING;

        _unavailableOverlay.SetActive(true);
        _statusView.SetActive(true);

        _imgStatus.color = _colorReturningStatus;
    }

}