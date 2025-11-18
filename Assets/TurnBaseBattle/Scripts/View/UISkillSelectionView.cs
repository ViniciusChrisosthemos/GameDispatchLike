using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISkillSelectionView : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private Image _imgCharacter;
    [SerializeField] private Button _btnPass;

    public UnityEvent OnPassAction;

    private void Awake()
    {
        _btnPass.onClick.AddListener(Pass);
    }

    public void SetCharacter(BattleCharacter character)
    {
        _view.SetActive(true);

        _imgCharacter.sprite = character.BaseCharacter.BodyArt;
    }

    public void SetActive(bool v)
    {
        _view.SetActive(v);
    }

    public void Pass()
    {
        OnPassAction?.Invoke();
    }
}
