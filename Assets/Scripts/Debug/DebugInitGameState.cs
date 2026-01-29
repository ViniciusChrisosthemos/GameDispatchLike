using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugInitGameState : MonoBehaviour
{
    public List<CharacterSO> Characters;

    private void Awake()
    {
        var characterUnit = Characters.Select(s => new CharacterUnit(s)).ToList();

        var guild = new Company("temp", 10000, 15, 1, 0, characterUnit);

        //guild.AllCharacters.ForEach(c => c.SetScheduledCharater(true));

        var gameState = new GameState("temp", 1, guild);

        GameManager.Instance.SetGameState(gameState);
    }
}
