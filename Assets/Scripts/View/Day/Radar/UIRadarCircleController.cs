using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UIRadarCircleController : MonoBehaviour
{
    [SerializeField] private Image _imgCircle;
    [SerializeField] private Image _imgBorder;
    [SerializeField] private TextMeshProUGUI _txtNumber;

    [SerializeField] private GameObject _normalCircleView;
    [SerializeField] private GameObject _numeredCircleView;

    private Transform _reference;

    private void Update()
    {
        if (_reference != null)
        {
            if (_reference.hasChanged)
            {
                transform.position = _reference.transform.position;
            }
        }
    }

    public void UpdateCircle(float circleRadius, Color color)
    {
        _normalCircleView.SetActive(true);
        _numeredCircleView.SetActive(false);

        _imgCircle.color = color;
        _imgBorder.color = color;

        GetComponent<RectTransform>().sizeDelta = Vector2.one * circleRadius;

        _txtNumber.gameObject.SetActive(false);
    }

    public void SetReference(Transform reference)
    {
        _reference = reference;
    }

    public void SetNumber(int number)
    {
        _normalCircleView.SetActive(false);
        _numeredCircleView.SetActive(true);

        _txtNumber.text = number.ToString();
        _txtNumber.gameObject.SetActive(true);
    }
}
