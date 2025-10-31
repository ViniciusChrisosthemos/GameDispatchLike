using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using static StatManager;

public class Temp : MonoBehaviour

{
    public UIRadarChartController radar;

    public List<float> values = new List<float>() { 0, 0, 0, 0.3f, 0.5f, 0.3f };

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.K))
        {
            radar.UpdateStats(values);
        }
    }
}
