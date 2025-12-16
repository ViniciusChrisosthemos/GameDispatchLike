using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAssignedHeroStatViewController : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private UIRadarChartController _teamRadarChartController;

    private Team _team;

    public void OpenScreen(Team team)
    {
        _view.SetActive(true);

        var values = team.GetTeamStats().GetValues();

        Debug.Log(team.GetTeamStats());
        foreach (var value in values)
        {
            Debug.Log(value);
        }

        _teamRadarChartController.UpdateStats(values);
        _teamRadarChartController.UpdateStats(values);

        _team = team;


    }

    public void Close()
    {
        _view.SetActive(false);
    }

    public void UpdateStats()
    {
        if (_team != null) OpenScreen(_team);
    }

    public void CloseWithoutNotify()
    {
        _view.SetActive(false);
    }
}
