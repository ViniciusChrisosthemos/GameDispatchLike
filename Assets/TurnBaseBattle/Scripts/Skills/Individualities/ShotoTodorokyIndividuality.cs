using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "Individuality", menuName = "TurnBaseBattle/Skills/Individuality/Shoto Todoroky")]
public class ShotoTodorokyIndividuality : AbstractIndividuality
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
            _currentIceStacks = 0;

            _character.SetResourceAmount(_fireResourceSO, _currentFireStacks);
            _character.SetResourceAmount(_iceResourceSO, _currentIceStacks);

            IndividualityUtils.ApplyResourceBonus(_character, _fireBonus, _currentFireStacks);
        }
        else
        {
            _currentIceStacks = iceStacks - fireStacks;
            _currentFireStacks = 0;

            _character.SetResourceAmount(_fireResourceSO, _currentFireStacks);
            _character.SetResourceAmount(_iceResourceSO, _currentIceStacks);

            IndividualityUtils.ApplyResourceBonus(_character, _iceBonus, _currentIceStacks);
        }
    }

    public List<ResourceBonus> IceBonus => _iceBonus;

    public List<ResourceBonus> FireBonus => _fireBonus;
}
