using UnityEngine;

public class UICharacterSOView : AbstractUICharacterView
{
    [Header("UICharacterSOView Field")]
    [Header("Optional")]
    [SerializeField] private UIRadarChartController _uiRadarCharatController;

    private CharacterSO _characterSO;

    protected override void HandleInit(object obj)
    {
        _characterSO = obj as CharacterSO;

        SetCharacterSO(_characterSO);

        if (_uiRadarCharatController != null)
        {
            _uiRadarCharatController.UpdateStats(_characterSO.BaseStats.GetValues());
        }
    }

    public CharacterSO CharacterSO => _characterSO;
}
