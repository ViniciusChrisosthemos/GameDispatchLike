using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DayCharacterManager : MonoBehaviour
{
    [SerializeField] private List<CharacterSO> _characterSO;
    [SerializeField] private List<CharacterUnit> _availableCharacters;

    [Header("Events")]
    public UnityEvent<CharacterUnit> OnCharacterChangeStatus;

    public List<CharacterUnit> Characters => _availableCharacters;

    public void HandleTeamInMission(Team team)
    {
        foreach (CharacterUnit character in team.Members)
        {
            character.SetStatusToInMission();

            OnCharacterChangeStatus?.Invoke(character);
        }
    }

    private void Awake()
    {
        _availableCharacters = new List<CharacterUnit>();

        _characterSO.ForEach(characterSO => _availableCharacters.Add(new CharacterUnit(characterSO)));
    }

    public void HandleTeamCompleteMission(MissionUnit missionUnit, Team team, float currentTime)
    {
        team.Members.ForEach(c => c.HandleMissionCompleted(missionUnit.Exp, currentTime));
    }

    public void UpdateCharacters(float elapseTime)
    {
        _availableCharacters.ForEach(c => c.UpdateCharacter(elapseTime));
    }
}
