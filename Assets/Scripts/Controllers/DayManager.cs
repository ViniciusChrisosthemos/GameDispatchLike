using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DayManager : MonoBehaviour
{
    [SerializeField] private DayCharacterManager _dayCharacterManager;

    [SerializeField] private int _dayDurationInSeconds = 5 * 60;
    [SerializeField] private int _totalMissions = 12;
    [SerializeField] private List<MissionSO> _missionsSO;

    [SerializeField] private List<MissionUnit> _currentMissions;
    [SerializeField] private List<TimelineMission> _timelineMissions;
    private float _elapseTime;
    private int _currentMissionID = 0;

    [Header("Events")]
    public UnityEvent OnDayStart;
    public UnityEvent OnDayEnd;
    public UnityEvent<float> OnTimeUpdated;
    public UnityEvent<List<MissionUnit>> OnMissionAvailable;
    public UnityEvent<List<MissionUnit>> OnMissionCompleted;
    public UnityEvent<List<MissionUnit>> OnMissionLost;

    private void Start()
    {
        StartDay();
    }

    public void StartDay()
    {
        _currentMissions = new List<MissionUnit>();

        _timelineMissions = GetTimelineMissions();
        _currentMissionID = 0;

        StartCoroutine(DayLoopCoroutine());
    }

    private IEnumerator DayLoopCoroutine()
    {
        OnDayStart?.Invoke();

        InvokeRepeating("CheckMissionsStatus", 1, 1);
        _elapseTime = 0f;

        while (true)
        {
            _elapseTime += Time.deltaTime;

            if (_elapseTime >= _dayDurationInSeconds)
            {
                break;
            }
            else
            {
                OnTimeUpdated?.Invoke(_elapseTime);
            }

            yield return null;
        }

        CancelInvoke("CheckMissionsStatus");

        OnDayEnd?.Invoke();
    }

    private void CheckMissionsStatus()
    {
        var indexToRemove = new List<int>();
        var missionToUpdate = new List<MissionUnit>();

        for (int i = 0; i < _timelineMissions.Count; i++)
        {
            var timelineMission = _timelineMissions[i];

            if (_elapseTime >= timelineMission.StartTime)
            {
                var mission = new MissionUnit(_currentMissionID, timelineMission.MissionSO, _elapseTime);

                _currentMissions.Add(mission);
                missionToUpdate.Add(mission);
                indexToRemove.Add(i);

                _currentMissionID++;
            }
            else
            {
                break;
            }
        }

        indexToRemove.ForEach(i => _timelineMissions.RemoveAt(i));

        if (missionToUpdate.Count > 0)
        {
            OnMissionAvailable?.Invoke(missionToUpdate);
            missionToUpdate.Clear();
        }


        indexToRemove = new List<int>();
        var missionLosted = new List<MissionUnit>();
        var missionCompleted = new List<MissionUnit>();

        foreach (var missionUnit in _currentMissions.ToArray())
        {
            missionUnit.UpdateMission(_elapseTime);

            if (missionUnit.IsMissionLost())
            {
                missionLosted.Add(missionUnit);
                _currentMissions.Remove(missionUnit);
            }
            else if (missionUnit.IsMissionCompleted())
            {
                missionCompleted.Add(missionUnit);
                _currentMissions.Remove(missionUnit);
            }
            else if (missionUnit.IsMissionClaimed())
            {
                _currentMissions.Remove(missionUnit);
            }
        }

        if (missionLosted.Count > 0) OnMissionLost?.Invoke(missionLosted);
        if (missionCompleted.Count > 0) OnMissionCompleted?.Invoke(missionCompleted);
    }

    public void ClaimMission(MissionUnit missionUnit)
    {
        missionUnit.ClaimMission();
    }

    private List<TimelineMission> GetTimelineMissions()
    {
        var timelineMissions = new List<TimelineMission>();

        for (int i = 0; i < _totalMissions; i++)
        {
            var missionSO = _missionsSO[Random.Range(0, _missionsSO.Count)];
            var startTime = Random.Range(0, _dayDurationInSeconds);
            timelineMissions.Add(new TimelineMission(missionSO, startTime));
        }

        timelineMissions.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));

        return timelineMissions;
    }

    public void AcceptMission(MissionUnit mission, Team team)
    {
        _dayCharacterManager.HandleTeamInMission(team);
        mission.StartMission(team, _elapseTime);
    }

    public int TotalDayTime { get => _dayDurationInSeconds; }

    private class TimelineMission
    {
        public MissionSO MissionSO;
        public float StartTime;

        public TimelineMission(MissionSO missionSO, float startTime)
        {
            MissionSO = missionSO;
            StartTime = startTime;
        }   
    }
}
