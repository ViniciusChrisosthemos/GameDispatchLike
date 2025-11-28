using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillResourceRuntime
{
    public int Amount { get; private set; }
    public SkillResourceSO SkillResourceSO;

    public SkillResourceRuntime(SkillResourceSO skillResourceSO, int initialAmount)
    {
        SkillResourceSO = skillResourceSO;
        Amount = initialAmount;
    }

    public void AddAmount(int amount)
    {
        Amount = Mathf.Clamp(Amount + amount, 0, SkillResourceSO.MaxAmount);
    }

    public void RemoveAmount(int amount)
    {
        Amount = Mathf.Clamp(Amount - amount, 0, SkillResourceSO.MaxAmount);
    }
}
