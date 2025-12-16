using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TurnBaseBattleController : MonoBehaviour
{
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
        _playerChracters = playerTeam;
        _enemyCharacters = enemyTeam;

        var allCharacters = new List<BattleCharacter>();
        allCharacters.AddRange(_playerChracters);
        allCharacters.AddRange(_enemyCharacters);

        _timelineController = new TimelineController(allCharacters.Select(c => (ITimelineElement)c).ToList());

        OnSetupReady?.Invoke(_playerChracters, _enemyCharacters, _timelineController);
    }

    public void StartBattle()
    {
        UpdateTurn();
    }

    private void UpdateTurn()
    {
        ITimelineElement timelineItem; 
        for (int i = 0; i < _timelineController.TrueSize; i++)
        {
            if (_timelineController.IsEmpty())
            {
                _timelineController.UpdateTimeLine();
            }

            timelineItem = _timelineController.Dequeue();

            if (timelineItem.IsActive())
            {
                _currentCharacter = (BattleCharacter)timelineItem;
                break;
            }
            else
            {
                ((BattleCharacter)timelineItem).OnTurnEnd();
            }
        }

        OnCharacterTurn?.Invoke(_playerChracters.Contains(_currentCharacter), _currentCharacter);
    }

    public void PassAction(BattleCharacter character)
    {
        UpdateTurn();
    }

    public SkillActionResult SkillAction(BattleCharacter character, BaseSkillSO skill, List<BattleCharacter> targets)
    {
        skill.ApplySkill(character, targets.Select(c => c as IBattleCharacter).ToList());

        _playerChracters.ForEach(c => c.UpdateAfterUseSkill());
        _enemyCharacters.ForEach(c => c.UpdateAfterUseSkill());

        foreach (var target in targets)
        {
            if (target.IsActive() && !target.IsAlive())
            {
                target.KillCharacter();
                _timelineController.TriggerItemDeactivated(target);
            }
        }

        if (_playerChracters.Contains(character))
        {
            if (!IsTeamAlive(_enemyCharacters))
            {
                _playerWin = true;
                OnBattleEnd?.Invoke(true);
            }
        }
        else
        {
            if (!IsTeamAlive(_playerChracters))
            {
                _playerWin = false;
                OnBattleEnd?.Invoke(false);
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