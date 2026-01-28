using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DayDatabase : Singleton<DayDatabase>
{
    [SerializeField] private List<DaySO> _daySOs;

    public DaySO GetDaySO(int day)
    {
        if (day - 1 < 0 || day - 1 >= _daySOs.Count) return null;
        return _daySOs[day - 1];
    }
}
