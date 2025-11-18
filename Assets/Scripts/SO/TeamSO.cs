using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Team", menuName = "ScriptableObjects/Character/Team")]
public class TeamSO : ScriptableObject
{
    public List<CharacterSO> Characters;

    public Team GetTeam()
    {
        var team = new Team(Characters.Count);

        Characters.ForEach(c => team.AddMember(new CharacterUnit(c)));

        return team;
    }
}
