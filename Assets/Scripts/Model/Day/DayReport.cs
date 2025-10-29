using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayReport
{
    public int MissionsAccepted { get; private set; }
    public int MissionLosted { get; private set; }
    public int MissionSucceded { get; private set; }
    public int MissionFailed { get; private set; }
    public int TotalGoldGained { get; private set; }
    public int TotalReputationGained { get; private set; }

    private Dictionary<CharacterUnit, int> MissionAcceptedPerCharacters;

    public DayReport()
    {
        MissionsAccepted = 0;
        MissionLosted = 0;
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

    public void HandleMissionLost()
    {
        MissionFailed++;
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
}
