using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    [SerializeField] private UIOpenBattleScreenView _playerOpenBattleTeamView;
    [SerializeField] private UIOpenBattleScreenView _enemyOpenBattleTeamView;

    [SerializeField] private UIBattleAnimationHelperView _battleSkillAnimationHelperView;

    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _openScreenTrigger = "OpenBattleScreen";
    [SerializeField] private string _openScreenState = "OpenBattleScreen";

    [Header("Screen Configuration")]
    [SerializeField] private ScreenConfigurationSO _screenConfigurationSO;

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
        
        _view.SetActive(false);
    }

    private void HandleBattleSetupReady(List<BattleCharacter> playerCharacters, List<BattleCharacter> enemyCharacters, TimelineController timelineController)
    {
        _playerUIListDisplay.SetItems(playerCharacters, null);
        _enemyUIListDisplay.SetItems(enemyCharacters, null);

        _playerBattleCharacterViews = _playerUIListDisplay.GetControllers().Select(i => i as UIBattleCharacterView).ToList();
        _enemyBattleCharacterViews = _enemyUIListDisplay.GetControllers().Select(i => i as UIBattleCharacterView).ToList();

        _uiSkillSelectionView.SetTeams(_playerBattleCharacterViews, _enemyBattleCharacterViews);
        _uiTimelineView.SetTimeline(timelineController);

        _playerOpenBattleTeamView.SetTeam(playerCharacters);
        _enemyOpenBattleTeamView.SetTeam(enemyCharacters);

        Debug.Log("OK");

        StartCoroutine(PlayAnimation(_openScreenTrigger, _openScreenState, null));
    }

    private IEnumerator PlayAnimation(string trigger, string state, Action callback)
    {
        _animator.SetTrigger(trigger);

        StartCoroutine(WaitForSecondsCoroutine(5f, () => 
        {
            _view.SetActive(true);
            _turnbaseBattleController.StartBattle();
        }));

        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName(state));

        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        yield return null;

        callback?.Invoke();
    }

    private IEnumerator WaitForSecondsCoroutine(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);

        callback?.Invoke();
    }


    private void HandleCharacterTurn(bool isPlayerCharacter, BattleCharacter character)
    {
        Debug.Log($"Character Turn: {character.BaseCharacter.Name}");
        _currentCharacter = character;

        if (isPlayerCharacter)
        {
            _skillActionQueue = new List<SkillAction>();

            _uiSkillSelectionView.SetCharacter(character);

            if (_currentCharacter.BaseCharacter.HasTurnVoiceLines())
            {
                SoundManager.Instance.PlaySFX(_currentCharacter.BaseCharacter.GetTurnVoiceLine(), 0.6f);
            }

            _playerBattleCharacterViews.Find(view => view.BattleCharacter == character).UpdateCharacterView();
        }
        else
        {
            _uiSkillSelectionView.SetActive(false);

            _enemyBattleCharacterViews.Find(view => view.BattleCharacter == character).UpdateCharacterView();
        }
    }

    public void RollDices()
    {
        var diceManager = _currentCharacter.GetDiceManager();

        _rollDiceController.RollDices(diceManager.GetSkillDicePrefab(), diceManager.GetSkillDices(), HandleDicesResult);

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

        _uiSkillSelectionView.OnTurnEnd();
    }

    public void RegisterAction(SkillAction currenSkillAction)
    {
        _skillActionQueue.Add(currenSkillAction);

        _lockedDices.AddRange(currenSkillAction.Skill.RequiredDiceValues);

        _uiSkillSelectionView.UpdateActionQueue(_skillActionQueue, _currentDiceValues, _lockedDices);
    }

    public void PlayActions()
    {
        StartCoroutine(PlayActionsCoroutine(Pass));
    }

    private IEnumerator PlayActionsCoroutine(Action callback)
    {
        foreach (var action in _skillActionQueue)
        {
            var skillResult = _turnbaseBattleController.SkillAction(action.Source, action.Skill, action.Targets);

            yield return AnimateAction(true, skillResult);
        }

        callback?.Invoke();
    }

    public IEnumerator AnimateAction(bool isPlayer, SkillActionResult actionResult)
    {
        if (actionResult.Skill.DataSO != null)
        {
            var ownerList = isPlayer ? _playerBattleCharacterViews : _enemyBattleCharacterViews;
            var opponentList = isPlayer ? _enemyBattleCharacterViews : _playerBattleCharacterViews;

            var source = ownerList.Find(view => view.BattleCharacter == actionResult.Source);
            var targets = opponentList.Where(view => actionResult.Targets.Contains(view.BattleCharacter)).ToList();

            if (!targets.Any())
            {
                targets = ownerList.Where(view => actionResult.Targets.Contains(view.BattleCharacter)).ToList();
            }

            yield return _battleSkillAnimationHelperView.AnimateSkillCoroutine(actionResult.Skill, source, targets, null);
        }

        UpdateCharacters();
        _uiSkillSelectionView.UpdateIndividualityView();
    }

    public void UpdateCharacters()
    {

        _playerBattleCharacterViews.ForEach(c => c.UpdateCharacterView());
        _enemyBattleCharacterViews.ForEach(c => c.UpdateCharacterView());
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

    public void PlayBattleMusic()
    {
        SoundManager.Instance.PlayMusic(_screenConfigurationSO.MusicBackground, _screenConfigurationSO.MusicVolume, _screenConfigurationSO.InLoop);
    }
}
