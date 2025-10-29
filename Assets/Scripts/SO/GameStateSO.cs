using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultGameState", menuName ="ScriptableObjects/Game State")]
public class GameStateSO : ScriptableObject
{
    public List<CharacterSO> AvailableCharacters;
    public int Balance;
    public int Reputation;
    public int CurrentDay;
}
