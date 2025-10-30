using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIItemController : MonoBehaviour
{
    protected object _item;
    protected Action<UIItemController> _onClick;

    public void Init(object obj, Action<UIItemController> controller)
    {
        _item = obj;

        _onClick = controller;

        HandleInit(obj);
    }

    protected abstract void HandleInit(object obj);

    public void SelectItem()
    {
        _onClick?.Invoke(this);
    }

    public T GetItem<T>() => (T)_item;
}
