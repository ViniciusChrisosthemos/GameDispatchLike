using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class DayManager : MonoBehaviour
{
    [SerializeField] private DayCharacterManager _dayCharacterManager;
    [SerializeField] private DayMissionManager _missionManager;

    [SerializeField] private int _dayDurationInSeconds = 5 * 60;
    [SerializeField] private int _totalMissions = 12;

    private float _elapseTime;
    private bool _isPaused = false;

    [Header("Events")]
    public UnityEvent<List<CharacterUnit>> OnDayStart;
    public UnityEvent OnDayEnd;
    public UnityEvent<float> OnTimeUpdated;
    public UnityEvent<List<MissionUnit>> OnMissionAvailable;

    [Header("Debug")]
    [SerializeField] private bool _runOnStart = false;

    private void Start()
    {
        if (_runOnStart) StartDay();
    }

    public void StartDay()
    {
        _isPaused = false;

        _missionManager.Init(_totalMissions, _dayDurationInSeconds, HandleNewMissions);

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
                    UpdateManagers();
                    accumTimeToUpdateMissions = 0f;
                }

                if (!_missionManager.HasMissions())
                {
                    break;
                }
                else
                {
                    OnTimeUpdated?.Invoke(_elapseTime);
                }

            }
        }

        OnDayEnd?.Invoke();
    }

    private void UpdateManagers()
    {
        _missionManager.UpdateMissions(_elapseTime);
        _dayCharacterManager.UpdateCharacters(_elapseTime);
    }

    public void ClaimMission(MissionUnit missionUnit, bool isSuccess)
    {
        missionUnit.ClaimMission();
        _dayCharacterManager.HandleTeamCompleteMission(missionUnit, missionUnit.Team, isSuccess, _elapseTime);
    }

    public void AcceptMission(MissionUnit mission, Team team)
    {
        _dayCharacterManager.HandleTeamAcceptMission(team);

        mission.AcceptMission(team);
    }

    public void StartMission(MissionUnit mission)
    {
        mission.StartMission(_elapseTime);
        _dayCharacterManager.HandleTeamStartMission(mission.Team);
    }

    public void PauseDay()
    {
        _isPaused = true;
    }

    public void ResumoDay()
    {
        _isPaused = false;
    }

    private void HandleNewMissions(List<MissionUnit> newMissions)
    {
        OnMissionAvailable?.Invoke(newMissions);
    }

    public void HandleCharacterArriveBase(Team team, float currentTime)
    {
        team.Members.ForEach(m => m.SetCharacterResting(currentTime));
    }

    internal void HandleCharacterArriveBase(Team team, object currentTime)
    {
        throw new NotImplementedException();
    }

    public int TotalDayTime { get => _dayDurationInSeconds; }
    public float CurrentTime => _elapseTime;
}
