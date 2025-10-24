using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDayManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DayManager _dayManager;

    [Header("UI/Missions")]
    [SerializeField] private Transform _missionParent;
    [SerializeField] private UIMissionController _missionPrefab;

    [Header("UI/Day Timer")]
    [SerializeField] private Slider _sliderDayTimer;

    private List<UIMissionController> _uiMissionControllers;

    private void Awake()
    {
        _dayManager.OnMissionAvailable.AddListener(HandleMissionAvailableEvent);
        _dayManager.OnTimeUpdated.AddListener(HandleTimeUpdatedEvet);
        _dayManager.OnMissionLost.AddListener(HandleMissionLostEvent);
    }

    private void Start()
    {
        _missionParent.ClearChilds();

        _uiMissionControllers = new List<UIMissionController>();

        _sliderDayTimer.value = 1;
    }

    private void HandleTimeUpdatedEvet(float currentTime)
    {
        _uiMissionControllers.ForEach(controller => controller.UpdateTime(currentTime));

        _sliderDayTimer.value = 1 - (currentTime / _dayManager.TotalDayTime);
    }

    private void HandleMissionLostEvent(List<MissionUnit> missions)
    {
        foreach (var mission in missions)
        {
            var controllerIndex = _uiMissionControllers.FindIndex(c => c.MissionUnit.ID == mission.ID);
            var controller = _uiMissionControllers[controllerIndex];

            _uiMissionControllers.RemoveAt(controllerIndex);
            Destroy(controller.gameObject);
        }
    }

    private void HandleMissionAvailableEvent(List<MissionUnit> missions)
    {
        foreach(var mission in missions)
        {
            var instance = Instantiate(_missionPrefab, _missionParent);

            instance.Init(mission);

            _uiMissionControllers.Add(instance);
        }
    }
}
