using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGiffController : MonoBehaviour
{
    [SerializeField] private Image _imgTarget;
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private float _frameRate = 0.1f;

    private int _currentIndex = 0;
    private float _accumTime = 0f;

    private void Update()
    {
        if (_sprites.Count == 0) return;

        _accumTime += Time.deltaTime;

        if (_accumTime >= _frameRate)
        {
            _accumTime = 0f;
            UpdateSprite();
        }
    }

    private void UpdateSprite()
    {
        _imgTarget.sprite = _sprites[_currentIndex];
        _currentIndex = (_currentIndex + 1) % _sprites.Count;
    }
}
