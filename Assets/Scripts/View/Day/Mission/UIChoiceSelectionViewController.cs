using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static MissionSO;

public class UIChoiceSelectionViewController : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private Transform _choiceParent;
    [SerializeField] private UIChoiceViewController _choiceViewControllerPrefab;
    [SerializeField] private UISendActionViewController _uiSendActionViewController;
    [SerializeField] private Button _btnCloseScreen;

    [Header("(Optional)")]
    [SerializeField] private TextMeshProUGUI _txtMissionName;
    [SerializeField] private TextMeshProUGUI _txtEventDescription;

    private Action<MissionChoice> _choiceSelectionCallback;
    private Action _closeScreenCallback;

    private void Start()
    {
        if (_btnCloseScreen != null)
            _btnCloseScreen.onClick.AddListener(Close);

        CloseWithoutNotify();
    }

    public void OpenScreen(MissionUnit missionUnit, RandomMissionEvent missionEvent, bool showStats, Action<MissionChoice> callback, Action closeCallback)
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
                var hasCharacter = heros.Contains(choice.Character);

                if (hasCharacter)
                {
                    controller.SetUnavailableOverlay(!hasCharacter);
                }
                else
                {
                    controller.SetOptionLocked(choice.Character.Name);
                }
            }

            controller.ShowStats(showStats);
        }

        _choiceSelectionCallback = callback;

        if (_btnCloseScreen != null)
        {
            _closeScreenCallback = closeCallback;
        }
    }

    private void HandleSelectChoice(UIItemController controller)
    {
        var choice = controller.GetItem<MissionChoice>();

        _uiSendActionViewController.StartSendAction(() =>
        {
            _closeScreenCallback = null;
            _choiceSelectionCallback?.Invoke(choice);
        });

    }

    public void CloseWithoutNotify()
    {
        _view.SetActive(false);  
    }

    public void Close()
    {
        Debug.Log("Close Choice Screen");
        _view.SetActive(false);
        
        _closeScreenCallback?.Invoke();
        _closeScreenCallback = null;
    }
}
