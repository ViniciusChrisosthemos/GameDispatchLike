using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static StatManager;

public class UIStatLevelUpViewController : MonoBehaviour
{
    [SerializeField] private Image _imgStat;
    [SerializeField] private TextMeshProUGUI _txtStatName;
    [SerializeField] private TextMeshProUGUI _txtStatAmount;
    [SerializeField] private Button _btnDecreaseValue;
    [SerializeField] private Button _btnIncreaseValue;
    [SerializeField] private GameObject _overlayBTNDecrease;
    [SerializeField] private GameObject _overlayBTNIncrease;

    [SerializeField] private StatType _type;

    private Action<StatType> _onDecreaseCallback;
    private Action<StatType> _onIncreaseCallback;

    public void Init(Action<StatType> onDecreaseCallback, Action<StatType> onIncreaseCallback)
    {
        _onDecreaseCallback = onDecreaseCallback;
        _onIncreaseCallback = onIncreaseCallback;

        _btnDecreaseValue.onClick.AddListener(() => HandleDecreaseStat(_type));
        _btnIncreaseValue.onClick.AddListener(() => HandleIncreaseStat(_type));

        _overlayBTNDecrease.SetActive(true);
        _overlayBTNIncrease.SetActive(true);
    }

    public void UpdateStatInfo(int statValue, bool hasPoints)
    {
        _overlayBTNDecrease.SetActive(true);
        _overlayBTNIncrease.SetActive(!hasPoints);

        _txtStatAmount.text = statValue.ToString();
    }

    private void HandleDecreaseStat(StatType statType)
    {
        _onDecreaseCallback?.Invoke(statType);
    }

    private void HandleIncreaseStat(StatType statType)
    {
        _onIncreaseCallback?.Invoke(statType);
    }

    public void SetStatAmount(int amount)
    {
        _txtStatAmount.text = amount.ToString();
    }

    public void SetBTNIncrease(bool isActive)
    {
        _overlayBTNIncrease.SetActive(isActive);
    }

    public void SetBTNDecrease(bool isActive)
    {
        _overlayBTNDecrease.SetActive(isActive);
    }

    public StatType Type => _type;
}
