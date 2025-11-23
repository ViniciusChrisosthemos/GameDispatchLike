using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatManager;

public class Team
{
    private const string WARNING_MESSAGE_MAX_MEMBER_AMOUNT_REACHED = "Cannot add member: team is at maximum capacity.";

    private List<CharacterUnit> _members;
    private int _maxSize;

    public Team(int maxSize)
    {
        _maxSize = maxSize;
        _members = new List<CharacterUnit>();
    }

    public Team(List<CharacterUnit> members)
    {
        _members = members;
        _maxSize = members.Count;
    }

    public void AddMember(CharacterUnit newMember)
    {
        if (_members.Count < _maxSize)
        {
            _members.Add(newMember);
        }
        else
        {
            Debug.LogWarning(WARNING_MESSAGE_MAX_MEMBER_AMOUNT_REACHED);
        }
    }

    public void RmvMember(CharacterUnit memberToRemove)
    {
        _members.Remove(memberToRemove);
    }

    public StatManager GetTeamStats()
    {
        var teamStates = new StatManager();

        foreach (var member in _members)
        {
            teamStates.ExpendStats(member.StatManager);
        }

        return teamStates;
    }

    public bool HasMember(CharacterUnit characterUnit) => _members.Contains(characterUnit);

    public float GetMoveSpeed() => Members[0].BaseCharacterSO.BaseMoveSpeed;
    public Stat GetStat(StatType statType) => GetTeamStats().GetStat(statType);
    public int Size => _members.Count;
    public int MaxSize { get { return _maxSize; } }
    public List<CharacterUnit> Members { get { return _members; } }
}
