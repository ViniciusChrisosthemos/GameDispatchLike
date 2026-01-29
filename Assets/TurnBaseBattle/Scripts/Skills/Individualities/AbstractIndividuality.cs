using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractIndividuality : ScriptableObject
{
    [SerializeField] private string _individualityName;
    [SerializeField] private string _individualityDescription;

    public AbstractIndividualityView IndividualityView;

    public abstract void Init(IBattleCharacter character);
    public abstract void OnTurnStart();
    public abstract void OnTurnEnd();
    public abstract void UpdateIndividuality();
    
    public string IndividualityName => _individualityName;
    
    public string IndividualityDescription => _individualityDescription;
}
