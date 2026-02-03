using System;
using UnityEngine;

public abstract class AbstractScreen : MonoBehaviour
{
    [SerializeField] private GameObject _view;

    private Action _updateMainScreenCallback;

    public void OpenScreen(Action updateMainScreenCallback)
    {
        _view.SetActive(true);

        _updateMainScreenCallback = updateMainScreenCallback;

        InitScreen();
    }

    protected abstract void InitScreen();

    protected void UpdateMainScreen()
    {
        _updateMainScreenCallback?.Invoke();
    }

    public void CloseScreen()
    {
        _view.SetActive(false);
    }
}
