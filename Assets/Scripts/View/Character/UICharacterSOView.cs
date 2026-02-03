using UnityEngine;

public class UICharacterSOView : AbstractUICharacterView
{
    [Header("UICharacterSOView Field")]
    [SerializeField] private UIRadarChartController _uiRadarChartController;

    private CharacterSO _characterSO;

    protected override void HandleInit(object obj)
    {
        _characterSO = obj as CharacterSO;

        SetCharacterInfo(_characterSO);

        _uiRadarChartController.UpdateStats(_characterSO.BaseStats.GetValues());
    }

    public CharacterSO CharacterSO => _characterSO;
}
