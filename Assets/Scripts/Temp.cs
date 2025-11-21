using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static StatManager;

public class Temp : MonoBehaviour
{
    private void Start()
    {
        // Find all active objects of a specific MonoBehaviour script type
        var customScripts = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);

        Debug.Log("==============");
        foreach (var i in customScripts)
        {
            Debug.Log($"{i.name}", i.gameObject);
        }
    }
}
