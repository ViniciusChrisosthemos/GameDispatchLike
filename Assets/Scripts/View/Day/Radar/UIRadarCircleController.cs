using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UIRadarCircleController : MonoBehaviour
{
    [SerializeField] private Image _imgCircle;

    public void UpdateCircle(float circleRadius, Color color)
    {
        _imgCircle.color = color;

        GetComponent<RectTransform>().sizeDelta = Vector2.one * circleRadius;
    }
}
