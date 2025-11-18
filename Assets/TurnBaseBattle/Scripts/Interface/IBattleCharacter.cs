using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleCharacter : ITimelineElement
{
    void TakeDamage(int damage);

    void TakeHeal(int heal);

    bool IsAlive();
}
