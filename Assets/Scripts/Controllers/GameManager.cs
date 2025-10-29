using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private GameState _gameState;

    [SerializeField] private List<CharacterSO> AvailableCharacters;

    private void Awake()
    {
        var day = 1;
        var charactersUnits = AvailableCharacters.Select(c => new CharacterUnit(c)).ToList();
        var guild = new Guild("Guilda", 0, 0, charactersUnits);

        _gameState = new GameState(day, guild);
    }

    public GameState GameState => _gameState;
}
