using System;
using UnityEngine;

public class SavebleSO : ScriptableObject
{
    private string _id;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(_id))
        {
            _id = Guid.NewGuid().ToString();
        }
    }

    public string ID => _id;
}
