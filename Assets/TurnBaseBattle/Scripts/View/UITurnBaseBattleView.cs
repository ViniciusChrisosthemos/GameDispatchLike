using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UITurnBaseBattleView : MonoBehaviour
{
    [SerializeField] private TurnBaseBattleController _turnbaseBattleController;

    [SerializeField] private UIListDisplay _playerUIListDisplay;
    [SerializeField] private UIListDisplay _enemyUIListDisplay;

    [SerializeField] private UITimelineView _uiTimelineView;
    [SerializeField] private UISkillSelectionView _uiSkillSelectionView;

    public TeamSO PlayerTeam;
    public TeamSO EnemyTeam;

    private BattleCharacter _currentCharacter;

    private void Awake()
    {
        _turnbaseBattleController.OnCharacterTurn.AddListener(HandleCharacterTurn);
        _uiSkillSelectionView.OnPassAction.AddListener(Pass);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Pass();
        }
    }

    private void HandleCharacterTurn(bool isPlayerCharacter, BattleCharacter character)
    {
        Debug.Log($"Character Turn: {character.BaseCharacter.Name}");
        _currentCharacter = character;

        if (isPlayerCharacter)
        {
            _uiSkillSelectionView.SetCharacter(character);
        }
        else
        {
            _uiSkillSelectionView.SetActive(false);
        }
    }

    private void Start()
    {
        var playerCharacters = new List<BattleCharacter>();
        var enemyCharacters = new List<BattleCharacter>();

        PlayerTeam.GetTeam().Members.ForEach(c => playerCharacters.Add(new BattleCharacter(c)));
        EnemyTeam.GetTeam().Members.ForEach(c => enemyCharacters.Add(new BattleCharacter(c)));

        _turnbaseBattleController.Setup(PlayerTeam.GetTeam(), EnemyTeam.GetTeam());
        _turnbaseBattleController.StartBattle();

        _playerUIListDisplay.SetItems(playerCharacters.Select(c => (object)c).ToList(), HandlePlayerCharacterSelected);
        _enemyUIListDisplay.SetItems(enemyCharacters.Select(c=>(object)c).ToList(), null);

        _uiTimelineView.SetTimeline(_turnbaseBattleController.TimelineController);

        var playerBattleCharacterViews = _playerUIListDisplay.GetItems().Select(i => i as UIBattleCharacterView).ToList();
        var enemyBattleCharacterViews = _enemyUIListDisplay.GetItems().Select(i => i as UIBattleCharacterView).ToList();
        _uiSkillSelectionView.SetTeams(playerBattleCharacterViews, enemyBattleCharacterViews);
    }

    public void Pass()
    {
        _turnbaseBattleController.PassAction(_currentCharacter);
    }

    private void HandlePlayerCharacterSelected(UIItemController controller)
    {
    }
}
