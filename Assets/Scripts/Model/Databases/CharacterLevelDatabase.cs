using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterLevelDatabase : Singleton<CharacterLevelDatabase>
{
    [SerializeField] private List<PlayerLevelSO> _levelDescriptionSOs;

    public PlayerLevelSO GetLevel(int level)
    {
        int clampedLevel = Mathf.Clamp(level, 0, _levelDescriptionSOs.Count);

        return _levelDescriptionSOs[clampedLevel];
    }
}
