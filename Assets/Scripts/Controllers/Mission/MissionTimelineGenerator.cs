using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class MissionTimelineEntry
{
    public int missionIndex;
    public float time;
}

[Serializable]
public class MissionTimelineGenerator
{
    [Header("Paramters")]
    [SerializeField] private float totalTimeSeconds = 120f;
    [SerializeField] int missionCount = 10;
    [SerializeField] float minSpacing = 3f;
    [SerializeField] float maxSpacing = 10f;
    [SerializeField] float timeJitter = 0.5f;
    

    public List<MissionTimelineEntry> GenerateTimeline()
    {
        var generatedTimeline = new List<MissionTimelineEntry>();

        float currentTime = 0f;

        for (int i = 0; i < missionCount; i++)
        {
            // Garante que o tempo não ultrapasse o limite
            if (currentTime > totalTimeSeconds)
                break;

            // Adiciona uma leve variação no tempo
            float jitter = Random.Range(-timeJitter, timeJitter);
            float missionTime = Mathf.Clamp(currentTime + jitter, 0f, totalTimeSeconds);

            generatedTimeline.Add(new MissionTimelineEntry
            {
                missionIndex = i,
                time = missionTime
            });

            // Incrementa o tempo com um espaçamento aleatório
            currentTime += Random.Range(minSpacing, maxSpacing);
        }

        // Ordena por tempo final (caso o jitter embaralhe um pouco)
        generatedTimeline.Sort((a, b) => a.time.CompareTo(b.time));

        return generatedTimeline;
    }
}
