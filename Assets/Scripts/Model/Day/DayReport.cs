using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayReport
{
    public int MissionsAccepted { get; private set; }
    public int MissionMisses { get; private set; }
    public int MissionSucceded { get; private set; }
    public int MissionFailed { get; private set; }
    public int TotalGoldGained { get; private set; }
    public int TotalReputationGained { get; private set; }
    public int TotalCalls { get; private set; }

    private Dictionary<CharacterUnit, int> MissionAcceptedPerCharacters;

    public int LevelGained { get; private set; }
    public float LevelPercLostByFail { get; private set; }
    public float LevelPercLostByMissed { get; private set; }

    public DayReport()
    {
        TotalCalls = 0;
        MissionsAccepted = 0;
        MissionMisses = 0;
        MissionSucceded = 0;
        MissionFailed = 0;
        TotalGoldGained = 0;
        TotalReputationGained = 0;
        MissionAcceptedPerCharacters = new Dictionary<CharacterUnit, int>();
    }

    public void HandleMissionAccepted(List<CharacterUnit> characters)
    {
        MissionsAccepted++;

        foreach (var character in characters)
        {
            if (!MissionAcceptedPerCharacters.ContainsKey(character))
            {
                MissionAcceptedPerCharacters[character] = 0;
            }

            MissionAcceptedPerCharacters[character] += 1;
        }
    }

    public void HandleMissionMiss()
    {
        MissionMisses++;
    }

    public void HandleMissionSucceded(int gold)
    {
        MissionSucceded++;
        TotalGoldGained += gold;
    }
    
    public void HandleMissionFailed()
    {
        MissionFailed++;
    }

    public void AddCalls(int callAmount)
    {
        TotalCalls += callAmount;
    }
}
