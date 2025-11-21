
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterViewController : UIItemController
{
    [SerializeField] private Image _imgCharacterView;
    [SerializeField] private CharacterArtType _characterArtType;

    [Header("(optional)")]
    [SerializeField] private UIRadarChartController _radarChartStatController;
    [SerializeField] private Button _btnButton;
    [SerializeField] private TextMeshProUGUI _txtName;

    private CharacterUnit _characterUnit;

    private void Awake()
    {
        if (_btnButton != null)
        {
            _btnButton.onClick.AddListener(SelectItem);
        }
    }

    protected override void HandleInit(object obj)
    {
        _characterUnit = obj as CharacterUnit;

        _imgCharacterView.sprite = _characterUnit.GetArt(_characterArtType);

        if (_txtName != null)
        {
            _txtName.text = _characterUnit.Name;
        }

        if (_radarChartStatController != null)
        {
            var values = _characterUnit.StatManager.GetValues();
            _radarChartStatController.UpdateStats(values);
        }
    }

    public CharacterUnit CharacterUnit => _characterUnit;
}