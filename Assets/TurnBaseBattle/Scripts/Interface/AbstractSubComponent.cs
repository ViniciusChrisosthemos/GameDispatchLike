using UnityEngine;

public abstract class AbstractSubComponent<T> : MonoBehaviour
{
    protected T _manager;

    public void Setup(T manager)
    {
        if (_manager == null)
        {
            BindHandles(manager);
        }

        _manager = manager;

        HandleInternalSetup(manager);
    }

    protected abstract void BindHandles(T mianComponent);
    protected abstract void HandleInternalSetup(T mainComponent);
}
