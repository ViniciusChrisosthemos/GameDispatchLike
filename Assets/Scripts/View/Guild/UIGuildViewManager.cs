using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class UIGuildViewManager : MonoBehaviour
{
    [Header("Screen References")]
    [SerializeField] private GameObject _view;
    [SerializeField] private Button _btnQuit;

    [Header("Guild")]
    [SerializeField] private TextMeshProUGUI _txtPlayerName;
    [SerializeField] private TextMeshProUGUI _txtLevelLabel;
    [SerializeField] private Slider _sliderExp;

    [Header("Day")]
    [SerializeField] private TextMeshProUGUI _txtDay;
    [SerializeField] private Button _btnStartDay;

    [Header("Characters")]
    [SerializeField] private UIListDisplay _scheduledCharactersListDisplay;
    [SerializeField] private UIListDisplay _availableCharactersListDisplay;

    [Header("Screens")]
    [SerializeField] private UICalendarView _uiCalendarView;

    [Header("Settings")]
    [SerializeField] private GameSettingsSO _gameSettingsSO;

    [Header("Events")]
    public UnityEvent OnScreenOpened;

    private Guild _guild;

    private void Start()
    {
        _btnStartDay.onClick.AddListener(StartDay);
        _btnQuit.onClick.AddListener(QuitGame);

        Init();
    }

    public void StartDay()
    {
        CustomSceneManager.Instance.LoadGameScene();
    }

    public void QuitGame()
    {
        GameManager.Instance.Quit();
    }

    public void Init()
    {
        var gameState = GameManager.Instance.GameState;
        _guild = gameState.Guild;

        _txtPlayerName.text = _guild.PlayerName;

        var levelDatabase = CharacterLevelDatabase.Instance;
        _sliderExp.value = _guild.GetCurrentExperienceNormalized(levelDatabase);
        _txtLevelLabel.text = levelDatabase.GetLevel(_guild.CurrentLevel).LevelDescription;

        _txtDay.text = gameState.Day.ToString();

        var scheduledCharacters = _guild.AllCharacters.Where(c => c.IsScheduled).Select(c => (object)c).ToList();
        var availableCharacters = _guild.AllCharacters.Where(c => !c.IsScheduled).Select(c => (object)c).ToList();

        Debug.Log($"{scheduledCharacters.Count} {availableCharacters.Count}");

        _scheduledCharactersListDisplay.SetItems(scheduledCharacters, HandleScheduledCharacterSelected);
        _availableCharactersListDisplay.SetItems(availableCharacters, HandleAvailableCharacterSelected);

        OnScreenOpened?.Invoke();

        _uiCalendarView.SetNormalDay(gameState.Day, null);
    }

    public void HandleScheduledCharacterSelected(UIItemController controller)
    {
        _scheduledCharactersListDisplay.RmvItem(controller);
        _availableCharactersListDisplay.AddItem(controller);

        controller.transform.SetParent(_availableCharactersListDisplay.Parent);
        controller.SetCallback(HandleAvailableCharacterSelected);

        _guild.SetScheduledCharacter(controller.GetItem<CharacterUnit>(), false);
    }

    public void HandleAvailableCharacterSelected(UIItemController controller)
    {
        if (_scheduledCharactersListDisplay.GetItems().Count >= _gameSettingsSO.MaxScheduledCharacters) return;

        _availableCharactersListDisplay.RmvItem(controller);
        _scheduledCharactersListDisplay.AddItem(controller);

        controller.transform.SetParent(_scheduledCharactersListDisplay.Parent);
        controller.SetCallback(HandleScheduledCharacterSelected);

        _guild.SetScheduledCharacter(controller.GetItem<CharacterUnit>(), true);
    }
}
