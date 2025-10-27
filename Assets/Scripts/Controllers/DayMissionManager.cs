using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class DayMissionManager : MonoBehaviour
{

    [SerializeField] private List<MissionInfo> _missionsInfo;

    [SerializeField] private List<MissionUnit> _currentMissions;
    [SerializeField] private List<TimelineMission> _timelineMissions;

    [Header("Events")]
    [SerializeField] private UnityEvent<List<MissionUnit>> OnNewMissionAvailable;

    private int _currentMissionID;

    public void Init(int missionAmount, int totalTimeInSeconds, Action<List<MissionUnit>> OnMissionAvailable)
    {
        _currentMissionID = 0;
        _timelineMissions = new List<TimelineMission>();
        
        var auxMissionInfoList = new List<TimelineMission>();

        foreach (var missionInfo in  _missionsInfo)
        {
            foreach (var location in missionInfo.PossiblePositions)
            {
                var startTime = Random.Range(0, totalTimeInSeconds);

                var timelineMission = new TimelineMission(missionInfo.MissionSO, location, startTime);

                auxMissionInfoList.Add(timelineMission);
            }
        }

        auxMissionInfoList.Shuffle();

        for (int i = 0; i < missionAmount; i++)
        {
            if (auxMissionInfoList.Count == 0) break;

            var index = Random.Range(0, auxMissionInfoList.Count);

            _timelineMissions.Add(auxMissionInfoList[index]);

            auxMissionInfoList.RemoveAt(index);
        }

        _timelineMissions.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));

        OnNewMissionAvailable.AddListener(newMissions => OnMissionAvailable?.Invoke(newMissions));
    }

    public void UpdateMissions(float currentTime)
    {
        var missionToUpdate = new List<MissionUnit>();

        foreach (var timelineMission in _timelineMissions.ToArray())
        {
            if (currentTime >= timelineMission.StartTime)
            {
                var mission = new MissionUnit(_currentMissionID, timelineMission.MissionSO, timelineMission.Location, currentTime);

                mission.OnMissionLose.AddListener(HandleMissionLostOrClaimed);
                mission.OnMissionClaimed.AddListener(HandleMissionLostOrClaimed);

                _currentMissions.Add(mission);
                missionToUpdate.Add(mission);

                _timelineMissions.Remove(timelineMission);

                _currentMissionID++;
            }
            else
            {
                break;
            }
        }

        if (missionToUpdate.Count > 0)
        {
            OnNewMissionAvailable?.Invoke(missionToUpdate);
        }

        foreach (var missionUnit in _currentMissions.ToArray())
        {
            missionUnit.UpdateMission(currentTime);
        }
    }
    
    private void HandleMissionLostOrClaimed(MissionUnit missionUnit)
    {
        _currentMissions.Remove(missionUnit);
    }

    public bool HasMissions() => _timelineMissions.Count + _currentMissions.Count > 0;

    private class TimelineMission
    {
        public MissionSO MissionSO;
        public Transform Location;
        public float StartTime;

        public TimelineMission(MissionSO missionSO, Transform location, float startTime)
        {
            MissionSO = missionSO;
            Location = location;
            StartTime = startTime;
        }
    }

    [Serializable]
    public class MissionInfo
    {
        public MissionSO MissionSO;
        public List<Transform> PossiblePositions;
    }
}
