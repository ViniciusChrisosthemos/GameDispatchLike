using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionUnit
{
    public enum MissionStatus
    {
        WaitingToBeAccepted,
        InProgress,
        Completed,
        Claimed,
        Lost
    }

    private MissionSO _missionSO;
    private MissionStatus _missionStatus;
    private float _startTime;
    private Team _currentTeam;

    public MissionUnit(MissionSO missionSO, float startTime)
    {
        _missionSO = missionSO;

        _missionStatus = MissionStatus.WaitingToBeAccepted;
        _startTime = startTime;
    }

    public void StartMission(Team team, float startTime)
    {
        _currentTeam = team;
        _missionStatus = MissionStatus.InProgress;
        _startTime = startTime;
    }

    public bool IsMissionWaitingToBeAccepted()
    {
        return _missionStatus == MissionStatus.WaitingToBeAccepted;
    }

    public bool IsMissionInProgress()
    {
        return _missionStatus == MissionStatus.InProgress;
    }

    public bool IsMissionClaimed()
    {
        return _missionStatus == MissionStatus.Claimed;
    }

    public bool IsMissionComplete(float currentTime)
    {
        if (!IsMissionInProgress())
            return false;
        
        return (currentTime - _startTime) >= _missionSO.TimeToComplete;
    }

    public bool IsMissionAvailable(float currentTime)
    {
        if (!IsMissionWaitingToBeAccepted())
            return false;
        
        return (currentTime - _startTime) >= _missionSO.TimeToAccept;
    }

    public bool IsMissionLost()
    {
        return _missionStatus == MissionStatus.Lost;
    }

    public void LostMission()
    {
        _missionStatus = MissionStatus.Lost;
    }

    public void CompleteMission()
    {
        _missionStatus = MissionStatus.Completed;
    }

    public void ClaimMission()
    {
        _missionStatus = MissionStatus.Claimed;
    }
}
