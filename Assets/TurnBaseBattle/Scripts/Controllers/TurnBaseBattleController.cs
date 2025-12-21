using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class TurnBaseBattleController : MonoBehaviour
{
    [SerializeField] private BattleLogger _battleLogger;

    private List<BattleCharacter> _playerChracters;
    private List<BattleCharacter> _enemyCharacters;
    private TimelineController _timelineController;
    private BattleCharacter _currentCharacter;

    public UnityEvent<bool> OnBattleEnd;
    public UnityEvent<bool, BattleCharacter> OnCharacterTurn;
    public UnityEvent<List<BattleCharacter>, List<BattleCharacter>, TimelineController> OnSetupReady;

    private bool _playerWin = false;

    public void Setup(List<BattleCharacter> playerTeam, List<BattleCharacter> enemyTeam)
    {
        _battleLogger.Reset();

        _playerChracters = playerTeam;
        _enemyCharacters = enemyTeam;

        _playerChracters.ForEach(c => c.Individuality.Init(c));
        _enemyCharacters.ForEach(c => c.Individuality.Init(c));

        var allCharacters = new List<BattleCharacter>();
        allCharacters.AddRange(_playerChracters);
        allCharacters.AddRange(_enemyCharacters);

        _timelineController = new TimelineController(allCharacters.Select(c => (ITimelineElement)c).ToList());

        OnSetupReady?.Invoke(_playerChracters, _enemyCharacters, _timelineController);
    }

    public void StartBattle()
    {
        _battleLogger.Log("Battle begins!");

        UpdateTurn();
    }

    private void UpdateTurn()
    {
        if (_currentCharacter != null)
        {
            _currentCharacter.OnTurnEnd();
            _battleLogger.Log($"End {_currentCharacter.GetName()} turn");
        }

        ITimelineElement timelineItem; 
        
        for (int i = 0; i < _timelineController.TrueSize; i++)
        {
            if (_timelineController.IsEmpty())
            {
                _timelineController.UpdateTimeLine();
            }

            timelineItem = _timelineController.Dequeue();
            var battleCharacter = (BattleCharacter)timelineItem;

            if (timelineItem.IsActive())
            {
                _currentCharacter = battleCharacter;
                break;
            }
            else
            {
                battleCharacter.OnTurnEnd();
                _battleLogger.Log($"Skip {_currentCharacter.GetName()} turn");
            }
        }


        _battleLogger.Log($"Start {_currentCharacter.GetName()} turn");

        _currentCharacter?.OnTurnStart();

        OnCharacterTurn?.Invoke(_playerChracters.Contains(_currentCharacter), _currentCharacter);
    }

    public void PassAction(BattleCharacter character)
    {
        UpdateTurn();
    }

    public SkillActionResult SkillAction(BattleCharacter character, BaseSkillSO skill, List<BattleCharacter> targets)
    {
        skill.ApplySkill(character, targets.Select(c => c as IBattleCharacter).ToList(), _battleLogger);

        _playerChracters.ForEach(c => c.UpdateAfterUseSkill());
        _enemyCharacters.ForEach(c => c.UpdateAfterUseSkill());

        foreach (var target in targets)
        {
            if (target.IsActive() && !target.IsAlive())
            {
                target.KillCharacter();
                _timelineController.TriggerItemDeactivated(target);

                _battleLogger.Log($"{target.GetName()} dies");
            }
        }

        if (_playerChracters.Contains(character))
        {
            if (!IsTeamAlive(_enemyCharacters))
            {
                _playerWin = true;
                OnBattleEnd?.Invoke(true);


                _battleLogger.Log($"Player wins!");
            }
        }
        else
        {
            if (!IsTeamAlive(_playerChracters))
            {
                _playerWin = false;
                OnBattleEnd?.Invoke(false);

                _battleLogger.Log($"Player losses!");
            }
        }

        var skillResult = new SkillActionResult(character, skill, targets);

        return skillResult;
    }

    private bool IsTeamAlive(List<BattleCharacter> team)
    {
        foreach (var character in team)
        {
            if (character.IsAlive()) return true;
        }

        return false;
    }

    public TimelineController TimelineController => _timelineController;

    public void EndBattle()
    {
        BattleManager.Instance.BattleEnd(_playerWin);
    }
}