using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RankType
{
    D,
    C,
    B,
    A,
    S
}

[CreateAssetMenu(fileName = "Rank", menuName = "ScriptableObjects/Character/Rank")]
public class RankSO : ScriptableObject
{
    public RankType Rank;
    public string Description;
    public Color BackgroundColor;
}
