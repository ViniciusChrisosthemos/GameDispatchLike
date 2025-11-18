using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillUnit
{
    private BaseSkillSO _skillSO;
    private int _currentLevel;

    public SkillUnit(BaseSkillSO skillSO)
    {
        _currentLevel = 1;
        _skillSO = skillSO;
    }

    public void UpgradeSkill()
    {
        var levels = _skillSO.GetLevels();

        if (_currentLevel == levels.Count) return;

        _currentLevel = Mathf.Min(_currentLevel + 1, levels.Count+1);
    }

    public int CurrentLevel => _currentLevel;
    public int MaxLevel => _skillSO.GetLevels().Count;
    public Sprite Art => _skillSO.Art;
    public string Description => _skillSO.GetDescription();
}
