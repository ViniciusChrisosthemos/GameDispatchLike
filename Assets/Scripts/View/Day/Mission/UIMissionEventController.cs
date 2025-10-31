using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static MissionSO;

public class UIMissionEventController : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private TextMeshProUGUI _txtDescription;
    [SerializeField] private Transform _choicesParent;
    [SerializeField] private UIChoiceViewController _uiChoiceControllerPrefab;

    private Action<MissionChoice> _choiceSelectedCallback;

    public void OpenScreen(List<CharacterSO> characters, RandomMissionEvent missionEvent, Action<MissionChoice> choiceSelectedCallback)
    {
        _view.SetActive(true);

        _txtDescription.text = missionEvent.Description;

        _choicesParent.ClearChilds();

        foreach (var choice in missionEvent.MissionChoices)
        {
            var controller = Instantiate(_uiChoiceControllerPrefab, _choicesParent);

            controller.Init(choice, HandleChoiceSelected);

            controller.SetUnavailableOverlay(choice.Character != null && characters.Contains(choice.Character));
        }

        _choiceSelectedCallback = choiceSelectedCallback;
    }

    private void HandleChoiceSelected(object obj)
    {
        var choice = (MissionChoice)obj;

        _choiceSelectedCallback?.Invoke(choice);

        CloseScreen();
    }

    private void CloseScreen()
    {
        _view.SetActive(false);
    }
}
