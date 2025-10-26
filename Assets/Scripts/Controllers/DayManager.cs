using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

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
    private bool _isPaused = false;

    [Header("Events")]
    public UnityEvent<List<CharacterUnit>> OnDayStart;
    public UnityEvent OnDayEnd;
    public UnityEvent<float> OnTimeUpdated;
    public UnityEvent<List<MissionUnit>> OnMissionAvailable;

    private void Start()
    {
        StartDay();
    }

    public void StartDay()
    {
        _currentMissions = new List<MissionUnit>();

        _timelineMissions = GetTimelineMissions();
        _currentMissionID = 0;
        _isPaused = false;

        StartCoroutine(DayLoopCoroutine());
    }

    private IEnumerator DayLoopCoroutine()
    {
        OnDayStart?.Invoke(_dayCharacterManager.Characters);

        _elapseTime = 0f;
        var accumTimeToUpdateMissions = 0f;

        while (true)
        {
            yield return new WaitForEndOfFrame();

            if (!_isPaused)
            {
                _elapseTime += Time.deltaTime;
                accumTimeToUpdateMissions += Time.deltaTime;

                if (accumTimeToUpdateMissions > 0.5f)
                {
                    CheckMissionsStatus();
                    accumTimeToUpdateMissions = 0f;
                }

                if (_elapseTime >= _dayDurationInSeconds)
                {
                    break;
                }
                else
                {
                    OnTimeUpdated?.Invoke(_elapseTime);
                }

            }
        }

        CancelInvoke("CheckMissionsStatus");

        OnDayEnd?.Invoke();
    }

    private void CheckMissionsStatus()
    {
        var missionToUpdate = new List<MissionUnit>();

        foreach(var timelineMission in _timelineMissions.ToArray())
        {
            if (_elapseTime >= timelineMission.StartTime)
            {
                var mission = new MissionUnit(_currentMissionID, timelineMission.MissionSO, _elapseTime);

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
            OnMissionAvailable?.Invoke(missionToUpdate);
            missionToUpdate.Clear();
        }

        foreach (var missionUnit in _currentMissions.ToArray())
        {
            missionUnit.UpdateMission(_elapseTime);

            if (missionUnit.IsMissionLost() || missionUnit.IsMissionClaimed())
            {
                _currentMissions.Remove(missionUnit);
                
                if (missionUnit.IsMissionClaimed())
                {
                    _dayCharacterManager.HandleTeamCompleteMission(missionUnit, missionUnit.Team, _elapseTime);
                }
            }
        }

        _dayCharacterManager.UpdateCharacters(_elapseTime);
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

    public void PauseDay()
    {
        _isPaused = true;
    }

    public void ResumoDay()
    {
        _isPaused = false;
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
