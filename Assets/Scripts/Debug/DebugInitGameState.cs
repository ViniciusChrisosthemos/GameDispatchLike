using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugInitGameState : MonoBehaviour
{
    public List<CharacterSO> Characters;
    public List<ContractMisionSO> Contracts;

    private void Awake()
    {
        int owned = 14;

        var allcharacterUnit = Characters.Select(s => new CharacterUnit(s)).ToList();

        var characters = allcharacterUnit.Take(owned).ToList();
        var candidates = allcharacterUnit.Skip(owned).ToList();

        var guild = new Company("temp", 4000, 0, 1, 0, characters);

        guild.AllCharacters.ForEach(c => c.SetScheduledCharater(true));

        var availableContracts = Contracts.Select(c => new ContractMissionRuntime(c)).ToList();
        var ongoingContracts = new List<ContractMissionRuntime>();

        var gameState = new GameState("temp", 1, guild, candidates, availableContracts, ongoingContracts);

        GameManager.Instance.SetGameState(gameState);
    }
}
