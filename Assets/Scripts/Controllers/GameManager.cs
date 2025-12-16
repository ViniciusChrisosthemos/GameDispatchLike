using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static Guild;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameStateSO DefaultGameState;

    private GameState _gameState;

    private FactoryCharacterUnit _factoryCharacterUnit;
    private FactoryGameState _factoryGameState;

    private void Start()
    {
        _factoryCharacterUnit = new FactoryCharacterUnit(CharacterDatabase.Instance);
        _factoryGameState = new FactoryGameState(_factoryCharacterUnit);
    }

    public UnityEvent OnQuitGame;

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
        return _gameState.Guild.HandleDayReport(dayReport, CharacterLevelDatabase.Instance);
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

    public void SetGamState(GameState gameState)
    {
        _gameState = gameState;
    }

    public GameState GameState => _gameState;
}
