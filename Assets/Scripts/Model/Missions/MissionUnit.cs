using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class MissionUnit
{
    public enum MissionStatus
    {
        WaitingToBeAccepted,
        Accepted,
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

    public UnityEvent<MissionUnit> OnMissionAccepted = new UnityEvent<MissionUnit>();
    public UnityEvent<MissionUnit> OnMissionStarted = new UnityEvent<MissionUnit>();
    public UnityEvent<MissionUnit> OnMissionCompleted = new UnityEvent<MissionUnit>();
    public UnityEvent<MissionUnit> OnMissionLose = new UnityEvent<MissionUnit>();
    public UnityEvent<MissionUnit> OnMissionClaimed = new UnityEvent<MissionUnit>();

    public Transform Location {  get; private set; }

    public MissionUnit(int id, MissionSO missionSO, Transform location, float startTime)
    {
        _id = id;
        _missionSO = missionSO;

        _missionStatus = MissionStatus.WaitingToBeAccepted;
        _startTime = startTime;

        Location = location;
    }

    public void StartMission(float startTime)
    {
        _missionStatus = MissionStatus.InProgress;
        _startTime = startTime;

        OnMissionStarted?.Invoke(this);
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

    public bool IsAccepted()
    {
        return _missionStatus == MissionStatus.Accepted;
    }

    public void UpdateMission(float currentTime)
    {
        if (_missionStatus == MissionStatus.WaitingToBeAccepted)
        {
            if (currentTime - _startTime >= _missionSO.TimeToAccept)
            {
                _missionStatus = MissionStatus.Lost;

                OnMissionLose?.Invoke(this);
            }
        }
        else if (_missionStatus == MissionStatus.InProgress)
        {
            if (currentTime - _startTime >= _missionSO.TimeToComplete)
            {
                _missionStatus = MissionStatus.Completed;

                OnMissionCompleted?.Invoke(this);
            }
        }
    }

    public bool IsMissionLost()
    {
        return _missionStatus == MissionStatus.Lost;
    }

    public void ClaimMission()
    {
        _missionStatus = MissionStatus.Claimed;

        OnMissionClaimed?.Invoke(this);
    }
    public bool IsMissionCompleted()
    {
        return _missionStatus == MissionStatus.Completed;
    }

    public float GetTotalTimeFromGetMission(float currentTime) => (currentTime - _startTime) / _missionSO.TimeToAccept;

    public float GetTotalTimeFromAcceptMission(float currentTime) => (currentTime - _startTime) / _missionSO.TimeToComplete;

    public void AcceptMission(Team team)
    {
        _currentTeam = team;
        _missionStatus = MissionStatus.Accepted;

        OnMissionAccepted?.Invoke(this);
    }

    public StatManager GetRequiredStats()
    {
        return _missionSO.RequiredStats;
    }

    public string Name => _missionSO.Name;
    public string Description => _missionSO.Description;
    public int Exp => _missionSO.RewardExperience;
    public int Gold => _missionSO.RewardExperience;
    public int Reputation => _missionSO.RewardReputation;
    public int MaxTeamSize => _missionSO.MaxTeamSize;
    public int ID => _id;

    public Team Team => _currentTeam;
}
