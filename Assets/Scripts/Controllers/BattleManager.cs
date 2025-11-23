using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    [SerializeField] private TurnBaseBattleController _turnBaseBattleController;

    private Team _playerTeam;
    private Team _enemyTeam;
    private Action<bool> _onBattleEndCallback;

    public void StartBattle(Team playerTeam, Team enemyTeam, Action<bool> onBattleEndCallback)
    {
        _playerTeam = playerTeam;
        _enemyTeam = enemyTeam;
        _onBattleEndCallback = onBattleEndCallback;

        var playerBattleCharacters = _playerTeam.Members.Select(c => new BattleCharacter(c)).ToList();
        var enemyBattleCharacters = _enemyTeam.Members.Select(c => new BattleCharacter(c)).ToList();

        _turnBaseBattleController.Setup(playerBattleCharacters, enemyBattleCharacters);
    }

    public (Team, Team) GetBattleArgs()
    {
        return (_playerTeam, _enemyTeam);
    }

    public void BattleEnd(bool isPlayerWinner)
    {
        _onBattleEndCallback?.Invoke(isPlayerWinner);
    }
}
