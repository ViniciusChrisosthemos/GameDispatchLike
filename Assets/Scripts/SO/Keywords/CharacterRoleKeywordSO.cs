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
    Commander,
    Rescuer,
    Infiltrator,
}

[CreateAssetMenu(fileName = "Keyword_Role_", menuName = "ScriptableObjects/Character/Keyword/Role Keyword")]
public class CharacterRoleKeywordSO : AbstractKeywordSO
{
    public CharacterRoleKeywordType KeywordValue;
}
