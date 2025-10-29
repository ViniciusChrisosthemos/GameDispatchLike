using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDayReportController : MonoBehaviour
{
    [SerializeField] private GameObject _view;

    [SerializeField] private TextMeshProUGUI _txtMissionAccepted;
    [SerializeField] private TextMeshProUGUI _txtMissionLosted;
    [SerializeField] private TextMeshProUGUI _txtMissionSuccessed;
    [SerializeField] private TextMeshProUGUI _txtMissionFailed;

    [SerializeField] private TextMeshProUGUI _txtTotalGoldGained;
    [SerializeField] private TextMeshProUGUI _txtTotalReputationGained;

    [SerializeField] private TextMeshProUGUI _txtMostSentCharacter;

    [SerializeField] private Button _btnCloseScreen;

    public void OpenScreen(DayReport report, Action callback)
    {
        _view.SetActive(true);

        _txtMissionAccepted.text = report.MissionsAccepted.ToString();
        _txtMissionLosted.text = report.MissionLosted.ToString();
        _txtMissionSuccessed.text = report.MissionSucceded.ToString();
        _txtMissionFailed.text = report.MissionFailed.ToString();

        _txtTotalGoldGained.text = report.TotalGoldGained.ToString();
        _txtTotalReputationGained.text = report.TotalReputationGained.ToString();

        _btnCloseScreen.onClick.RemoveAllListeners();
        _btnCloseScreen.onClick.AddListener(() =>
        {
            HandleCloseScreen();
            callback?.Invoke();
        });
    }

    private void HandleCloseScreen()
    {
        _view.SetActive(false);
    }
}
