using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatManager;

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Mission")]
public class MissionSO : ScriptableObject
{
    public enum MissionType { Rescure, Attack, Negociation, Assault, Investigation, Pursuit, Minor_Inconvenience,
    Social_Event, Security, Red_Ring}

    [Header("Mission")]
    public string Name;
    public string Description;
    public List<MissionRequirement> RequirementDescriptionItems;
    public int DifficultyLevel;
    public StatManager RequiredStats;
    public int MaxTeamSize;
    public int RewardExperience;
    public int RewardGold;
    public int RewardReputation;
    public int TimeToAccept;
    public int TimeToComplete;
    public int TimeToAnswerEvent;

    [Header("Addiciona Info")]
    public Sprite ClientArt;
    public Sprite EnvironmentArt;
    public MissionType Type;

    [Header("Addicional Events")]
    public List<RandomMissionEvent> RandomMissionEvents = new List<RandomMissionEvent>();

    [Header("Battle Events")]
    public List<BattleEvent> BattleEvents = new List<BattleEvent>();

    [Serializable]
    public class BattleEvent
    {
        public List<CharacterSO> EnemyTeam;
    }

    [Serializable]
    public class RandomMissionEvent
    {
        public string Title;
        public string Description;
        public List<MissionChoice> MissionChoices;
    }

    [Serializable]
    public class MissionChoice
    {
        [Header("Choice Info")]
        public MissionRequirement Requirement;
        public int StatAmountRequired;
        public string SucceededDescription;
        public string FailedDescription;

        [Header("Hero Required (optional)")]
        public CharacterSO Character;
    }

    [Serializable]
    public class MissionRequirement
    {
        public string Description;
        public StatType StatType;
    }
}
