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

    public UnityEvent<bool, BattleCharacter> OnCharacterTurn;
    public UnityEvent<bool> OnBattleEnd;

    public void Setup(List<BattleCharacter> playerTeam, List<BattleCharacter> enemyTeam)
    {
        _playerChracters = playerTeam;
        _enemyCharacters = enemyTeam;

        var allCharacters = new List<BattleCharacter>();
        allCharacters.AddRange(_playerChracters);
        allCharacters.AddRange(_enemyCharacters);

        _timelineController = new TimelineController(allCharacters.Select(c => (ITimelineElement)c).ToList());
    }

    public void StartBattle()
    {
        UpdateTurn();
    }

    private void UpdateTurn()
    {
        if (_timelineController.IsEmpty())
        {
            _timelineController.UpdateTimeLine();
        }

        _currentCharacter = (BattleCharacter)_timelineController.Dequeue();

        OnCharacterTurn?.Invoke(_playerChracters.Contains(_currentCharacter), _currentCharacter);
    }

    public void PassAction(BattleCharacter character)
    {
        UpdateTurn();
    }

    public SkillActionResult SkillAction(BattleCharacter character, BaseSkillSO skill, List<BattleCharacter> targets)
    {
        skill.ApplySkill(character, targets.Select(c => c as IBattleCharacter).ToList());

        foreach (var target in targets)
        {
            if (target.IsActive() && !target.IsAlive())
            {
                target.KillCharacter();

                _timelineController.UpdateTimeLine();
            }
        }

        if (_playerChracters.Contains(character))
        {
            Debug.Log($"Check if Player Characters are alives");
            if (!IsTeamAlive(_enemyCharacters))
            {
                Debug.Log("     Player Lose");
                OnBattleEnd?.Invoke(true);
            }
        }
        else
        {
            Debug.Log($"Check if Enemies Characters are alives");
            if (!IsTeamAlive(_playerChracters))
            {
                Debug.Log("     Player Win");
                OnBattleEnd?.Invoke(false);
            }
        }

        var skillResult = new SkillActionResult(character, skill, targets);

        return skillResult;
    }

    private bool IsTeamAlive(List<BattleCharacter> team)
    {
        Debug.Log("IsTeamAlive");
        foreach (var c in team)
        {
            Debug.Log($"     => {c.BaseCharacter.Name} {c.IsAlive()} {c.Health}");
        }

        foreach (var character in team)
        {
            if (character.IsActive() && character.IsAlive()) return true;
        }

        return false;
    }

    public TimelineController TimelineController => _timelineController;
}