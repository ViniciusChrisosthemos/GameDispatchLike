using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "ScriptableObjects/Mission")]
public class MissionSO : ScriptableObject
{
    public string Name;
    public string Description;
    public int DifficultyLevel;
    public StatManager RequiredStats;
    public int MaxTeamSize;
    public Sprite MissionImage;
    public int RewardExperience;
    public int RewardGold;
    public int TimeToAccept;
    public int TimeToComplete;
}
