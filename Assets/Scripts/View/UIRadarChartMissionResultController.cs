using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRadarChartMissionResultController : MonoBehaviour
{
    [SerializeField] private List<UIRadarChartController> _radarChartControllers;

    public void AnimateRadarChartComparison(List<List<float>> listOfValues)
    {
        Debug.Log($"{_radarChartControllers.Count} {listOfValues.Count}");
        var lesserIndex = Mathf.Min(_radarChartControllers.Count, listOfValues.Count);

        for (int i = 0; i < lesserIndex; i++)
        {
            _radarChartControllers[i].UpdateStats(listOfValues[i]);
        }
    }
}
