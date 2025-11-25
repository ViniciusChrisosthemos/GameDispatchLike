using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractIndividualityView : MonoBehaviour
{
    [SerializeField] private GameObject _view;

    public void OpenView()
    {
        _view.SetActive(true);

        UpdateView();
    }

    public void CloseView()
    {
        _view.SetActive(false);
    }

    public abstract void OnTurnStart();
    public abstract void OnTurnEnd();

    public abstract void InitView(IBattleCharacter character, AbstractIndividuality individuality);
    public abstract void UpdateView();
}
