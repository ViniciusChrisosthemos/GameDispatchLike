using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DayManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DayCharacterManager _dayCharacterManager;
    [SerializeField] private DayMissionManager _missionManager;

    [Header("Events")]
    public UnityEvent<List<CharacterUnit>> OnDayStart;
    public UnityEvent<DayReport> OnDayEnd;
    public UnityEvent<float> OnTimeUpdated;
    public UnityEvent<List<MissionUnit>> OnMissionAvailable;

    [Header("Day Parameters")]
    [SerializeField] private List<DaySO> _daySOs;
    [SerializeField] private DaySO _defaultDaySO;

    [Header("Events")]
    public UnityEvent<bool> OnGamePausedChange;

    [Header("Debug")]
    [SerializeField] private bool _runOnStart = false;

    private float _elapseTime;
    private bool _isPaused = false;
    private DayReport _dayReport;

    private GameState _gameState;

    private void Start()
    {
        _missionManager.OnMissionMiss.AddListener(HandleMissionMiss);

        StartDay();
    }

    public void StartDay()
    {
        _isPaused = false;
        _dayReport = new DayReport();
        _gameState = GameManager.Instance.GameState;

        var day = _gameState.Day;
        var characters = _gameState.Guild.ScheduledCharacters;

        characters.ForEach(c => c.SetStatusToAvailable());


        DaySO daySO = day >= _daySOs.Count ? _defaultDaySO : _daySOs[day-1];
        var missionAmount = daySO.UseAllMissions ? daySO.MissionSOs.Count : daySO.MissionAmount;

        _dayCharacterManager.Init(characters);

        Debug.Log($"{daySO.name} {daySO.MissionSOs.Count} {daySO.MissionAmount} {daySO.DayDurationInSeconds} {missionAmount}");
        _missionManager.Init(daySO.MissionSOs, missionAmount, daySO.DayDurationInSeconds, HandleNewMissions);

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

    private void HandleMissionMiss(MissionUnit missionUnit)
    {
        _dayReport.HandleMissionMiss();
    }

    public void StartMission(MissionUnit mission)
    {
        mission.StartMission(_elapseTime);
        _dayCharacterManager.HandleTeamStartMission(mission.Team);
    }

    public void PauseDay()
    {
        _isPaused = true;

        OnGamePausedChange?.Invoke(_isPaused);
    }

    public void ResumeDay()
    {
        _isPaused = false;

        OnGamePausedChange?.Invoke(_isPaused);
    }

    private void HandleNewMissions(List<MissionUnit> newMissions)
    {
        OnMissionAvailable?.Invoke(newMissions);

        _dayReport.AddCalls(newMissions.Count);
    }

    public void HandleCharacterArriveBase(Team team, float currentTime)
    {
        team.Members.ForEach(m => m.SetCharacterResting(currentTime));
    }

    public float CurrentTime => _elapseTime;
}
