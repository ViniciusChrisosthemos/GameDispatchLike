using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static MissionSO;
using static StatManager;

public class UIMissionEventController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameObject _view;
    [SerializeField] private UIChoiceViewController _choiceViewControllerPrefab;

    [Header("Mission Info")]
    [SerializeField] private TextMeshProUGUI _txtMissionType;
    [SerializeField] private TextMeshProUGUI _txtMissionDescription;

    [Header("Screen Views")]
    [SerializeField] private GameObject _selectChoiceView;
    [SerializeField] private GameObject _choiceMadeView;

    [Header("Select Choice / Middle")]
    [SerializeField] private TextMeshProUGUI _txtEventTitle;
    [SerializeField] private TextMeshProUGUI _txtEventDescription;
    [SerializeField] private Transform _middleChoiceParent;

    [Header("Select Choice / Right")]
    [SerializeField] private UIRadarChartController _selectChoiceUIRadarController;

    [Header("Choice Result / Middle / Stat Comparison")]
    [SerializeField] private GameObject _statComparisonView;
    [SerializeField] private UIRadarChartController _choiceResultExpectedStatUIRadarController;
    [SerializeField] private UIRadarChartController _choiceResultGivenStatUIRadarController;
    [SerializeField] private Image _imgRadarMask;
    [SerializeField] private List<RadarMask> _radarMasks;
    [SerializeField] private Button _btnOk;

    [Header("Choice Result / Middle / Character Choice")]
    [SerializeField] private GameObject _characterChoiceView;
    [SerializeField] private Image _imgCharacterChoiceArt;

    [Header("Choice Result / Right")]
    [SerializeField] private Transform _choiceResultChoiceParent;

    private Action<bool> _callback;
    private MissionUnit _missionUnit;
    private MissionChoice _missionChoice;
    private RandomMissionEvent _missionEvent;

    private void Start()
    {
        CloseScreen();
    }

    public void OpenSelectChoiceScreen(MissionUnit missionUnit, RandomMissionEvent missionEvent, Action<bool> callback)
    {
        _view.SetActive(true);
        _selectChoiceView.SetActive(true);
        _choiceMadeView.SetActive(false);

        _txtMissionType.text = missionUnit.MissionSO.Type.ToString();
        _txtMissionDescription.text = missionUnit.Description;

        _txtEventTitle.text = missionEvent.Title;
        _txtEventDescription.text = missionEvent.Description;

        _middleChoiceParent.ClearChilds();

        foreach (MissionChoice choice in missionEvent.MissionChoices)
        {
            var controller = Instantiate(_choiceViewControllerPrefab, _middleChoiceParent);

            controller.Init(choice, HandleSelectChoice);
        }

        _selectChoiceUIRadarController.UpdateStats(missionUnit.Team.GetTeamStats().GetValues());

        _missionEvent = missionEvent;
        _missionUnit = missionUnit;
        _callback = callback;
    }

    private void HandleSelectChoice(UIItemController controller)
    {
        _missionChoice = controller.GetItem<MissionChoice>();

        OpenChoiceResultScreen();
    }

    public void OpenChoiceResultScreen()
    {
        _selectChoiceView.SetActive(false);
        _choiceMadeView.SetActive(true);

        _btnOk.onClick.RemoveAllListeners();

        _choiceResultChoiceParent.ClearChilds();

        foreach (MissionChoice choice in _missionEvent.MissionChoices)
        {
            var controller = Instantiate(_choiceViewControllerPrefab, _choiceResultChoiceParent);

            controller.Init(choice, null);
            controller.SetUnavailableOverlay(choice != _missionChoice);
        }

        if (_missionChoice.Character == null)
        {
            _statComparisonView.SetActive(true);
            _characterChoiceView.SetActive(false);

            var expectedValues = new List<float>();
            var givenValues = new List<float>();
            var totalStatAmount = Enum.GetValues(typeof(StatType)).Length;

            var expectedStatValue = _missionChoice.StatAmountRequired;
            var givenStatValue = _missionUnit.Team.GetStat(_missionChoice.StatType).GetValue();

            for (int i = 0; i < totalStatAmount; i++)
            {
                expectedValues.Add(expectedStatValue / 10f);
                givenValues.Add(givenStatValue / 10f);
            }

            _choiceResultExpectedStatUIRadarController.UpdateStats(expectedValues);
            _choiceResultGivenStatUIRadarController.UpdateStats(givenValues);

            var mask = _radarMasks.Find(m => m.Type == _missionChoice.StatType);
            _imgRadarMask.sprite = mask.Mask;

            _btnOk.onClick.AddListener(() => TriggerCallback(givenStatValue >= expectedStatValue));
        }
        else
        {
            _statComparisonView.SetActive(false);
            _characterChoiceView.SetActive(true);

            _imgCharacterChoiceArt.sprite = _missionChoice.Character.FullArt;

            _btnOk.onClick.AddListener(() => TriggerCallback(true));
        }
    }

    private void TriggerCallback(bool result)
    {
        _callback?.Invoke(result);
        CloseScreen();
    }

    private void CloseScreen()
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
