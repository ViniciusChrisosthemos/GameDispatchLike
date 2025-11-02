using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MissionSO;
using static StatManager;

public class UIChoiceResultViewController : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private GameObject _view;
    [SerializeField] private GameObject _statComparisonView;
    [SerializeField] private GameObject _characterChoiceView;
    [SerializeField] private Button _btnOk;

    [Header("Stat Comparison References")]
    [SerializeField] private UIRadarChartController _choiceResultExpectedStatUIRadarController;
    [SerializeField] private UIRadarChartController _choiceResultGivenStatUIRadarController;
    [SerializeField] private Image _imgRadarMask;
    [SerializeField] private List<RadarMask> _radarMasks;

    [Header("Character Choice References")]
    [SerializeField] private Image _imgCharacterChoiceArt;

    private Action<bool> _callback;

    private void Start()
    {
        Close();
    }

    public void OpenScreen(MissionUnit missionUnit, RandomMissionEvent missionEvent, MissionChoice missionChoice, Action<bool> callback)
    {
        _view.SetActive(true);

        _btnOk.onClick.RemoveAllListeners();

        if (missionChoice.Character == null)
        {
            _statComparisonView.SetActive(true);
            _characterChoiceView.SetActive(false);

            var expectedValues = new List<float>();
            var givenValues = new List<float>();
            var totalStatAmount = Enum.GetValues(typeof(StatType)).Length;

            var expectedStatValue = missionChoice.StatAmountRequired;
            var givenStatValue = missionUnit.Team.GetStat(missionChoice.Requirement.StatType).GetValue();

            for (int i = 0; i < totalStatAmount; i++)
            {
                expectedValues.Add(expectedStatValue / 10f);
                givenValues.Add(givenStatValue / 10f);
            }

            _choiceResultExpectedStatUIRadarController.UpdateStats(expectedValues);
            _choiceResultGivenStatUIRadarController.UpdateStats(givenValues);

            var mask = _radarMasks.Find(m => m.Type == missionChoice.Requirement.StatType);
            _imgRadarMask.sprite = mask.Mask;

            _btnOk.onClick.AddListener(() => TriggerCallback(givenStatValue >= expectedStatValue));
        }
        else
        {
            _statComparisonView.SetActive(false);
            _characterChoiceView.SetActive(true);

            _imgCharacterChoiceArt.sprite = missionChoice.Character.FullArt;

            _btnOk.onClick.AddListener(() => TriggerCallback(true));
        }

        _callback = callback;
    }

    private void TriggerCallback(bool result)
    {
        _callback?.Invoke(result);
    }

    public void Close()
    {
        _view.SetActive(false);
    }

    [Serializable]
    public class RadarMask
    {
        public StatType Type;
        public Sprite Mask;
    }
}
