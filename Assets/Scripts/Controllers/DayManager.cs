using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class DayManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DayCharacterManager _dayCharacterManager;
    [SerializeField] private DayMissionManager _missionManager;

    [Header("Parameters")]
    [SerializeField] private int _dayDurationInSeconds = 5 * 60;
    [SerializeField] private int _totalMissions = 12;

    [Header("Events")]
    public UnityEvent<List<CharacterUnit>> OnDayStart;
    public UnityEvent<DayReport> OnDayEnd;
    public UnityEvent<float> OnTimeUpdated;
    public UnityEvent<List<MissionUnit>> OnMissionAvailable;

    [Header("Debug")]
    [SerializeField] private bool _runOnStart = false;

    private float _elapseTime;
    private bool _isPaused = false;
    private DayReport _dayReport;

    private GameState _gameState;

    public void StartDay(List<CharacterUnit> characters)
    {
        _gameState = GameManager.Instance.GameState;

        _isPaused = false;
        _dayReport = new DayReport();

        _dayCharacterManager.Init(characters);
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

        yield return new WaitUntil(() => _dayCharacterManager.AllCharacterInBase());

        OnDayEnd?.Invoke(_dayReport);
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

        if (isSuccess)
        {
            _dayReport.HandleMissionSucceded(missionUnit.Gold);

            _gameState.Guild.AddGold(missionUnit.Gold);
            _gameState.Guild.AddReputation(missionUnit.Reputation);
        }
        else
        {
            _dayReport.HandleMissionFailed();

            _gameState.Guild.RmvReputation(missionUnit.Reputation);
        }
    }

    public void AcceptMission(MissionUnit mission, Team team)
    {
        _dayCharacterManager.HandleTeamAcceptMission(team);

        mission.AcceptMission(team);

        _dayReport.HandleMissionAccepted(team.Members);
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

    public int TotalDayTime { get => _dayDurationInSeconds; }
    public float CurrentTime => _elapseTime;
}
