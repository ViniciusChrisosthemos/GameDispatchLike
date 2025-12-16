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
    [SerializeField] private ScreenConfigurationSO _screenConfigurationSO;

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

        SoundManager.Instance.PlayMusic(_screenConfigurationSO.MusicBackground, _screenConfigurationSO.MusicVolume, _screenConfigurationSO.InLoop);
    }

    public void HandleScheduledCharacterSelected(UIItemController controller)
    {
        Debug.Log($"Scheduled Character {controller.GetItem<CharacterUnit>().Name}");
        ChangeCharacterInLists(controller, _scheduledCharactersListDisplay, _availableCharactersListDisplay, false);
    }

    public void HandleAvailableCharacterSelected(UIItemController controller)
    {
        if (_scheduledCharactersListDisplay.Count >= _gameSettingsSO.MaxScheduledCharacters) return;

        Debug.Log($"Avilable Character {controller.GetItem<CharacterUnit>().Name}");
        ChangeCharacterInLists(controller, _availableCharactersListDisplay, _scheduledCharactersListDisplay, true);
    }

    private void ChangeCharacterInLists(UIItemController controller, UIListDisplay originList, UIListDisplay destinyList, bool isCharacterScheduled)
    {
        var item = controller.GetItem<CharacterUnit>();

        originList.RmvItem(item);
        destinyList.AddItem(item);

        _guild.SetScheduledCharacter(item, isCharacterScheduled);
    }
}