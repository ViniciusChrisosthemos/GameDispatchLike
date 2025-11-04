using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMissionResultController : MonoBehaviour
{
    [SerializeField] private UIMissionResultViewController _missionResultViewController;
    
    public void TriggerAnimation()
    {
        _missionResultViewController.StartAnimateResult();
    }
}
