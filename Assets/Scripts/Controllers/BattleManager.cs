using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BattleManager : Singleton<BattleManager>
{
    [SerializeField] private TurnBaseBattleController _turnBaseBattleController;
    [SerializeField] private UIBattleHUDView _uiBattleHUDView;
    [SerializeField] private UICharacterHUDView _uiCharacterHUDView;
    [SerializeField] private TargetSelectionController _targetSelectionController;
    [SerializeField] private CharacterSpotManager _characterSpotManager;
    [SerializeField] private BattleCameraController _battleCameraController;
    [SerializeField] private BattleEnemyBehaviour _battleEnemyBehaviour;

    [Header("Objects To Disable")]
    [SerializeField] private List<GameObject> _objectsToDisableOnBattleEnd;

    private Team _playerTeam;
    private Team _enemyTeam;
    private Action<bool> _onBattleEndCallback;

    public UnityEvent OnBattleEnd;

    public void StartBattle(Team playerTeam, Team enemyTeam, Action<bool> onBattleEndCallback)
    {
        _playerTeam = playerTeam;
        _enemyTeam = enemyTeam;
        _onBattleEndCallback = onBattleEndCallback;

        var playerBattleCharacters = _playerTeam.Members.Select(c => new BattleCharacter(c)).ToList();
        var enemyBattleCharacters = _enemyTeam.Members.Select(c => new BattleCharacter(c)).ToList();

        _turnBaseBattleController.Setup(playerBattleCharacters, enemyBattleCharacters);
        _characterSpotManager.Setup(this);
        _uiBattleHUDView.Setup(this);
        _uiCharacterHUDView.Setup(this);
        _targetSelectionController.Setup(this);
        _battleCameraController.Setup(this);
        _battleEnemyBehaviour.Setup(this);

        _turnBaseBattleController.StartBattle();
    }

    public (Team, Team) GetBattleArgs()
    {
        return (_playerTeam, _enemyTeam);
    }

    public void BattleEnd(bool isPlayerWinner)
    {
        OnBattleEnd?.Invoke();
        _onBattleEndCallback?.Invoke(isPlayerWinner);

        _objectsToDisableOnBattleEnd.ForEach(obj => obj.SetActive(false));
    }

    public TurnBaseBattleController GetBattleController() => _turnBaseBattleController;

    public CharacterSpotManager GetCharacterSpotManager() => _characterSpotManager;

    public BattleCameraController GetBattleCameraController() => _battleCameraController;
}
