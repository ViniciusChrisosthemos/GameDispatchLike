using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Mission")]
public class MissionSO : ScriptableObject
{
    public enum MissionType { Rescure, Attack, Negociation }

    [Header("Mission")]
    public string Name;
    public string Description;
    public string RequirementDescription;
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

    [Serializable]
    public class RandomMissionEvent
    {
        public string Description;
        public List<MissionChoice> MissionChoices;
    }

    [Serializable]
    public class MissionChoice
    {
        [Header("Choice Info")]
        public string Description;
        public StatManager.StatType StatType;
        public int StatAmountRequired;
        public string SucceededDescription;
        public string FailedDescription;

        [Header("Hero Required (optional)")]
        public CharacterSO Character;
    }
}
