using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShotoTodorokyIndividualityView : AbstractIndividualityView
{
    [SerializeField] private Slider _sliderIce;
    [SerializeField] private Slider _sliderFire;

    [SerializeField] private UIListDisplay _uiListIceBonus;
    [SerializeField] private UIListDisplay _uiListFireBonus;

    [SerializeField] private SkillResourceSO _iceResource;
    [SerializeField] private SkillResourceSO _fireResource;

    private IBattleCharacter _character;
    private ShotoTodorokyIndividuality _individuality;

    public override void InitView(IBattleCharacter character, AbstractIndividuality individuality)
    {
        Debug.Log("[SHOTO] InitView");
        _character = character;

        _sliderFire.value = 0;
        _sliderIce.value = 0;

        _individuality = individuality as ShotoTodorokyIndividuality;

        _uiListIceBonus.SetItems(_individuality.IceBonus, null);
        _uiListFireBonus.SetItems(_individuality.FireBonus, null);



        UpdateView();
    }

    public override void OnTurnEnd()
    {

    }

    public override void OnTurnStart()
    {

    }

    public override void UpdateView()
    {
        Debug.Log("[SHOTO] UpdateView");

        var iceStack = _character.GetSkillResourceAmount(_iceResource);
        var fireStack = _character.GetSkillResourceAmount(_fireResource);

        _sliderIce.value = iceStack;
        _sliderFire.value = fireStack;

        var iceBonusControllers = _uiListIceBonus.GetControllers().Select(c => c as UIStatBonusItemView).ToList();
        var fireBonusControllers = _uiListFireBonus.GetControllers().Select(c => c as UIStatBonusItemView).ToList();

        Debug.Log($"{iceBonusControllers.Count} {_individuality.IceBonus.Count}  {fireBonusControllers.Count} {_individuality.FireBonus.Count}");

        for (int i = 0; i < _individuality.IceBonus.Count; i++)
        {
            iceBonusControllers[i].SetActive(_individuality.IceBonus[i].IsApplied);
        }

        for (int i = 0; i < _individuality.FireBonus.Count; i++)
        {
            fireBonusControllers[i].SetActive(_individuality.FireBonus[i].IsApplied);
        }
    }
}
