using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IndividualityFactory
{
    public static AbstractIndividualityView CreateIndividualityView(BattleCharacter character, Transform parent)
    {
        var individuality = character.BaseCharacter.Individuality;
        var individualityViewPrefab = character.BaseCharacter.IndividualityView;
        
        var viewInstance = GameObject.Instantiate(individualityViewPrefab, parent);
        viewInstance.InitView(character, individuality);

        return viewInstance;
    }
}
