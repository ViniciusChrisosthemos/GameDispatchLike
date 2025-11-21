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

    public BattleCharacter (CharacterUnit characterUnit)
    {
        _isActive = true;
        _characterUnit = characterUnit;
        _currentStatManager = new StatManager(_characterUnit.StatManager);

        _currentHealth = _maxHealth = characterUnit.StatManager.GetStat(StatManager.StatType.Endurance).GetValue() * 10;
    }

    public CharacterSO BaseCharacter => _characterUnit.BaseCharacterSO;

    public int GetPriority()
    {
        return _currentStatManager.GetStat(StatManager.StatType.Agility).GetValue();
    }

    public bool IsActive()
    {
        return _isActive;
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

    public List<BaseSkillSO> GetSkills() => BaseCharacter.Skills;

    public int Health => _currentHealth;
    public int MaxHealth => _maxHealth;
}
