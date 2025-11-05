
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLevel_", menuName = "ScriptableObjects/Player Level")]
public class PlayerLevelSO : ScriptableObject
{
    public string LevelDescription;
    public int ExpToLevelUp;
}
