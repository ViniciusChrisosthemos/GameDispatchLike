using System;
using UnityEngine;

public abstract class AbstractScreen : MonoBehaviour
{
    [SerializeField] private GameObject _view;

    private Action _updateMainScreenCallback;

    public void OpenScreen(Action callback)
    {
        _view.SetActive(true);

        InitScreen();

        _updateMainScreenCallback = callback;
    }

    public void CloseScreen()
    {
        _view.SetActive(false);
    }

    protected abstract void InitScreen();

    public void UpdateMainScreen()
    {
        _updateMainScreenCallback?.Invoke();
    }
}
