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

    public UnityEvent<BattleCharacter> OnCharacterTurn;
    public UnityEvent<bool> OnPlayerWin;

    public void Setup(Team playerTeam, Team enemyTeam)
    {
        _playerChracters = new List<BattleCharacter>();
        _enemyCharacters = new List<BattleCharacter>();

        playerTeam.Members.ForEach(c => _playerChracters.Add(new BattleCharacter(c)));
        enemyTeam.Members.ForEach(c => _enemyCharacters.Add(new BattleCharacter(c)));

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

        OnCharacterTurn?.Invoke(_currentCharacter);
    }

    public void PassAction(BattleCharacter character)
    {
        UpdateTurn();
    }

    public void SkillAction(BattleCharacter character, BaseSkillSO skill, List<BattleCharacter> targets)
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
            if (!IsTeamAlive(_enemyCharacters))
            {
                OnPlayerWin?.Invoke(true);
            }
        }
        else
        {
            if (!IsTeamAlive(_playerChracters))
            {
                OnPlayerWin?.Invoke(false);
            }
        }
    }

    private bool IsTeamAlive(List<BattleCharacter> team)
    {
        foreach (var character in team)
        {
            if (character.IsActive() && character.IsAlive()) return true;
        }

        return false;
    }
}