using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DayStatus
{
    Normal,
    Warning,
    Danger
}

[CreateAssetMenu(fileName = "Day", menuName = "ScriptableObjects/Day")]
public class DaySO : ScriptableObject
{
    public List<MissionSO> MissionSOs;
    public bool UseAllMissions;
    public int MissionAmount;
    public int DayDurationInSeconds;
    public DayStatus Status;
}
