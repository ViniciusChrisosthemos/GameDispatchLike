using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEventCompleteController : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private Button _btnOK;
    [SerializeField] private GameObject _successMessagem;
    [SerializeField] private GameObject _failMessagem;

    [Header("Normal choice")]
    [SerializeField] private GameObject _normalChoiceView;
    [SerializeField] private Image _imgTargetStat;
    [SerializeField] private TextMeshProUGUI _txtStatRequired;
    [SerializeField] private TextMeshProUGUI _txtStatGiven;

    [Header("Character choice")]
    [SerializeField] private GameObject _characterChoiceView;
    [SerializeField] private Image _imgCharacterChoice;

    [Header("Parameters")]
    [SerializeField] private List<StatInfoSO> _statsInfoSOs;

    private void Start()
    {
        CloseScreen();
    }

    public void OpenScreen(MissionUnit mission, Action callback)
    {
        _view.SetActive(true);

        var missionEvent = mission.MissionEvent;
        var choice = mission.MissionChoice;

        if (choice.Character == null)
        {
            _normalChoiceView.SetActive(true);
            _characterChoiceView.SetActive(false);

            var statInfo = _statsInfoSOs.Find(s => s.Type == choice.StatType);

            if (statInfo != null)
            {
                _imgTargetStat.sprite = statInfo.Sprite;

                var statRequired = choice.StatAmountRequired;
                var statGiven = mission.Team.GetTeamStats().GetStat(choice.StatType).GetValue();

                _txtStatRequired.text = statRequired.ToString();
                _txtStatGiven.text = statGiven.ToString();

                _successMessagem.SetActive(statGiven >= statRequired);
                _failMessagem.SetActive(_successMessagem.activeSelf);
            }
            else
            {
                Debug.LogError($"[{GetType()}][OpenScreen] StatInfo '{choice.StatType}' não encontrado!");
            }
        }
        else
        {
            _normalChoiceView.SetActive(false);
            _characterChoiceView.SetActive(true);

            _imgCharacterChoice.sprite = choice.Character.BodyArt;
        }

        _btnOK.onClick.RemoveAllListeners();
        _btnOK.onClick.AddListener(() => { callback?.Invoke(); CloseScreen(); });
    }

    public void CloseScreen()
    {
        _view.SetActive(false);
    }
}
