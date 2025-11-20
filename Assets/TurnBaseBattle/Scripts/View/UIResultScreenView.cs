using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIResultScreenView : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private GameObject _playerWinView;
    [SerializeField] private GameObject _enemyWinView;

    private void Start()
    {
        _view.SetActive(false);
    }

    public void ShowResult(bool playerWin)
    {
        _view.SetActive(true);

        _playerWinView.SetActive(playerWin);
        _enemyWinView.SetActive(!playerWin);
    }
}
