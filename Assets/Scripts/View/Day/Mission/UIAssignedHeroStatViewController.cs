using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAssignedHeroStatViewController : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private UIRadarChartController _teamRadarChartController;

    public void OpenScreen(Team team)
    {
        _view.SetActive(true);

        var values = team.GetTeamStats().GetValues();

        _teamRadarChartController.UpdateStats(values);
    }

    public void Close()
    {
        _view.SetActive(false);
    }
}
