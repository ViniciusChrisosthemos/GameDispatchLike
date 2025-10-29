using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

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

        LoadData();
    }

    public UnityEvent OnQuitGame;

    private void LoadData()
    {
        Debug.Log($"[{GetType()}][LoadData] Loading GameData ...");

        try
        {
            var gameStateData = SaveSystem.Load();

            _gameState = _factoryGameState.CreateGameState(gameStateData);

            Debug.Log($"[{GetType()}][LoadData]         GameData Loaded!");
        }
        catch(GameNotFoundException ex)
        {
            Debug.LogWarning($"[{GetType()}][LoadData] Game Not found!");

            _gameState = _factoryGameState.CreateGameState("Guilda", DefaultGameState);

        }catch (BadFormatGameException ex)
        {
            Debug.LogWarning($"[{GetType()}][LoadData] Bad format game data!");
            _gameState = _factoryGameState.CreateGameState("Guilda", DefaultGameState);
        }
    }

    public void CompleteDay()
    {
        _gameState.IncrementDay();
    }

    public UIGuildViewManager guildView;

    public void Quit()
    {
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        guildView.CommitChanges();
        SaveSystem.Save(new GameStateData(_gameState));
    }

    public GameState GameState => _gameState;
}
