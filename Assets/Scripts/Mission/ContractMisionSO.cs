using UnityEngine;

[CreateAssetMenu(fileName = "ContractMission_Rank", menuName = "ScriptableObjects/Mission/Contract Mission")]
public class ContractMisionSO : AbstractBaseMission
{
    [Header("Parameters")]
    public int DurationDays;
    public int DaysToAccept;
}
