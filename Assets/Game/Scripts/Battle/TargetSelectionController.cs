using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TargetSelectionController : AbstractSubComponent<BattleManager>
{
    [SerializeField] private Transform _targetIconParent;
    [SerializeField] private GameObject _targetIconPrefab;


    private List<CharacterSpot> _enemiesSpots;
    private List<CharacterSpot> _playerSpots;

    private bool _isSelectingTargets = false;
    private bool _isSingeTarget = false;

    private GameObject _singleTargetInstace;

    private Action<List<CharacterSpot>> _onTargetSelectionCallback;

    public void Init(List<CharacterSpot> characterSpots)
    {
        _playerSpots = characterSpots.Where(item => item.IsPlayerCharacter).ToList();
        _enemiesSpots = characterSpots.Where(item => !item.IsPlayerCharacter).ToList();

        foreach (var spot in _enemiesSpots)
        {
            spot.OnCharacterSelected.AddListener(HandleCharacterSelected);
            spot.OnCharacterHoverEnter.AddListener(HandleCharacterHoverEnter);
            spot.OnCharacterHoverExit.AddListener(HandleCharacterHoverExit);
        }

        _singleTargetInstace = null;
        _targetIconParent.ClearChilds();
    }

    public void SetSingleTargetSelection(Action<List<CharacterSpot>> callback)
    {
        _isSelectingTargets = true;
        _isSingeTarget = true;

        _singleTargetInstace = Instantiate(_targetIconPrefab, _targetIconParent);
        _singleTargetInstace.SetActive(false);

        _onTargetSelectionCallback = callback;
    }

    public void SetAlltargetSelection(Action<List<CharacterSpot>> callback)
    {
        _isSelectingTargets = true;
        _isSingeTarget = false;

        foreach (var spot in _enemiesSpots)
        {
            var targetIconInstance = Instantiate(_targetIconPrefab, _targetIconParent);
            targetIconInstance.transform.position = spot.TargetPosition.position;
        }

        _onTargetSelectionCallback = callback;
    }

    private void HandleCharacterSelected(CharacterSpot characterSpot)
    {
        if (!_isSelectingTargets) return;

        var targets = new List<CharacterSpot>();

        if (_isSingeTarget)
        {
            targets.Add(characterSpot);
        }
        else
        {
            targets.AddRange(_enemiesSpots);
        }

        _onTargetSelectionCallback?.Invoke(targets);

        DisableSelection();
    }

    private void HandleCharacterHoverEnter(CharacterSpot characterSpot)
    {
        if (!_isSelectingTargets) return;

        if (_isSingeTarget)
        {
            _singleTargetInstace.SetActive(true);
            _singleTargetInstace.transform.position = characterSpot.TargetPosition.position;
        }
    }

    private void HandleCharacterHoverExit(CharacterSpot characterSpot)
    {
        if (!_isSelectingTargets) return;

        if (_isSingeTarget)
        {
            _singleTargetInstace.SetActive(false);
        }
    }

    public void DisableSelection()
    {
        _isSelectingTargets = false;
        _targetIconParent.ClearChilds();
    }

    protected override void BindHandles(BattleManager mianComponent)
    {
        return;
    }

    protected override void HandleInternalSetup(BattleManager mainComponent)
    {
        var characterSpots = mainComponent.GetCharacterSpotManager().GetCharacterSpots();

        Init(characterSpots);
    }
}
