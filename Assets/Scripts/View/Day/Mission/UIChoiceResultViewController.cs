using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
    [SerializeField] private GameObject _successView;
    [SerializeField] private GameObject _failView;
    [SerializeField] private TextMeshProUGUI _txtTitle;

    [Header("Stat Comparison References")]
    [SerializeField] private UIRadarChartController _choiceResultExpectedStatUIRadarController;
    [SerializeField] private UIRadarChartController _choiceResultGivenStatUIRadarController;
    [SerializeField] private Image _imgRadarMask;
    [SerializeField] private List<RadarMask> _radarMasks;

    [Header("Character Choice References")]
    [SerializeField] private Image _imgCharacterChoiceArt;
    [SerializeField] private Image _imgCharacterFaceChoiceArt;

    [Header("Events")]
    public UnityEvent OnChoiceResultFailEvent;
    public UnityEvent OnChoiceResultSuccessEvent;

    private Action<bool> _callback;
    private bool _choiceResult;

    private List<float> _expectedValues;
    private List<float> _givenValues;
    private List<bool> _mask;

    private void Start()
    {
        Close();
    }

    public void OpenScreen(MissionUnit missionUnit, RandomMissionEvent missionEvent, MissionChoice missionChoice, Action<bool> callback)
    {
        _view.SetActive(true);
        _successView.SetActive(false);
        _failView.SetActive(false);

        _txtTitle.text = missionUnit.Name;

        _btnOk.onClick.RemoveAllListeners();

        if (missionChoice.Character == null)
        {
            _statComparisonView.SetActive(true);
            _characterChoiceView.SetActive(false);

            _expectedValues = new List<float>();
            _givenValues = new List<float>();
            var totalStatAmount = Enum.GetValues(typeof(StatType)).Length;

            var expectedStatValue = missionChoice.StatAmountRequired;
            var givenStatValue = missionUnit.Team.GetStat(missionChoice.Requirement.StatType).GetValue();

            for (int i = 0; i < totalStatAmount; i++)
            {
                _expectedValues.Add(expectedStatValue / 10f);
                _givenValues.Add(givenStatValue / 10f);
            }

            _mask = new List<bool>();

            foreach (StatType statType in Enum.GetValues(typeof(StatType)))
            {
                _mask.Add(statType == missionChoice.Requirement.StatType);
            }

            _choiceResultExpectedStatUIRadarController.UpdateStats(_expectedValues, _mask);
            _choiceResultGivenStatUIRadarController.UpdateStats(_givenValues, _mask);

            var srpiteMask = _radarMasks.Find(m => m.Type == missionChoice.Requirement.StatType);
            _imgRadarMask.sprite = srpiteMask.Mask;

            _successView.SetActive(givenStatValue >= expectedStatValue);
            _failView.SetActive(!_successView.activeSelf);

            _btnOk.onClick.AddListener(() => TriggerCallback(givenStatValue >= expectedStatValue));

            _choiceResult = givenStatValue >= expectedStatValue;
        }
        else
        {
            _statComparisonView.SetActive(false);
            _characterChoiceView.SetActive(true);

            _imgCharacterChoiceArt.sprite = missionChoice.Character.FullArt;
            _imgCharacterFaceChoiceArt.sprite = missionChoice.Character.FaceArt;

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

    public void TriggerChoiceResultEvent()
    {
        if (_choiceResult)
        {
            OnChoiceResultSuccessEvent?.Invoke();
        }
        else
        {
            OnChoiceResultFailEvent?.Invoke();
        }
    }

    public void UpdateRadarCharts()
    {
        _choiceResultExpectedStatUIRadarController.UpdateStats(_expectedValues, _mask);
        _choiceResultGivenStatUIRadarController.UpdateStats(_givenValues, _mask);
    }

    [Serializable]
    public class RadarMask
    {
        public StatType Type;
        public Sprite Mask;
    }
}
