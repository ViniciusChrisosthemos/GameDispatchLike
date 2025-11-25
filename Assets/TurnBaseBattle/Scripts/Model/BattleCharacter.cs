using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacter : IBattleCharacter, ITimelineElement
{
    private CharacterUnit _characterUnit;
    private StatManager _currentStatManager;

    private int _currentHealth;
    private int _maxHealth;

    private bool _isActive;
    private int _actionBlockerAmount;

    private List<SkillStatusRuntime> _currentStatus;
    private List<SkillResourceRuntime> _currentResources;

    public BattleCharacter (CharacterUnit characterUnit)
    {
        _isActive = true;
        _characterUnit = characterUnit;
        _currentStatManager = new StatManager(_characterUnit.StatManager);

        _currentHealth = _maxHealth = characterUnit.StatManager.GetStat(StatManager.StatType.Endurance).GetValue() * 10;

        _actionBlockerAmount = 0;

        _currentStatus = new List<SkillStatusRuntime>();
        _currentResources = new List<SkillResourceRuntime>();
    }

    public CharacterSO BaseCharacter => _characterUnit.BaseCharacterSO;

    public int GetPriority()
    {
        return _currentStatManager.GetStat(StatManager.StatType.Agility).GetValue();
    }

    public bool IsActive()
    {
        return _isActive && _actionBlockerAmount == 0;
    }

    public bool IsAlive()
    {
        return _currentHealth > 0;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth = Mathf.Max(_currentHealth - damage, 0);
    }

    public void TakeHeal(int heal)
    {
        _currentHealth = Mathf.Min(_currentHealth + heal, _maxHealth);
    }

    public float GetNormalizedHealth()
    {
        return (float)_currentHealth / _maxHealth;
    }

    public void KillCharacter()
    {
        _isActive = false;
    }

    public StatManager GetStatManager()
    {
        return _currentStatManager;
    }

    public DiceManager GetDiceManager()
    {
        return _characterUnit.DiceManager;
    }
    public int GetMaxHealth()
    {
        return MaxHealth;
    }

    public void AddActionBlocker()
    {
        _actionBlockerAmount++;
    }

    public void RmvActionBlocker()
    {
        _actionBlockerAmount = Math.Max(0, _actionBlockerAmount - 1);
    }

    public void AddStatus(AbstractSkillStatus status)
    {
        var statusRuntime = new SkillStatusRuntime(status, status.Duration, this);

        _currentStatus.Add(statusRuntime);

        statusRuntime.StatusSO.OnApply(statusRuntime);
    }

    public void OnTurnStart()
    {
        _currentStatus.ForEach(statusRuntime => statusRuntime.StatusSO.OnTurnStart(statusRuntime));
    }

    public void OnTurnEnd()
    {
        _currentStatus.ForEach(statusRuntime => statusRuntime.StatusSO.OnTurnEnd(statusRuntime));
    }

    public void AddResource(SkillResourceSO resource, int amount)
    {
        var currentResource = _currentResources.Find(r => r.SkillResourceSO == resource);

        if (currentResource == null)
        {
            currentResource = new SkillResourceRuntime(resource, amount);
            _currentResources.Add(currentResource);
        }
        else
        {
            currentResource.AddAmount(amount);
        }
    }

    public void RmvResource(SkillResourceSO resource, int amount)
    {
        var currentResource = _currentResources.Find(r => r.SkillResourceSO == resource);
        
        if (currentResource != null)
        {
            currentResource.RemoveAmount(amount);

            if (currentResource.Amount == 0)
            {
                _currentResources.Remove(currentResource);
            }
        }
    }

    public int GetSkillResourceAmount(SkillResourceSO resourceSO)
    {
        var currentResource = _currentResources.Find(r => r.SkillResourceSO == resourceSO);

        return currentResource != null ? currentResource.Amount : 0;
    }

    public int Health => _currentHealth;
    public int MaxHealth => _maxHealth;
    public List<BaseSkillSO> GetSkills() => BaseCharacter.Skills;

}
