using UnityEngine;

public class UICharacterUnitView : AbstractUICharacterView
{
    [Header("UiCharacterUnitView Field")]
    [Header("(optional)")]
    [SerializeField] private UIRadarChartController _uiRadarChartController;

    private CharacterUnit _characterUnit;

    protected override void HandleInit(object obj)
    {
        _characterUnit = obj as CharacterUnit;

        SetCharacterSO(_characterUnit.BaseCharacterSO);

        if (_uiRadarChartController != null)
        {
            _uiRadarChartController.UpdateStats(_characterUnit.StatManager.GetValues());
        }
    }

    public CharacterUnit CharacterUnit => _characterUnit;
}
