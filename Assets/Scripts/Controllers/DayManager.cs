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

    private List<MissionUnit> _currentMissions;
    private List<TimelineMission> _timelineMissions;
    private float _startTime;

    [Header("Events")]
    public UnityEvent OnDayStart;
    public UnityEvent OnDayEnd;
    public UnityEvent<float> OnTimeUpdated;
    public UnityEvent<MissionUnit> OnMissionAvailable;
    public UnityEvent<MissionUnit> OnMissionCompleted;
    public UnityEvent<MissionUnit> OnMissionLost;

    public void StartDay()
    {
        _currentMissions = new List<MissionUnit>();

        _timelineMissions = GetTimelineMissions();
        _startTime = Time.time;

        StartCoroutine(DayLoopCoroutine());
    }

    private IEnumerator DayLoopCoroutine()
    {
        OnDayStart?.Invoke();

        while (true)
        {
            var elapsedTime = Time.time - _startTime;

            var indexToRemove = new List<int>();
            for(int i = 0; i < _timelineMissions.Count; i++)
            {
                var timelineMission = _timelineMissions[i];

                if (elapsedTime >= timelineMission.StartTime)
                {
                    var mission = new MissionUnit(timelineMission.MissionSO, elapsedTime);
                    
                    _currentMissions.Add(mission);
                    indexToRemove.Add(i);

                    OnMissionAvailable?.Invoke(mission);
                }
                else
                {
                    break;
                }
            }

            indexToRemove.ForEach(i => _timelineMissions.RemoveAt(i));

            indexToRemove.Clear();
            for(int i = 0; i < _currentMissions.Count; i++)
            {
                var missionUnit = _currentMissions[i];

                if (missionUnit.IsMissionWaitingToBeAccepted())
                {
                    if (!missionUnit.IsMissionAvailable(elapsedTime))
                    {
                        indexToRemove.Add(i);

                        missionUnit.LostMission();

                        OnMissionLost?.Invoke(missionUnit);
                    }
                }
                else if (missionUnit.IsMissionInProgress())
                {
                    if (missionUnit.IsMissionComplete(elapsedTime))
                    {
                        missionUnit.CompleteMission();

                        OnMissionCompleted?.Invoke(missionUnit);
                    }
                }
                else if (missionUnit.IsMissionClaimed())
                {
                    indexToRemove.Add(i);
                }
            }

            if (elapsedTime >= _dayDurationInSeconds)
            {
                break;
            }
            else
            {
                OnTimeUpdated?.Invoke(elapsedTime);
            }

            yield return null;
        }

        OnDayEnd?.Invoke();
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

    public void AcceptMission(Team team)
    {

    }

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
