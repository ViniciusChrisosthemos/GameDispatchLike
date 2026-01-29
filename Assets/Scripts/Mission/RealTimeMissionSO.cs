using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatManager;

[CreateAssetMenu(fileName = "RealTimeMission_Rank", menuName = "ScriptableObjects/Mission/Real Time Mission")]
public class RealTimeMissionSO : AbstractBaseMission
{
    [Header("Parameters")]
    public int TimeToAccept;
    public int TimeToComplete;
    public int TimeToAnswerEvent;

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
}
