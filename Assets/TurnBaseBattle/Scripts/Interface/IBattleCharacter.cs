using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleCharacter : ITimelineElement
{
    int GetMaxHealth();
    void TakeDamage(int damage);

    void TakeHeal(int heal);

    bool IsAlive();

    StatManager GetStatManager();

    DiceManager GetDiceManager();

    void AddStatus(AbstractSkillStatus status);
    void AddActionBlocker();
    void RmvActionBlocker();

    void OnTurnStart();
    void OnTurnEnd();

    void AddResource(SkillResourceSO resource, int amount);
    void RmvResource(SkillResourceSO resource, int amount);

    int GetSkillResourceAmount(SkillResourceSO resourceSO);
}
