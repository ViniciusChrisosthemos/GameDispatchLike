using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDeleteItemView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform _mainUIComponent;
    [SerializeField] private GameObject _view;
    [SerializeField] private Button _btnButton;

    [SerializeField] private LayoutElement _layoutElement;

    private void Start()
    {
        _view.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _view.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _view.SetActive(false);
    }

    public void SetCallback(Action callback)
    {
        _btnButton.onClick.RemoveAllListeners();
        _btnButton.onClick.AddListener(() => callback?.Invoke());
    }

    public void SetLayoutElementToIgnoreLayout(bool toIgnore)
    {
        _layoutElement.ignoreLayout = toIgnore;
    }

    public RectTransform GetMainUIComponent()
    {
        return _mainUIComponent;
    }
}
