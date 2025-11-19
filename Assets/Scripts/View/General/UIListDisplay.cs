using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIListDisplay : MonoBehaviour
{
    [SerializeField] private Transform _itemParent;
    [SerializeField] private UIItemController _itemPrefab;

    [Header("Pages (Optional)")]
    [SerializeField] private bool _usePaging = false;
    [SerializeField] private int _maxItemsPerPage = 10;
    [SerializeField] private Button _btnPrev;
    [SerializeField] private Button _btnNext;
    [SerializeField] private TextMeshProUGUI _txtCurrentPages;

    private List<object> _items;
    private List<UIItemController> _controllers;

    private int _maxPages;
    private int _currentPage;

    public UIListDisplay() { }

    public void SetItems(List<object> newItems, Action<UIItemController> onSelectItem)
    {
        _items = newItems;
        _controllers = new List<UIItemController>();

        _itemParent.ClearChilds();

        int controllersAmount = _usePaging ? _maxItemsPerPage : newItems.Count;

        for (int i = 0; i < controllersAmount; i++)
        {
            var instance = GameObject.Instantiate(_itemPrefab, _itemParent);

            if (i < newItems.Count)
            {
                instance.Init(newItems[i], onSelectItem);
                instance.gameObject.SetActive(true);
            }
            else
            {
                instance.SetCallback(onSelectItem);
                instance.gameObject.SetActive(false);
            }

            _controllers.Add(instance);
        }

        if (_usePaging)
        {
            if (_btnNext != null)
            {
                _btnNext.onClick.RemoveAllListeners();
                _btnNext.onClick.AddListener(NextPage);
            }

            if (_btnPrev != null)
            {
                _btnPrev.onClick.RemoveAllListeners();
                _btnPrev.onClick.AddListener(PrevPage);
            }

            _currentPage = 0;
            _maxPages = Mathf.FloorToInt((float)newItems.Count / (_maxItemsPerPage + 1));

            UpdatePage();
        }
    }

    public void UpdatePage()
    {
        if (_usePaging)
        {
            if (_txtCurrentPages != null) _txtCurrentPages.text = $"{_currentPage + 1} / {_maxPages + 1}";

            if (_btnPrev != null) _btnPrev.interactable = _currentPage > 0;
            if (_btnNext != null) _btnNext.interactable = _currentPage < _maxPages;

            int itemIndex;
            int beginIndex = _currentPage * _maxItemsPerPage;

            for (int i = 0; i < _controllers.Count; i++)
            {
                itemIndex = beginIndex + i;

                _controllers[i].gameObject.SetActive(true);

                if (itemIndex < _items.Count)
                {
                    _controllers[i].SetItem(_items[itemIndex]);
                }
                else
                {
                    _controllers[i].gameObject.SetActive(false);
                }
            }
        }
    }

    private void NextPage()
    {
        _currentPage = Mathf.Min(_currentPage + 1, _maxPages);

        UpdatePage();
    }

    private void PrevPage()
    {
        _currentPage = Mathf.Max(_currentPage - 1, 0);

        UpdatePage();
    }

    public void AddItem(object item)
    {
        _items.Add(item);

        _maxPages = Mathf.FloorToInt((float)_items.Count / (_maxItemsPerPage + 1));

        if (_usePaging) UpdatePage();
    }

    public void RmvItem(object item)
    {
        Debug.Log($"Current Controller SIze {_items.Count}");
        
        _items.Remove(item);

        _maxPages = Mathf.FloorToInt((float)_items.Count / (_maxItemsPerPage + 1));
        _currentPage = Mathf.Min(_currentPage, _maxPages);

        if (_usePaging) UpdatePage();

        Debug.Log($"New Controller SIze {_items.Count} ");
    }

    public List<UIItemController> GetControllers() => _controllers;
    public List<T> GetItems<T>() => _items.Select(h => (T)h).ToList();
    public int Count => _items.Count;

    public Transform Parent => _itemParent;

    private class ItemHolder
    {
        public object Item;
        public UIItemController Controller;

        public ItemHolder(object item, UIItemController controller)
        {
            Item = item;
            Controller = controller;
        }
    }
}
