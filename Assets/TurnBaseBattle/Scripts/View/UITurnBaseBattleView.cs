using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITurnBaseBattleView : MonoBehaviour
{
    [SerializeField] private TurnBaseBattleController _turnbaseBattleController;

    public TeamSO PlayerTeam;
    public TeamSO EnemyTeam;

    private void Awake()
    {
        _turnbaseBattleController.OnCharacterTurn.AddListener(HandleCharacterTurn);
    }

    private void HandleCharacterTurn(BattleCharacter character)
    {

    }

    private void Start()
    {
        _turnbaseBattleController.Setup(PlayerTeam.GetTeam(), EnemyTeam.GetTeam());
        _turnbaseBattleController.StartBattle();
    }
}
