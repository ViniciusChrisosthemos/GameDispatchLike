using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterRoleKeywordType
{
    Fighter,
    Strategist,
    Runner,
    Defender,
    Supporter,
    Controller,
    Rescuer,
    Infiltrator,
}

[CreateAssetMenu(fileName = "Keyword_", menuName = "ScriptableObjects/Character/Keyword/Role Keyword")]
public class CharacterRoleKeywordSO : AbstractKeywordSO
{
    public CharacterRoleKeywordType KeywordValue;
}
