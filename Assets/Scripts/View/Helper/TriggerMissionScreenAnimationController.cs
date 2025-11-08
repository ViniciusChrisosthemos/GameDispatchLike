using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMissionScreenAnimationController : MonoBehaviour
{
    [SerializeField] private UIMissionResultViewController _missionResultViewController;
    [SerializeField] private UIChoiceResultViewController _choiceResultViewController;
    [SerializeField] private UIAssignedHeroStatViewController _assignedHeroStatViewController;

    public void TriggerMissionResultAnimation()
    {
        _missionResultViewController.StartAnimateResult();
    }

    public void TriggerChoiceResultEvent()
    {
        _choiceResultViewController.TriggerChoiceResultEvent();
    }

    public void TriggerAssignedHeroEvent()
    {
        _assignedHeroStatViewController.UpdateStats();
    }

    public void TriggerChoiceResultRadarEvent()
    {
        _choiceResultViewController.UpdateRadarCharts();
    }
}
