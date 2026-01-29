using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugInitGameState : MonoBehaviour
{
    public List<CharacterSO> Characters;

    private void Awake()
    {
        int owned = 4;

        var allcharacterUnit = Characters.Select(s => new CharacterUnit(s)).ToList();

        var characters = allcharacterUnit.Take(owned).ToList();
        var candidates = allcharacterUnit.Skip(owned).ToList();

        var guild = new Company("temp", 0, 0, 1, 0, characters);

        guild.AllCharacters.ForEach(c => c.SetScheduledCharater(true));

        var gameState = new GameState("temp", 1, guild, candidates);

        GameManager.Instance.SetGameState(gameState);
    }
}
