using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIBattleHUDView : AbstractSubComponent<BattleManager>
{
    [Header("UI References")]
    [SerializeField] private GameObject _view;
    [SerializeField] private GameObject _environmentView;
    [SerializeField] private UIListDisplay _characterListDisplay;
    [SerializeField] private UIListDisplay _timelineListDisplay;

    private void Start()
    {
        _view.SetActive(false);
        _environmentView.SetActive(false);
    }

    private void HandleCharacterTurnChanged(bool isPlayerCharacter, BattleCharacter currentCharacter)
    {
        var battleController = _manager.GetBattleController();

        var characters = battleController.GetCharacterOrderInTurn();
        characters.Insert(0, currentCharacter);

        //_timelineListDisplay.SetItems(characters, null);
    }

    public void Init(List<BattleCharacter> playerCharacters)
    {
        _characterListDisplay.SetItems(playerCharacters, null);

        _view.SetActive(true);
        _environmentView.SetActive(true);
    }

    protected override void BindHandles(BattleManager mainComponent)
    {
        var battleController = mainComponent.GetBattleController();

        battleController.OnCharacterTurn.AddListener(HandleCharacterTurnChanged);
        battleController.OnBattleEnd.AddListener((isPlayerWinner) => HandleBattleEnd());

        Debug.Log("UIBattleHUDView");
    }

    protected override void HandleInternalSetup(BattleManager mainComponent)
    {
        Init(mainComponent.GetBattleController().PlayerCharacters);
    }

    private void HandleBattleEnd()
    {
        Debug.Log("UIBattleHUDView Battle Ended");

        _view.SetActive(false);
        _environmentView.SetActive(false);

    }
}
