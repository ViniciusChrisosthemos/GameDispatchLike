using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UITurnBaseBattleView : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private TurnBaseBattleController _turnbaseBattleController;
    [SerializeField] private RollDiceController _rollDiceController;

    [SerializeField] private UIListDisplay _playerUIListDisplay;
    [SerializeField] private UIListDisplay _enemyUIListDisplay;

    [SerializeField] private UITimelineView _uiTimelineView;
    [SerializeField] private UISkillSelectionView _uiSkillSelectionView;

    [SerializeField] private UIResultScreenView _uiResultScreenView;

    private BattleCharacter _currentCharacter;
    private List<SkillAction> _skillActionQueue;
    private List<DiceValueSO> _currentDiceValues;
    private List<DiceValueSO> _lockedDices;


    private List<UIBattleCharacterView> _playerBattleCharacterViews;
    private List<UIBattleCharacterView> _enemyBattleCharacterViews;


    private void Awake()
    {
        _turnbaseBattleController.OnBattleEnd.AddListener(HandleBattleEnd);
        _turnbaseBattleController.OnCharacterTurn.AddListener(HandleCharacterTurn);
        _uiSkillSelectionView.OnPassAction.AddListener(Pass);

        _turnbaseBattleController.OnSetupReady.AddListener(HandleBattleSetupReady);
    }

    private void HandleBattleSetupReady(List<BattleCharacter> playerCharacters, List<BattleCharacter> enemyCharacters, TimelineController timelineController)
    {
        _view.SetActive(true);

        _playerUIListDisplay.SetItems(playerCharacters.Select(c => (object)c).ToList(), null);
        _enemyUIListDisplay.SetItems(enemyCharacters.Select(c => (object)c).ToList(), null);

        _playerBattleCharacterViews = _playerUIListDisplay.GetControllers().Select(i => i as UIBattleCharacterView).ToList();
        _enemyBattleCharacterViews = _enemyUIListDisplay.GetControllers().Select(i => i as UIBattleCharacterView).ToList();

        _uiSkillSelectionView.SetTeams(_playerBattleCharacterViews, _enemyBattleCharacterViews);
        _uiTimelineView.SetTimeline(timelineController);

        _turnbaseBattleController.StartBattle();
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
        _rollDiceController.RollDices(6, HandleDicesResult);

        _uiSkillSelectionView.HideButtons();
    }

    private void HandleDicesResult(List<DiceValueSO> diceValues)
    {
        _currentDiceValues = diceValues;
        _lockedDices = new List<DiceValueSO>();

        _uiSkillSelectionView.UpdateDices(diceValues);
        _uiSkillSelectionView.UpdateAvailableSkills(diceValues);
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

    public async void PlayActions()
    {
        foreach (var action in _skillActionQueue)
        {
            _turnbaseBattleController.SkillAction(action.Source, action.Skill, action.Targets);

            UpdateCharacters();

            await Task.Delay(1000);
        }

        Pass();
    }

    public void UpdateCharacters()
    {
        _playerBattleCharacterViews.ForEach(c => c.UpdateHealth());
        _enemyBattleCharacterViews.ForEach(c => c.UpdateHealth());
    }

    private void HandleBattleEnd(bool playerWin)
    {
        _uiResultScreenView.ShowResult(playerWin, () =>
        {
            _view.SetActive(false);
            _turnbaseBattleController.EndBattle();
        });
    }

    public List<SkillAction> GetActionQueue()
    {
        return _skillActionQueue;
    }

    public void RemoveAction(SkillAction action)
    {
        _skillActionQueue.Remove(action);

        action.Skill.RequiredDiceValues.ForEach(d => _lockedDices.Remove(d));
        _uiSkillSelectionView.UpdateActionQueue(_skillActionQueue, _currentDiceValues, _lockedDices);
    }
}
