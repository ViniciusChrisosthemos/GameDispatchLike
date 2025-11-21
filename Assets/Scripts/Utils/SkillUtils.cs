using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SkillUtils
{
    public static List<BaseSkillSO> GetAvailableSkills(List<BaseSkillSO> skills, List<DiceValueSO> values)
    {
        var availableSkills = new List<BaseSkillSO>();

        var valuesDict = GetDictionary(values);

        foreach (var skill in skills)
        {
            var requiredValuesDict = GetDictionary(skill.RequiredDiceValues);

            if (requiredValuesDict.All(kv => valuesDict.TryGetValue(kv.Key, out int c) && c >= kv.Value))
            {
                availableSkills.Add(skill);
            }
        }

        return availableSkills;
    }

    private static Dictionary<T, int> GetDictionary<T>(List<T> values) => values.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
}
