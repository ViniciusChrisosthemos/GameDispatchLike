using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DayMissionManager : MonoBehaviour
{
    [SerializeField] private List<Transform> _availableNodes;
    [SerializeField] private List<MissionUnit> _currentMissions;
    [SerializeField] private List<TimelineMission> _timelineMissions;
    [SerializeField] private MissionTimelineGenerator _missionTimeLineGenerator;

    [Header("Events")]
    public UnityEvent<List<MissionUnit>> OnNewMissionAvailable;
    public UnityEvent<MissionUnit> OnMissionMiss;

    private int _currentMissionID;

    public void Init(List<MissionSO> missions, int missionAmount, int totalTimeInSeconds, Action<List<MissionUnit>> OnMissionAvailable)
    {
        _currentMissionID = 0;
        _currentMissions = new List<MissionUnit>();
        _timelineMissions = new List<TimelineMission>();

        var missioEntries = _missionTimeLineGenerator.GenerateTimeline();

        var possibleLocations = new List<Transform>();
        foreach(var missionEntry in missioEntries)
        {
            if (_timelineMissions.Count >= missionAmount) break;

            if (missionEntry.missionIndex >= missions.Count) break;

            var missionSO = missions[missionEntry.missionIndex];
            
            if (possibleLocations.Count == 0)
            {
                possibleLocations = _availableNodes.ToList();
                possibleLocations.Shuffle();
            }

            var location = possibleLocations[0];
            possibleLocations.RemoveAt(0);

            var time = missionEntry.time;
            var timelineMission = new TimelineMission(missionSO, location, time);

            _timelineMissions.Add(timelineMission);
        }

        /*
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
        */

        OnNewMissionAvailable.RemoveAllListeners();
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

                mission.OnMissioMiss.AddListener(HandleMissionMiss);
                mission.OnMissionClaimed.AddListener(HandleMissionClaimed);

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

    private void HandleMissionMiss(MissionUnit missionUnit)
    {
        _currentMissions.Remove(missionUnit);
        OnMissionMiss?.Invoke(missionUnit);
    }

    private void HandleMissionClaimed(MissionUnit missionUnit)
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
