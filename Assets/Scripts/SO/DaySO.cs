using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Day", menuName = "ScriptableObjects/Day")]
public class DaySO : ScriptableObject
{
    public List<RealTimeMissionSO> MissionSOs;
    public bool UseAllMissions;
    public int MissionAmount;
    public int DayDurationInSeconds;

    public List<string> Tips;
}
