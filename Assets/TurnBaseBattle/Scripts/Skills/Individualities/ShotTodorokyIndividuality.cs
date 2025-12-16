using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ShotTodorokyIndividuality : AbstractIndividuality
{
    [SerializeField] private SkillResourceSO _fireResourceSO;
    [SerializeField] private SkillResourceSO _iceResourceSO;

    [SerializeField] private List<ResourceBonus> _fireBonus;
    [SerializeField] private List<ResourceBonus> _iceBonus;

    private IBattleCharacter _character;

    private int _currentFireStacks = 0;
    private int _currentIceStacks = 0;

    public override void Init(IBattleCharacter character)
    {
        _character = character;
    }

    public override void OnTurnEnd()
    {

    }

    public override void OnTurnStart()
    {

    }

    public override void UpdateIndividuality()
    {
        var fireStacks = _character.GetSkillResourceAmount(_fireResourceSO);
        var iceStacks = _character.GetSkillResourceAmount(_iceResourceSO);
        
        if (fireStacks > iceStacks)
        {
            _currentFireStacks = fireStacks - iceStacks;
            _character.RmvResource(_iceResourceSO, iceStacks);

            IndividualityUtils.ApplyResourceBonus(_character, _fireBonus, _currentFireStacks);
        }
        else
        {
            _currentIceStacks = iceStacks - fireStacks;
            _character.RmvResource(_fireResourceSO, fireStacks);

            IndividualityUtils.ApplyResourceBonus(_character, _iceBonus, _currentIceStacks);
        }
    }
}
