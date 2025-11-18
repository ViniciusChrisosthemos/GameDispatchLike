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

        _txtGuildName.text = _guild.Name;
        _txtBalance.text = _guild.Balance.ToString();
        _txtPopularity.text = _guild.Reputation.ToString();

        _txtDay.text = gameState.Day.ToString();

        _scheduledCharactersParent.ClearChilds();
        _availableCharactersParent.ClearChilds();

        foreach (var character in _guild.AllCharacters)
        {
            if (character.IsScheduled)
            {
                InstantiateCharacter(_scheduledCharactersParent, character, HandleScheduledCharacterSelected);
            }
            else
            {
                InstantiateCharacter(_availableCharactersParent, character, HandleAvailableCharacterSelected);
            }
        }

        OnScreenOpened?.Invoke();
    }

    private void InstantiateCharacter(Transform parent, CharacterUnit character, Action<UICharacterViewController> handler)
    {
        var controller = Instantiate(_characterViewControllerPrefab, parent);
        controller.UpdateCharacter(character, handler);
    }

    public void HandleScheduledCharacterSelected(UICharacterViewController controller)
    {
        controller.UpdateCharacter(controller.CharacterUnit, HandleAvailableCharacterSelected);
        controller.transform.SetParent(_availableCharactersParent, false);

        _guild.SetScheduledCharacter(controller.CharacterUnit, false);
    }

    public void HandleAvailableCharacterSelected(UICharacterViewController controller)
    {
        if (_scheduledCharactersParent.childCount >= _gameSettingsSO.MaxScheduledCharacters) return;

        controller.UpdateCharacter(controller.CharacterUnit, HandleScheduledCharacterSelected);
        controller.transform.SetParent(_scheduledCharactersParent, false);

        _guild.SetScheduledCharacter(controller.CharacterUnit, true);
    }
}
