using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        int itemCount = _usePaging ? _maxItemsPerPage : newItems.Count;

        for (int i = 0; i < itemCount; i++)
        {
            var instance = GameObject.Instantiate(_itemPrefab, _itemParent);

            if (i < newItems.Count)
            {
                instance.Init(newItems[i], onSelectItem);
            }

            _controllers.Add(instance);
        }

        if (_usePaging)
        {
            _btnNext.onClick.RemoveAllListeners();
            _btnNext.onClick.AddListener(NextPage);

            _btnPrev.onClick.RemoveAllListeners();
            _btnPrev.onClick.AddListener(PrevPage);

            _currentPage = 0;
            _maxPages = Mathf.FloorToInt((float)newItems.Count / (_maxItemsPerPage + 1));
        }

        UpdatePage();
    }

    private void UpdatePage()
    {
        if (_usePaging)
        {
            _txtCurrentPages.text = $"{_currentPage + 1} / {_maxPages + 1}";

            _btnPrev.interactable = _currentPage > 0;
            _btnNext.interactable = _currentPage < _maxPages;

            int itemIndex;
            int beginIndex = _currentPage * _maxItemsPerPage;

            for (int i = 0; i < _maxItemsPerPage; i++)
            {
                itemIndex = beginIndex + i;

                if (itemIndex < _items.Count)
                {
                    _controllers[i].SetItem(_items[itemIndex]);
                    _controllers[i].gameObject.SetActive(true);
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

    public void AddItem(UIItemController controller)
    {
        if (!_controllers.Contains(controller))
        {
            _controllers.Add(controller);
        }
    }

    public void RmvItem(UIItemController controller, Transform newParent=null)
    {
        if (_controllers.Contains(controller))
        {
            _controllers.Remove(controller);
        }
    }

    public List<UIItemController> GetItems() => _controllers;
    public Transform Parent => _itemParent;
}
