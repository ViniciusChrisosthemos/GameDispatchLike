using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static StatManager;

public class Temp : MonoBehaviour
{
    public UIRadarChartMissionResultController controller;
    public UIAnimatePolygonBounceController animatePolygonBounceController;
    public UIRadarChartController radar;

    public List<CharacterSO> characterSOs;
    public MissionSO missionSO;

    public float duration = 2f;
    public float speed = 150f;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            var charactersUnits = new List<CharacterUnit>();

            characterSOs.ForEach(c =>charactersUnits.Add(new CharacterUnit(c)));

            var team = new Team(charactersUnits.Count);

            charactersUnits.ForEach(c => team.AddMember(c));
            
            var listOfvalues = new List<List<float>>();

            var v1 = missionSO.RequiredStats.GetValues();
            var v2 = team.GetTeamStats().GetValues();
            listOfvalues.Add(v1);
            listOfvalues.Add(v2);

            controller.AnimateRadarChartComparison(listOfvalues);

            animatePolygonBounceController.Animate(radar.GetVertices().Select(v => new Vector2(v.x, v.y)).ToList(), duration, speed);
        }
    }
}
