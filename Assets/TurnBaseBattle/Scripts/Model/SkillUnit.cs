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

    public int CurrentLevel => _currentLevel;
    public Sprite Art => _skillSO.Art;
    public string Description => _skillSO.GetDescription();
}
