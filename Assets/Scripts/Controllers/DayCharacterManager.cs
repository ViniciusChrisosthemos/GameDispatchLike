using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DayCharacterManager : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent<CharacterUnit> OnCharacterChangeStatus;

    private List<CharacterUnit> _availableCharacters;

    public void Init(List<CharacterUnit> characters)
    {
        _availableCharacters = characters;
    }

    public void HandleTeamAcceptMission(Team team)
    {
        foreach (CharacterUnit character in team.Members)
        {
            character.SetStatusToInGoingToMission();

            OnCharacterChangeStatus?.Invoke(character);
        }
    }

    public void HandleTeamCompleteMission(MissionUnit missionUnit, Team team, bool isSuccess, float currentTime)
    {
        team.Members.ForEach(c => c.HandleMissionCompleted(isSuccess ? missionUnit.Exp : 0, currentTime));
    }

    public void HandleTeamStartMission(Team team)
    {
        team.Members.ForEach(c => c.SetStatusToInMission());
    }

    public void UpdateCharacters(float elapseTime)
    {
        _availableCharacters.ForEach(c => c.UpdateCharacter(elapseTime));
    }

    public bool AllCharacterInBase()
    {
        foreach (var  character in _availableCharacters)
        {
            if (!character.IsAvailable() && !character.IsResting()) return false;
        }

        return true;
    }

    public List<CharacterUnit> Characters => _availableCharacters;
}
