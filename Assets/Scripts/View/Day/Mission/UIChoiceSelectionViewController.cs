using System;
using System.Linq;
using TMPro;
using UnityEngine;
using static MissionSO;

public class UIChoiceSelectionViewController : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private Transform _choiceParent;
    [SerializeField] private UIChoiceViewController _choiceViewControllerPrefab;

    [Header("(Optional)")]
    [SerializeField] private TextMeshProUGUI _txtMissionName;
    [SerializeField] private TextMeshProUGUI _txtEventDescription;

    private Action<MissionChoice> _choiceSelectionCallback;

    private void Start()
    {
        Close();
    }

    public void OpenScreen(MissionUnit missionUnit, RandomMissionEvent missionEvent, bool showStats, Action<MissionChoice> callback)
    {
        _view.SetActive(true);

        if (_txtMissionName != null) _txtMissionName.text = missionUnit.Name;
        if (_txtEventDescription != null) _txtEventDescription.text = missionEvent.Description;

        _choiceParent.ClearChilds();
        var heros = missionUnit.Team.Members.Select(m => m.BaseCharacterSO).ToList();

        foreach (MissionChoice choice in missionEvent.MissionChoices)
        {
            var controller = Instantiate(_choiceViewControllerPrefab, _choiceParent);

            controller.Init(choice, HandleSelectChoice);

            if (choice.Character == null)
            {
                controller.SetUnavailableOverlay(false);
            }
            else
            {
                controller.SetUnavailableOverlay(!heros.Contains(choice.Character));
            }

            controller.ShowStats(showStats);
        }

        _choiceSelectionCallback = callback;
    }

    private void HandleSelectChoice(UIItemController controller)
    {
        var choice = controller.GetItem<MissionChoice>();

        _choiceSelectionCallback?.Invoke(choice);
    }

    public void Close()
    {
        _view.SetActive(false);
    }
}
