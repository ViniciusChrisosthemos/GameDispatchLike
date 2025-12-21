using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static BattleCharacter;

public class BattleCharacter : IBattleCharacter, ITimelineElement
{
    private CharacterUnit _characterUnit;
    private StatManager _currentStatManager;

    private int _currentHealth;
    private int _maxHealth;

    private bool _isActive;
    private int _actionBlockerAmount;

    private AbstractIndividuality _individuality;

    private List<SkillHolder> _skillHolders;
    private List<SkillStatusRuntime> _currentStatus;
    private List<SkillResourceRuntime> _currentResources;

    public Action OnStatusUpdated;

    public BattleCharacter (CharacterUnit characterUnit)
    {
        _isActive = true;
        _characterUnit = characterUnit;
        _currentStatManager = new StatManager(_characterUnit.StatManager);

        _currentHealth = _maxHealth = characterUnit.StatManager.GetStat(StatManager.StatType.Endurance).GetValue() * 10;

        _actionBlockerAmount = 0;

        _skillHolders = characterUnit.BaseCharacterSO.Skills.Select(skill => new SkillHolder(this, skill)).ToList();
        _currentStatus = new List<SkillStatusRuntime>();
        _currentResources = new List<SkillResourceRuntime>();

        _individuality = GameObject.Instantiate(characterUnit.BaseCharacterSO.Individuality);
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
        var currentStatus = _currentStatus.Find(s => s.GetType() == status.GetType());

        if (currentStatus != null)
        {
            currentStatus.AddStack(status.InitialStacks);
            currentStatus.ResetDuration();
        }
        else
        {
            var statusRuntime = new SkillStatusRuntime(status, this);

            _currentStatus.Add(statusRuntime);

            statusRuntime.ApplyStatus(statusRuntime);
        }
    }


    public void OnTurnStart()
    {
        _currentStatus.ForEach(statusRuntime => statusRuntime.StatusSO.OnTurnStart(statusRuntime, BattleLogger.Instance));
        _individuality.OnTurnStart();
    }

    public void OnTurnEnd()
    {
        Debug.Log($"[OnTurnEnd]   {BaseCharacter.Name}   {_currentStatus.Count}");

        foreach(var s in _currentStatus)
        {
            Debug.Log($"    {s.StatusSO.name} {s.RemainingDuration} {s.Stacks}");
        }

        _currentStatus.ForEach(statusRuntime => statusRuntime.StatusSO.OnTurnEnd(statusRuntime, BattleLogger.Instance));
        var expiredStatuses = _currentStatus.FindAll(s => s.IsExpired);
        expiredStatuses.ForEach(s => _currentStatus.Remove(s));

        Debug.Log($"    {_currentStatus.Count}");

        foreach (var s in _currentStatus)
        {
            Debug.Log($"    {s.StatusSO.name} {s.RemainingDuration} {s.Stacks}");
        }

        _individuality.OnTurnEnd();

        OnStatusUpdated?.Invoke();
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

    public void SetResourceAmount(SkillResourceSO resourceSO, int amount)
    {
        var currentResource = _currentResources.Find(r => r.SkillResourceSO == resourceSO);

        if (currentResource != null)
        {
            currentResource.SetAmount(amount);

            if (currentResource.Amount == 0)
            {
                _currentResources.Remove(currentResource);
            }
        }
    }

    public int GetSkillResourceAmount(SkillResourceSO resourceSO)
    {
        var currentResource = _currentResources.Find(r => r.SkillResourceSO.Name.Equals(resourceSO.Name));
        return currentResource != null ? currentResource.Amount : 0;
    }

    public void UpdateAfterUseSkill()
    {
        _individuality.UpdateIndividuality();
    }

    public List<SkillStatusRuntime> GetStatus()
    {
        return _currentStatus;
    }

    public CharacterUnit CharacterUnit => _characterUnit;
    public int Health => _currentHealth;
    public int MaxHealth => _maxHealth;
    public List<SkillHolder> GetSkills() => _skillHolders;


    public AbstractIndividuality Individuality => _individuality;
    public AbstractIndividualityView IndividualityView => BaseCharacter.Individuality.IndividualityView;

    public class SkillHolder
    {
        public BattleCharacter Owner;
        public BaseSkillSO Skill;

        public SkillHolder(BattleCharacter owner, BaseSkillSO skill)
        {
            Owner = owner; 
            Skill = skill;
        }

        public string GetDescription() => Skill.GetDescription(Owner);
    }

    public string GetName() => _characterUnit.Name;
}
