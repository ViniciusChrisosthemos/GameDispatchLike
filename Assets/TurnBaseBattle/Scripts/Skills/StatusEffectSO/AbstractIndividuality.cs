using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractIndividuality : ScriptableObject
{
    public abstract void Init(IBattleCharacter character);
    public abstract void OnTurnStart();
    public abstract void OnTurnEnd();
    public abstract void UpdateIndividuality();
}
