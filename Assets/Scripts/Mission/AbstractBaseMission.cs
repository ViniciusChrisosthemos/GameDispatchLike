using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatManager;

public abstract class AbstractBaseMission : ScriptableObject
{
    public enum MissionType
    {
        Rescure, Attack, Negociation, Assault, Investigation, Pursuit, Minor_Inconvenience,
        Social_Event, Security, Red_Ring
    }

    [Header("Mission Data")]
    public string Name;
    public string Description;
    public RankSO Rank;
    public List<MissionRequirement> RequirementDescriptionItems;
    public StatManager RequiredStats;
    public int MaxTeamSize;
    public int RewardExperience;
    public int RewardGold;
    public int RewardReputation;

    [Header("Addiciona Info")]
    public Sprite ClientArt;
    public Sprite EnvironmentArt;
    public MissionType Type;

    [Serializable]
    public class MissionRequirement
    {
        public string Description;
        public StatType StatType;
    }
}
