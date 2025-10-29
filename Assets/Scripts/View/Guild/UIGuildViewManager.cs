using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildViewManager : MonoBehaviour
{
    [Header("Day References")]
    [SerializeField] private DayManager _dayManager;

    [Header("Screen References")]
    [SerializeField] private GameObject _view;

    [Header("Guild")]
    [SerializeField] private TextMeshProUGUI _txtGuildName;
    [SerializeField] private TextMeshProUGUI _txtBalance;
    [SerializeField] private TextMeshProUGUI _txtPopularity;

    [Header("Day")]
    [SerializeField] private TextMeshProUGUI _txtDay;
    [SerializeField] private Button _btnStartDay;

    [Header("Characters")]
    [SerializeField] private Transform _scheduledCharactersParent; 
    [SerializeField] private Transform _availableCharactersParent;
    [SerializeField] private UICharacterViewController _characterViewControllerPrefab;


    [Header("Settings")]
    [SerializeField] private GameSettingsSO _gameSettingsSO;

    private void Awake()
    {
        _btnStartDay.onClick.AddListener(StartDay);
    }

    private void Start()
    {
        OpenScreen();
    }

    public void StartDay()
    {
        CommitChanges();

        var guild = GameManager.Instance.GameState.Guild;

        _dayManager.StartDay(guild.ScheduledCharacters);

        CloseWindow();
    }

    public void Init()
    {
        var gameState = GameManager.Instance.GameState;

        _txtGuildName.text = gameState.Guild.Name;
        _txtBalance.text = gameState.Guild.Balance.ToString();
        _txtPopularity.text = gameState.Guild.Reputation.ToString();

        _txtDay.text = gameState.Day.ToString();

        _scheduledCharactersParent.ClearChilds();
        _availableCharactersParent.ClearChilds();

        foreach (var character in gameState.Guild.ScheduledCharacters)
        {
            var controller = Instantiate(_characterViewControllerPrefab, _scheduledCharactersParent);
            controller.UpdateCharacter(character, HandleScheduledCharacterSelected);
        }

        foreach (var character in gameState.Guild.HiredCharacters)
        {
            if (gameState.Guild.ScheduledCharacters.Contains(character)) continue;

            var controller = Instantiate(_characterViewControllerPrefab, _availableCharactersParent);
            controller.UpdateCharacter(character, HandleAvailableCharacterSelected);
        }
    }

    public void HandleScheduledCharacterSelected(UICharacterViewController controller)
    {
        controller.transform.SetParent(_availableCharactersParent, false);
    }

    public void HandleAvailableCharacterSelected(UICharacterViewController controller)
    {
        if (_scheduledCharactersParent.childCount >= _gameSettingsSO.MaxScheduledCharacters) return;

        controller.transform.SetParent(_scheduledCharactersParent, false);
    }

    public void CommitChanges()
    {
        var scheduledCharacters = _scheduledCharactersParent.GetComponentsInChildren<UICharacterViewController>().Select(c => c.CharacterUnit).ToList();

        var guild = GameManager.Instance.GameState.Guild;

        guild.ScheduledCharacters.Clear();
        guild.SetScheduledCharacters(scheduledCharacters);
    }

    public void CloseWindow()
    {
        _view.SetActive(false);
    }

    public void OpenScreen()
    {
        _view.SetActive(true);

        Init();
    }
}
