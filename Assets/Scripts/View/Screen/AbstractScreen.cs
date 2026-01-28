using UnityEngine;

public abstract class AbstractScreen : MonoBehaviour
{
    [SerializeField] private GameObject _view;

    public void OpenScreen()
    {
        _view.SetActive(true);

        InitScreen();
    }

    protected abstract void InitScreen();

    public void CloseScreen()
    {
        _view.SetActive(false);
    }
}
