using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugBattleScreen : MonoBehaviour
{
    public List<CharacterSO> Characters;

    private void Awake()
    {
        var characterUnit = Characters.Select(s => new CharacterUnit(s)).ToList();

        var guild = new Guild("temp", 0, 0, 1, 0, characterUnit);

        guild.AllCharacters.ForEach(c => c.SetScheduledCharater(true));

        var gameState = new GameState("temp", 1, guild);

        GameManager.Instance.SetGamState(gameState);
    }
}
