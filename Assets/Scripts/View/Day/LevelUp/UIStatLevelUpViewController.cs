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

    private Action<StatType> _onDecreaseCallback;
    private Action<StatType> _onIncreaseCallback;

    private Stat _currentStat;
    private StatType _type;

    public void Init(StatInfoSO statInfo, Stat currentStat, Action<StatType> onDecreaseCallback, Action<StatType> onIncreaseCallback)
    {
        _imgStat.sprite = statInfo.Sprite;
        _txtStatName.text = statInfo.Description.ToUpper();
        _txtStatAmount.text = currentStat.GetValue().ToString();

        _onDecreaseCallback = onDecreaseCallback;
        _onIncreaseCallback = onIncreaseCallback;

        _btnDecreaseValue.onClick.AddListener(() => HandleDecreaseStat(statInfo.Type));
        _btnIncreaseValue.onClick.AddListener(() => HandleIncreaseStat(statInfo.Type));

        _overlayBTNDecrease.SetActive(true);
        _overlayBTNIncrease.SetActive(true);

        _type = statInfo.Type;
        _currentStat = currentStat;
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

    public Stat Stat => _currentStat;
    public StatType Type => _type;
}
