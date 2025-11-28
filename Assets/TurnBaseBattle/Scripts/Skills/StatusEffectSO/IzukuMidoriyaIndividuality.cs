using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Individuality", menuName = "TurnBaseBattle/Skills/Individuality/Izuku Midoriya")]
public class IzukuMidoriyaIndividuality : AbstractIndividuality
{
    [SerializeField] private SkillResourceSO ResourceType;

    [SerializeField] private List<ResourceBonus> ResourceBonuses;

    [SerializeField] private float _percDamageTakenIfHasNoStacks = 0.2f;
    [SerializeField] private List<AbstractSkillStatus> _statusToAddIfHasNoStacks;

    private int _lastStack;

    private IBattleCharacter _character;

    public override void Init(IBattleCharacter characrter)
    {
        _lastStack = 0;

        _character = characrter;
    }

    public override void OnTurnStart()
    {
        _character.AddResource(ResourceType, 1);
        UpdateStatus(_character, _character.GetSkillResourceAmount(ResourceType));
    }

    public override void OnTurnEnd()
    {

    }

    public override void UpdateIndividuality()
    {
        UpdateStatus(_character, _character.GetSkillResourceAmount(ResourceType));
    }

    public void UpdateStatus(IBattleCharacter target, int stack)
    {
        if (stack == _lastStack) return;

        if (stack <= 0)
        {
            HandleZeroStack(target);
        }
        else
        {
            HandleNoZeroStack(target, stack);
        }

        _lastStack = stack;
    }

    private void HandleZeroStack(IBattleCharacter target)
    {
        var damage = Mathf.RoundToInt(target.GetMaxHealth() * _percDamageTakenIfHasNoStacks);

        target.TakeDamage(damage);

        _statusToAddIfHasNoStacks.ForEach(statusEffect => target.AddStatus(statusEffect));
    }

    private void HandleNoZeroStack(IBattleCharacter target, int stack)
    {
        foreach (var resourceBonus in ResourceBonuses)
        {
            if (stack >= resourceBonus.ResourceAmountRequired)
            {
                if (resourceBonus.IsApplied) continue;

                resourceBonus.Bonus.AddBonus(target);
                resourceBonus.IsApplied = true;
            }
            else
            {
                if (resourceBonus.IsApplied)
                {
                    resourceBonus.Bonus.RmvBonus(target);
                }
            }
        }
    }

    public int GetOFALevel()
    {
        return _character.GetSkillResourceAmount(ResourceType);
    }

    [Serializable]
    private class ResourceBonus
    {
        public int ResourceAmountRequired;
        public AbstractSkillBonus Bonus;
        public bool IsApplied;
    }
}
