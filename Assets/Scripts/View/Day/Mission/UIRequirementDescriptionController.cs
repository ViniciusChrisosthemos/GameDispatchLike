using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MissionSO;

public class UIRequirementDescriptionController : MonoBehaviour
{
    [SerializeField] private GameSettingsSO _gameSettingsSO;
    [SerializeField] private Transform _descriptionParent;
    [SerializeField] private UIRequirementTextViewController _uiRequirementTextViewControllerPrefab;

    public void SetDescription(List<MissionRequirement> descriptions, bool showStats)
    {
        _descriptionParent.ClearChilds();

        foreach (var requirement in descriptions)
        {
            var controller = Instantiate(_uiRequirementTextViewControllerPrefab, _descriptionParent);
            controller.SetText(requirement, _gameSettingsSO.RequirementStringColor);
            controller.SetStatView(showStats);
        }
    }
}
