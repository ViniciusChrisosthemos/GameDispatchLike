using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GameSettings", menuName ="ScriptableObjects/GameSettings")]
public class GameSettingsSO: ScriptableObject
{
    public int MaxScheduledCharacters = 5;
}
