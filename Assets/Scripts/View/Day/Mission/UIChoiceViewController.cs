using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static MissionSO;

public class UIChoiceViewController : UIItemController
{
    private const string STRING_OPTION_UNAVAILABLE = "[OPTION UNAVAILABLE] {0} required.";

    [Header("Normal Choice")]
    [SerializeField] private GameObject _normalChoiceView;
    [SerializeField] private UIRequirementTextViewController _uiRequirementTextViewController;

    [Header("Character Choice")]
    [SerializeField] private GameObject _characterChoiceView;
    [SerializeField] private TextMeshProUGUI _txtCharacterChoiceDescription;
    [SerializeField] private Image _imgCharacterRequired;

    [Header("All")]
    [SerializeField] private GameObject _unavailableOverlay;
    [SerializeField] private Button _btnClick;
    [SerializeField] private GameSettingsSO _gameSettingsSO;

    private MissionChoice _missionChoice;

    private void Start()
    {
        _btnClick.onClick.AddListener(() => SelectItem());
    }

    protected override void HandleInit(object obj)
    {
        _missionChoice = obj as MissionChoice;

        if (_missionChoice.Character == null)
        {
            _normalChoiceView.SetActive(true);
            _characterChoiceView.SetActive(false);

            _uiRequirementTextViewController.SetText(_missionChoice.Requirement, _gameSettingsSO.RequirementStringColor);
        }
        else
        {
            _normalChoiceView.SetActive(false);
            _characterChoiceView.SetActive(true);

            _txtCharacterChoiceDescription.text = _missionChoice.Requirement.Description;
            _imgCharacterRequired.sprite = _missionChoice.Character.FaceArt;
        }

        SetUnavailableOverlay(false);
    }

    public void SetUnavailableOverlay(bool isUnavailable)
    {
        _btnClick.interactable = !isUnavailable;
        _unavailableOverlay.SetActive(isUnavailable);
    }

    public void SetOptionLocked(string characterName)
    {
        _normalChoiceView.SetActive(true);
        _characterChoiceView.SetActive(false);

        _unavailableOverlay.SetActive(true);

        _uiRequirementTextViewController.SetText(string.Format(STRING_OPTION_UNAVAILABLE, characterName));
    }

    public void ShowStats(bool showStats)
    {
        if (_missionChoice.Character == null)
        {
            _uiRequirementTextViewController.SetStatView(showStats);
        }
    }
}
