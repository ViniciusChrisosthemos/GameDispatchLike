using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillTreeItem
{
    public BaseSkillSO Skill;
    public List<BaseSkillSO> Dependencies;
}

[CreateAssetMenu(fileName = "Skill-Tree_", menuName = "ScriptableObjects/TurnBasedBattle/Skill Tree")]
public class SkillTreeSO : ScriptableObject
{
    public List<SkillTreeItem> Items;
}
