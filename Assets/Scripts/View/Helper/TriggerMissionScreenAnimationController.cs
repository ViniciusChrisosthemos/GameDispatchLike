using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMissionScreenAnimationController : MonoBehaviour
{
    [SerializeField] private UIMissionResultViewController _missionResultViewController;
    [SerializeField] private UIChoiceResultViewController _choiceResultViewController;

    public void TriggerMissionResultAnimation()
    {
        _missionResultViewController.StartAnimateResult();
    }
}
