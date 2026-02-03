using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static Company;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameStateSO DefaultGameState;

    public UnityEvent OnQuitGame;

    private GameState _gameState;

    private FactoryGameState _factoryGameState;
    private FactoryCharacterUnit _factoryCharacterUnit;

    private CharacterShopController _characterShopController;

    private void Start()
    {
        _factoryGameState = new FactoryGameState(_factoryCharacterUnit);
        _factoryCharacterUnit = new FactoryCharacterUnit(CharacterDatabase.Instance);

        InitCharacterShopController();
    }

    private void InitCharacterShopController()
    {
        var allCharacters = CharacterDatabase.Instance.AllCharacters;
        var playerCharacters = _gameState.Company.AllCharacters.Select(characterUnit => characterUnit.BaseCharacterSO).ToList();

        _characterShopController = new CharacterShopController(allCharacters, playerCharacters);
    }

    private void LoadData(string saveFile)
    {
        Debug.Log($"[{GetType()}][LoadData] Loading GameData ...");

        try
        {
            var gameStateData = SaveSystem.Load(saveFile);

            _gameState = _factoryGameState.CreateGameState(saveFile, gameStateData);

            Debug.Log($"[{GetType()}][LoadData]         GameData Loaded!");
        }
        catch(GameNotFoundException ex)
        {
            Debug.LogWarning($"[{GetType()}][LoadData] Game Not found!");

            _gameState = _factoryGameState.CreateGameState(saveFile, "Guilda", DefaultGameState);

        }catch (BadFormatGameException ex)
        {
            Debug.LogWarning($"[{GetType()}][LoadData] Bad format game data!");
            _gameState = _factoryGameState.CreateGameState(saveFile, "Guilda", DefaultGameState);
        }
    }

    public LevelUPDescription CompleteDay(DayReport dayReport)
    {
        _gameState.IncrementDay();

        return _gameState.Company.HandleDayReport(dayReport, CharacterLevelDatabase.Instance);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        if (_gameState != null)
        {
            SaveSystem.Save(new GameStateData(_gameState));
        }
    }

    public void LoadSave(string currentSave)
    {
        LoadData(currentSave);
    }

    public void NewGame(string guildName)
    {
        var saveFile = guildName.Replace(" ", "_") + ".json";
        _gameState = _factoryGameState.CreateGameState(saveFile, guildName, DefaultGameState);
    }

    public List<string> GetSaves()
    {
        return SaveSystem.GetAllSaveFileNames();
    }

    public void DeleteSave(string saveFile)
    {
        SaveSystem.DeleteSave(saveFile);
    }

    public void SetGameState(GameState gameState)
    {
        _gameState = gameState;
    }

    public DaySO GetCurrentDay()
    {
        return DayDatabase.Instance.GetDaySO(_gameState.Day);
    }

    public void DismissCharacter(CharacterSO character)
    {
        _gameState.Company.RemoveCharacter(character);
        _characterShopController.DismissCharacter(character);
    }

    public void BuyCharacter(CharacterSO character)
    {
        if (_gameState.Company.Balance < character.RecruitmentCost) return;

        _gameState.Company.DecreaseBalance(character.RecruitmentCost);
        _gameState.Company.AddCharacter(character);
        _characterShopController.BuyCharacter(character);
    }

    public bool CanAfford(CharacterSO character)
    {
        return _gameState.Company.Balance >= character.RecruitmentCost;
    }

    public GameState GameState => _gameState;

    public CharacterShopController CharacterShopController => _characterShopController;
}
