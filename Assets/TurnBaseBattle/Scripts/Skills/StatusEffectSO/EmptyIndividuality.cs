using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Individuality", menuName = "TurnBaseBattle/Skills/Individuality/Empty")]
public class EmptyIndividuality : AbstractIndividuality
{
    public override void Init(IBattleCharacter character) {}

    public override void OnTurnEnd() {}

    public override void OnTurnStart() {}

    public override void UpdateIndividuality() {}
}
