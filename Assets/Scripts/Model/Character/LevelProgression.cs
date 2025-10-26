using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelProgression", menuName = "ScriptableObjects/LevelProgression")]
public class LevelProgression : ScriptableObject
{
    public AnimationCurve XPCurve;
    public int MaxLevel = 10;
    public int MultiplyXPBy = 1000;

    public int GetXPForLevel(int level)
    {
        var clampedLevel = Mathf.Clamp(level, 1, MaxLevel);
        var normalizedLevel = (float)(clampedLevel) / (MaxLevel - 1);

        return Mathf.RoundToInt(XPCurve.Evaluate(normalizedLevel) * MultiplyXPBy);
    }
}
