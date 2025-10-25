using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
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

    [SerializeField] private int _id;
    [SerializeField] private MissionSO _missionSO;
    [SerializeField] private MissionStatus _missionStatus;
    [SerializeField] private float _startTime;
    [SerializeField] private Team _currentTeam;

    public MissionUnit(int id, MissionSO missionSO, float startTime)
    {
        _id = id;
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

    public void UpdateMission(float currentTime)
    {
        if (_missionStatus == MissionStatus.WaitingToBeAccepted)
        {
            if (currentTime - _startTime >= _missionSO.TimeToAccept)
            {
                _missionStatus = MissionStatus.Lost;
            }
        }
        else if (_missionStatus == MissionStatus.InProgress)
        {
            if (currentTime - _startTime >= _missionSO.TimeToComplete)
            {
                _missionStatus = MissionStatus.Completed;
            }
        }
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
    public bool IsMissionCompleted()
    {
        return _missionStatus == MissionStatus.Completed;
    }

    public float GetTotalTimeFromGetMission(float currentTime) => (currentTime - _startTime) / _missionSO.TimeToAccept;

    public float GetTotalTimeFromAcceptMission(float currentTime) => (currentTime - _startTime) / _missionSO.TimeToComplete;


    public string Name => _missionSO.Name;
    public string Description => _missionSO.Description;
    public int Exp => _missionSO.RewardExperience;
    public int Gold => _missionSO.RewardExperience;
    public int MaxTeamSize => _missionSO.MaxTeamSize;
    public int ID => _id;
}
