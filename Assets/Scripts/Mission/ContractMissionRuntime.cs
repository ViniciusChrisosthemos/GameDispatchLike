using UnityEngine;

public class ContractMissionRuntime
{
    private ContractMisionSO _contractData;
    private Team _teamAssigned;

    private int _receivedDay;
    private int _accetedDay;
    private bool _isCompleted;
    private bool _inProgress;
    private bool _isExpired;

    public ContractMissionRuntime(ContractMisionSO contractData)
    {
        _contractData = contractData;
        _teamAssigned = new Team(_contractData.MaxTeamSize);

        _isCompleted = false;
        _inProgress = false;
        _isExpired = false;
    }

    public void StartMission(Team team, int currentDay)
    {
        _teamAssigned = team;
        _accetedDay = currentDay;

        _isCompleted = false;
        _inProgress = true;
    }

    public void UpdateContract(int currentDay)
    {
        if (_isExpired || _isCompleted) return;

        if (InProgress)
        {
            if (currentDay - _accetedDay >= _contractData.DurationDays)
            {
                _isCompleted = true;
            }
        }
        else
        {
            if (currentDay - _receivedDay >= _contractData.DaysToAccept)
            {
                _isExpired = true;
            }
        }
    }

    public float GetProgress(int currentDay)
    {
        if (_isCompleted) return 1f;

        int daysPassed = currentDay - _accetedDay;
        return Mathf.Clamp01((float)daysPassed / _contractData.DurationDays);
    }

    public Reward GetReward()
    {
        return new Reward(_contractData.RewardGold, _contractData.RewardExperience);
    }

    public Team TeamAssigned => _teamAssigned;

    public bool IsCompleted => _isCompleted;
    public bool InProgress => _inProgress;
    public bool IsExpired => _isExpired;

    public ContractMisionSO ContractMisionSO => _contractData;
}
