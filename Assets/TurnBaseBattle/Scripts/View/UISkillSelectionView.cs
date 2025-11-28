using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISkillSelectionView : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private Image _imgCharacter;
    [SerializeField] private Button _btnPass;
    [SerializeField] private Button _btnRollDices;
    [SerializeField] private Button _btnPlayActions;
    [SerializeField] private UIListDisplay _skillListDisplay;
    [SerializeField] private UIListDisplay _skillActionListDisplay;
    [SerializeField] private UITurnBaseBattleView _uiTurnBaseBattleView;
    [SerializeField] private UIListDisplay _dicesValuesBackgroundListDisplay;
    [SerializeField] private UIListDisplay _dicesValuesListDisplay;

    [Header("Individuality")]
    [SerializeField] private Transform _individualitiesParent;
    private Dictionary<IBattleCharacter, AbstractIndividualityView> _individualitiesViews;

    [Header("Events")]
    public UnityEvent OnPassAction;

    private List<UIBattleCharacterView> _playerBattleCharactersControllers;
    private List<UIBattleCharacterView> _enemyBattleCharactersControllers;

    private AbstractIndividualityView _currentIndividualityView;

    private SelectionState _selectionState;
    private SkillAction _currenSkillAction;
    
    private enum SelectionState
    {
        SelectingSkill,
        SelectingTarget
    }

    private void Awake()
    {
        _btnPass.onClick.AddListener(Pass);
        _btnRollDices.onClick.AddListener(RollDices);
        _btnPlayActions.onClick.AddListener(PlayActions);
    }

    public void SetTeams(List<UIBattleCharacterView> playerCahractersControllers, List<UIBattleCharacterView> enemyCharactersControllers)
    {
        _playerBattleCharactersControllers = playerCahractersControllers;
        _enemyBattleCharactersControllers = enemyCharactersControllers;

        _playerBattleCharactersControllers.ForEach(c => c.OnSelected.AddListener(HandleCharacterViewSelected));
        _enemyBattleCharactersControllers.ForEach(c => c.OnSelected.AddListener(HandleCharacterViewSelected));

        _selectionState = SelectionState.SelectingSkill;

        _individualitiesParent.ClearChilds();
        _individualitiesViews = new Dictionary<IBattleCharacter, AbstractIndividualityView>();

        foreach (var character in playerCahractersControllers)
        {
            if (!character.HasIndividuality) continue;
            
            var individualityView = IndividualityFactory.CreateIndividualityView(character.BattleCharacter, _individualitiesParent);

            _individualitiesViews.Add(character.BattleCharacter, individualityView);

            individualityView.CloseView();
        }
    }

    public void SetCharacter(BattleCharacter character)
    {
        _view.SetActive(true);

        _imgCharacter.sprite = character.BaseCharacter.BodyArt;

        var items = character.BaseCharacter.Skills.Select(s => (object)s).ToList();
        _skillListDisplay.SetItems(items, HandleSkillSelected);

        _currenSkillAction = new SkillAction(character, null, null);
        _skillActionListDisplay.Clear();
        _dicesValuesListDisplay.Clear();

        _playerBattleCharactersControllers.ForEach(c => c.Clear());
        _enemyBattleCharactersControllers.ForEach(c => c.Clear());

        _btnPass.gameObject.SetActive(true);
        _btnRollDices.gameObject.SetActive(true);
        _btnPlayActions.gameObject.SetActive(false);


        if (_individualitiesViews.ContainsKey(character))
        {
            _currentIndividualityView = _individualitiesViews[character];
            _currentIndividualityView.OpenView();
            _currentIndividualityView.OnTurnStart();
        }

        _dicesValuesBackgroundListDisplay.SetItems(character.GetDiceManager().GetSkillDices());
    }

    private void HandleSkillSelected(UIItemController controller)
    {
        var skill = controller.GetItem<BaseSkillSO>();

        _selectionState = SelectionState.SelectingTarget;

        switch (skill.SkillTargetType)
        {
            case SkillTargetType.Ally: HandleTargetSkill(skill, _playerBattleCharactersControllers); break;
            case SkillTargetType.Enemy: HandleTargetSkill(skill, _enemyBattleCharactersControllers); break;
            default: break;
        }

        _currenSkillAction.Skill = skill;
    }

    private void HandleTargetSkill(BaseSkillSO skill, List<UIBattleCharacterView> targetControllers)
    {
        targetControllers.ForEach(c => c.SetTarget());
    }

    private void HandleCharacterViewSelected(UIBattleCharacterView characterView)
    {
        Debug.Log($"{characterView.name}", characterView.gameObject);
        if (_selectionState != SelectionState.SelectingTarget) return;

        var targets = new List<UIBattleCharacterView>();
        var controllerList = _enemyBattleCharactersControllers.Contains(characterView) ? _enemyBattleCharactersControllers : _playerBattleCharactersControllers;

        switch (_currenSkillAction.Skill.SkillTargetAmount)
        {
            case SkillTargetAmountType.AllTargets: targets = controllerList; break;
            case SkillTargetAmountType.SingleTarget: targets.Add(characterView); break;
            default: break;
        }

        var battleCharactersTargets = targets.Select(t => t.BattleCharacter).ToList();

        controllerList.ForEach(c => c.ClearSelection());

        targets.ForEach(c => c.SetConfirmedTarget());

        _selectionState = SelectionState.SelectingSkill;

        _currenSkillAction.Targets = battleCharactersTargets;

        _uiTurnBaseBattleView.RegisterAction(_currenSkillAction);

        _currenSkillAction = new SkillAction(_currenSkillAction.Source, null, null);
    }

    public void UpdateActionQueue(List<SkillAction> skillActionQueue, List<DiceValueSO> availableDices, List<DiceValueSO> lockedDices)
    {
        var availableDicesCopy = availableDices.ToList();
        var lockedDicesCopy = lockedDices.ToList();

        lockedDicesCopy.ForEach(d => availableDicesCopy.Remove(d));

        _skillActionListDisplay.SetItems(skillActionQueue.Select(sa => (object)sa).ToList(), HandleSkillActionSelected);

        foreach (var controller in _dicesValuesListDisplay.GetControllers())
        {
            var diceValue = controller.GetItem<DiceValueSO>();
            var diceController = controller.GetComponent<UIDiceView>();

            diceController.SetDiceLocked(false);

            if (lockedDicesCopy.Contains(diceValue))
            {
                diceController.SetDiceLocked(true);
                lockedDicesCopy.Remove(diceValue);
            }
        }

        UpdateAvailableSkills(availableDicesCopy);

        _btnPlayActions.gameObject.SetActive(skillActionQueue.Count > 0);
    }

    private void HandleSkillActionSelected(UIItemController controller)
    {
        var action = controller.GetItem<SkillAction>();

        _uiTurnBaseBattleView.RemoveAction(action);
    }

    public void SetActive(bool v)
    {
        _view.SetActive(v);
    }

    public void Pass()
    {
        OnPassAction?.Invoke();
    }

    public void PlayActions()
    {
        _uiTurnBaseBattleView.PlayActions();

        _playerBattleCharactersControllers.ForEach(c => c.Clear());
        _enemyBattleCharactersControllers.ForEach(c => c.Clear());
    }

    public void RollDices()
    {
        _uiTurnBaseBattleView.RollDices();
    }

    public void UpdateAvailableSkills(List<DiceValueSO> diceValues)
    {
        Debug.Log($"[UpdateAvailableSkills] {diceValues.Count}");

        foreach (var controller in _skillListDisplay.GetControllers())
        {
            var skillController = controller as UISkillDisplayController;
            var skill = skillController.GetItem<BaseSkillSO>();
            var requiredDices = skill.RequiredDiceValues;

            var isAvailable = IsSkillAvailable(requiredDices, diceValues);
            skillController.SetAvailable(!isAvailable);

            Debug.Log($"    Skill {skill.Name} {isAvailable}");
        }
    }

    public void UpdateDices(List<DiceValueSO> diceValues)
    {
        var orderedValues = diceValues.OrderBy(d => d.Priority).Select(v => v as object).ToList();

        _dicesValuesListDisplay.SetItems(orderedValues, null);

        _btnPass.gameObject.SetActive(true);
        _btnRollDices.gameObject.SetActive(false);
        _btnPlayActions.gameObject.SetActive(false);
    }

    public void HideButtons()
    {
        _btnPass.gameObject.SetActive(false);
        _btnRollDices.gameObject.SetActive(false);
        _btnPlayActions.gameObject.SetActive(false);
    }

    private bool IsSkillAvailable<T>(List<T> l1, List<T> l2)
    {
        var c1 = l1.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
        var c2 = l2.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());

        return c1.All(kv => c2.TryGetValue(kv.Key, out int c) && c >= kv.Value);
    }

    public void UpdateIndividualityView()
    {
        if (_currentIndividualityView != null)
        {
            _currentIndividualityView.UpdateView();
        }
    }

    public void OnTurnEnd()
    {
        if (_currentIndividualityView != null)
        {
            _currentIndividualityView.OnTurnEnd();
            _currentIndividualityView.CloseView();
        }
    }
}
