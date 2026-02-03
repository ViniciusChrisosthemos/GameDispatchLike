using UnityEngine;

public class UICharacterUnitView : AbstractUICharacterView
{
    [Header("Character Unit Field")]
    [SerializeField] private UIRadarChartController _uiRadarChartController;

    private CharacterUnit _characterUnit;

    protected override void HandleInit(object obj)
    {
        _characterUnit = obj as CharacterUnit;

        SetCharacterInfo(_characterUnit.BaseCharacterSO);

        _uiRadarChartController.UpdateStats(_characterUnit.StatManager.GetValues());
    }

    public CharacterUnit CharacterUnit => _characterUnit;
}
