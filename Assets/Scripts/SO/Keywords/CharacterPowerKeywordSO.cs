using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CharacterPowerKeywordType
{
    Elementalist,
    Shapeshifter,
    Telekinetic,
    GravityBender,
    Soundbreaker,
    Shadowweaver,
    Lightcaster,
    Metalshaper,
    Titanform,
    Beastborn
}

[CreateAssetMenu(fileName = "Keyword_", menuName = "ScriptableObjects/Character/Keyword/Power Keyword")]
public class CharacterPowerKeywordSO : AbstractKeywordSO
{
    public CharacterPowerKeywordType KeywordValue;
}