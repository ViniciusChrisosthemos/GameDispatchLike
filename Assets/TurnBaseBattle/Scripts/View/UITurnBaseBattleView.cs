using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UITurnBaseBattleView : MonoBehaviour
{
    [SerializeField] private TurnBaseBattleController _turnbaseBattleController;
    [SerializeField] private RollDiceController _rollDiceController;

    [SerializeField] private UIListDisplay _playerUIListDisplay;
    [SerializeField] private UIListDisplay _enemyUIListDisplay;

    [SerializeField] private UITimelineView _uiTimelineView;
    [SerializeField] private UISkillSelectionView _uiSkillSelectionView;
    [SerializeField] private GameObject _diceView;

    [SerializeField] private UIResultScreenView _uiResultScreenView;

    public TeamSO PlayerTeam;
    public TeamSO EnemyTeam;

    private BattleCharacter _currentCharacter;
    private List<SkillAction> _skillActionQueue;
    private List<DiceValueSO> _currentDiceValues;
    private List<DiceValueSO> _lockedDices;

    private void Awake()
    {
        _turnbaseBattleController.OnCharacterTurn.AddListener(HandleCharacterTurn);
        _uiSkillSelectionView.OnPassAction.AddListener(Pass);
    }

    private void Start()
    {
        var playerCharacters = new List<BattleCharacter>();
        var enemyCharacters = new List<BattleCharacter>();

        PlayerTeam.GetTeam().Members.ForEach(c => playerCharacters.Add(new BattleCharacter(c)));
        EnemyTeam.GetTeam().Members.ForEach(c => enemyCharacters.Add(new BattleCharacter(c)));

        _playerUIListDisplay.SetItems(playerCharacters.Select(c => (object)c).ToList(), HandlePlayerCharacterSelected);
        _enemyUIListDisplay.SetItems(enemyCharacters.Select(c => (object)c).ToList(), null);


        var playerBattleCharacterViews = _playerUIListDisplay.GetControllers().Select(i => i as UIBattleCharacterView).ToList();
        var enemyBattleCharacterViews = _enemyUIListDisplay.GetControllers().Select(i => i as UIBattleCharacterView).ToList();
        _uiSkillSelectionView.SetTeams(playerBattleCharacterViews, enemyBattleCharacterViews);

        _turnbaseBattleController.OnBattleEnd.AddListener(HandleBattleEnd);
        _turnbaseBattleController.Setup(playerCharacters, enemyCharacters);
        _turnbaseBattleController.StartBattle();

        _uiTimelineView.SetTimeline(_turnbaseBattleController.TimelineController);
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
            _skillActionQueue = new List<SkillAction>();

            _uiSkillSelectionView.SetCharacter(character);
        }
        else
        {
            _uiSkillSelectionView.SetActive(false);
        }
    }

    public void RollDices()
    {
        _diceView.SetActive(true);
        _rollDiceController.RollDices(6, HandleDicesResult);

        _uiSkillSelectionView.HideButtons();
    }

    private void HandleDicesResult(List<DiceValueSO> diceValues)
    {
        _currentDiceValues = diceValues;
        _lockedDices = new List<DiceValueSO>();

        _rollDiceController.SetActive(false);
        _uiSkillSelectionView.UpdateDices(diceValues);
        _uiSkillSelectionView.UpdateAvailableSkills(diceValues);
        _diceView.SetActive(false);
    }

    public void Pass()
    {
        _turnbaseBattleController.PassAction(_currentCharacter);
    }

    public void RegisterAction(SkillAction currenSkillAction)
    {
        _skillActionQueue.Add(currenSkillAction);

        _lockedDices.AddRange(currenSkillAction.Skill.RequiredDiceValues);

        _uiSkillSelectionView.UpdateActionQueue(_skillActionQueue, _currentDiceValues, _lockedDices);
    }

    private void HandlePlayerCharacterSelected(UIItemController controller)
    {
    }

    public async void PlayActions()
    {
        var playerBattleCharacterViews = _playerUIListDisplay.GetControllers().Select(i => i as UIBattleCharacterView).ToList();
        var enemyBattleCharacterViews = _enemyUIListDisplay.GetControllers().Select(i => i as UIBattleCharacterView).ToList();

        foreach (var action in _skillActionQueue)
        {
            _turnbaseBattleController.SkillAction(action.Source, action.Skill, action.Targets);

            playerBattleCharacterViews.ForEach(c => c.UpdateHealth());
            enemyBattleCharacterViews.ForEach (c => c.UpdateHealth());

            await Task.Delay(1000);
        }

        Pass();
    }

    private void HandleBattleEnd(bool playerWin)
    {
        _uiResultScreenView.ShowResult(playerWin);
    }
}
