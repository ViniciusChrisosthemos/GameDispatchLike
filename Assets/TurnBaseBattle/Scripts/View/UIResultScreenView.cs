using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResultScreenView : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private GameObject _playerWinView;
    [SerializeField] private GameObject _enemyWinView;
    [SerializeField] private Button _btnOk;

    private void Start()
    {
        _view.SetActive(false);
    }

    public void ShowResult(bool playerWin, Action callback)
    {
        _view.SetActive(true);

        _playerWinView.SetActive(playerWin);
        _enemyWinView.SetActive(!playerWin);
        
        _btnOk.onClick.AddListener(() => { _view.SetActive(false); callback?.Invoke(); });
    }
}
