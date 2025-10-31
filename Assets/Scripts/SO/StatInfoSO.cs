using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatInfo", menuName = "ScriptableObjects/Stat Info")]
public class StatInfoSO : ScriptableObject
{
    public StatManager.StatType Type;
    public Sprite Sprite;
    public string Description;
}
