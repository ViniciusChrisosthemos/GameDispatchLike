using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DicesSkillBonus_", menuName = "TurnBaseBattle/Skills/SkillBonus/Dices")]
public class DicesSkillBonus : AbstractSkillBonus
{
    [SerializeField] private DiceType _diceType;
    [SerializeField] private int _amountToAdd;

    public override void AddBonus(IBattleCharacter target)
    {
        var diceManager = target.GetDiceManager();

        diceManager.AddDices(_diceType, _amountToAdd);
    }

    public override void RmvBonus(IBattleCharacter target)
    {
        var diceManager = target.GetDiceManager();

        diceManager.RmvDices(_diceType, _amountToAdd);
    }

    public override string Description => string.Format(_description, _amountToAdd);
}
