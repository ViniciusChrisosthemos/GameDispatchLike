using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public static class IndividualityUtils
{
    public static void ApplyResourceBonus(IBattleCharacter target, List<ResourceBonus> resourceBonuses, int stack)
    {
        foreach (var resourceBonus in resourceBonuses)
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
}
