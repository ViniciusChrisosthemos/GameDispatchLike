using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RollDiceController : MonoBehaviour
{
    [SerializeField] private GameObject _diceView;
    [SerializeField] private DiceThrower _diceThrower;
    [SerializeField] private Transform _diceParent;

    private int _diceAmount;
    private Dictionary<int, DiceValueSO> _diceValues;

    private Action<List<DiceValueSO>> _allDiceRolledCallback;

    public void RollDices(int diceAmount, Action<List<DiceValueSO>> values)
    {
        _diceView.SetActive(true);
        _diceValues = new Dictionary<int, DiceValueSO>();

        _diceParent.ClearChilds();
        _diceThrower.RollDices(diceAmount, _diceParent, HandleDiceResult);

        _diceAmount = diceAmount;
        _allDiceRolledCallback = values;
    }

    public void SetActive(bool v)
    {
        _diceView.SetActive(v);
    }

    private void HandleDiceResult(int diceID, DiceValueSO diceValue)
    {
        _diceValues.Add(diceID, diceValue);

        Debug.Log($"HandleDiceResult {diceID}  {diceValue}  {_diceValues.Count}");

        if (_diceValues.Count >= _diceAmount)
        {
            var values = _diceValues.Values.ToList();

            _allDiceRolledCallback?.Invoke(values);
        }
    }
}
