using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MissionSO;

public class UIRequirementViewController : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private Transform _descriptionParent;
    [SerializeField] private UIRequirementTextViewController _uiRequirementTextViewControllerPrefab;

    [SerializeField] private GameSettingsSO _gameSettingsSO;

    private void Start()
    {
        Close();
    }

    public void UpdateWindow(List<MissionRequirement> descriptions, bool showStats)
    {
        _view.SetActive(true);
        _descriptionParent.ClearChilds();

        foreach (var requirement in descriptions)
        {
            var controller = Instantiate(_uiRequirementTextViewControllerPrefab, _descriptionParent);
            controller.SetText(requirement, _gameSettingsSO.RequirementStringColor);
            controller.SetStatView(showStats);
        }
    }

    public void Close()
    {
        _view.SetActive(false);
    }
}
