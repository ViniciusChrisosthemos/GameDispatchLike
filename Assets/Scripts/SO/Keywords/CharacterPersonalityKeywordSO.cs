using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterPersonalityKeywordType
{
    HotBlooded,
    ColdMind,
    LoneWolf,
    Prodigy,
    Rebel,
    Virtuoso,
    IronWill,
    WildSpirit,
    Optimist,
    ChaoticMind
}

[CreateAssetMenu(fileName = "Keyword_", menuName = "ScriptableObjects/Character/Keyword/Personality Keyword")]
public class CharacterPersonalityKeywordSO : AbstractKeywordSO
{
    public CharacterPersonalityKeywordType KeywordValue;
}