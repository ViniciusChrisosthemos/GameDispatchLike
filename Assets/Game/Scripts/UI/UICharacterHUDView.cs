using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UICharacterHUDView : AbstractSubComponent<BattleManager>
{
    [Header("UI References")]
    [SerializeField] private GameObject _actionSelectionView;
    [SerializeField] private GameObject _skillSelectionView;
    [SerializeField] private Transform _defaultTargetCameraPosition;

    [Header("UI References / Actions Selection View")]
    [SerializeField] private Button _btnPassTurn;
    [SerializeField] private Button _btnRollDices;

    [Header("UI References / Skill Selection View")]
    [SerializeField] private UIListDisplay _skillsListDisplay;

    [Header("UI References / target Selection")]
    [SerializeField] private TargetSelectionController _targetSelectionController;

    private SelectionState _currentState;
    private CharacterSpot _currentCharacter;
    private BaseSkillSO _currentSkill;

    private List<CharacterSpot> _enemiesSpots;

    private enum SelectionState
    {
        ActionSelection,
        SkillSelection,
        TargetSelection,
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            HandleEscPressed();
        }
    }

    private void HandleEscPressed()
    {
        switch (_currentState)
        {
            case SelectionState.ActionSelection: break;
            case SelectionState.SkillSelection: SetActionSelection(_currentCharacter); break;
            case SelectionState.TargetSelection: HandleSkillSelection(); break;
        }
    }

    private void HandleCharacterTurnChanged(bool isPlayer, BattleCharacter battleCharacter)
    {
        _currentCharacter = _manager.GetCharacterSpotManager().GetCharacter(battleCharacter);

        SetActionSelection(_currentCharacter);
    }

    private void SetActionSelection(CharacterSpot characterSpot)
    {
        ShowActionSelectionView();

        _manager.GetBattleCameraController().MoveCameraTo(_currentCharacter.ActionSelectionCameraSpot);

        if (characterSpot.IsPlayerCharacter)
        {
            _actionSelectionView.SetActive(true);

            transform.position = _currentCharacter.ActionSelectionCanvasSpot.position;
            transform.rotation = _currentCharacter.ActionSelectionCanvasSpot.rotation;

            _currentState = SelectionState.ActionSelection;
        }
        else
        {
            DisableUI();

            HandleEnemyTurn();
        }
    }

    private async void HandleEnemyTurn()
    {
        //await Task.Delay(1000);
    }

    private void HandleSkillSelection()
    {
        ShowSkillSelectionView();

        _manager.GetBattleCameraController().MoveCameraTo(_currentCharacter.SkillSelectionCameraSpot);

        transform.position = _currentCharacter.SkillSelectionCanvasSpot.position;
        transform.rotation = _currentCharacter.SkillSelectionCanvasSpot.rotation;

        _skillsListDisplay.SetItems(_currentCharacter.Character.BaseCharacter.Skills, HandleTargetSelection);

        _currentState = SelectionState.SkillSelection;
        _targetSelectionController.DisableSelection();
    }

    private void HandleTargetSelection(UIItemController itemController)
    {
        _currentSkill = itemController.GetItem<BaseSkillSO>();

        ShowTargetSelectionView();

        _manager.GetBattleCameraController().MoveCameraTo(_defaultTargetCameraPosition);

        if (_currentSkill.SkillTargetType == SkillTargetType.Enemy && _currentSkill.SkillTargetAmount == SkillTargetAmountType.SingleTarget)
        {
            _targetSelectionController.SetSingleTargetSelection(HandleTargetSelected);
        }
        else
        {
            _targetSelectionController.SetAlltargetSelection(HandleTargetSelected);
        }

        _currentState = SelectionState.TargetSelection;
    }

    private async void HandleTargetSelected(List<CharacterSpot> characterSeleced)
    {
        /*
        _battleController.PlayAction(_currentSkill, characterSeleced);
        
        _currentState = SelectionState.ActionSelection;
        
        await HandleSkillApplied();*/
    }

    private async Task HandleSkillApplied()
    {
        /*
        _enemiesSpots.ForEach(character => character.UpdateHP());

        await Task.Delay(1000);

        _battleController.PassTurn();
        */
    }

    public void ShowActionSelectionView()
    {
        _actionSelectionView.SetActive(true);
        _skillSelectionView.SetActive(false);
    }

    public void ShowSkillSelectionView()
    {
        _actionSelectionView.SetActive(false);
        _skillSelectionView.SetActive(true);
    }

    public void ShowTargetSelectionView()
    {
        _actionSelectionView.SetActive(false);
        _skillSelectionView.SetActive(false);
    }

    private void DisableUI()
    {
        _actionSelectionView.SetActive(false);
        _skillSelectionView.SetActive(false);
    }

    public void PassTurn()
    {
        _manager.GetBattleController().PassAction();
    }

    protected override void BindHandles(BattleManager mianComponent)
    {
        _btnPassTurn.onClick.AddListener(PassTurn);
        _btnRollDices.onClick.AddListener(HandleSkillSelection);

        mianComponent.GetBattleController().OnCharacterTurn.AddListener(HandleCharacterTurnChanged);
    }

    protected override void HandleInternalSetup(BattleManager mainComponent)
    {

        var characterSpots = mainComponent.GetCharacterSpotManager().GetCharacterSpots();

        _targetSelectionController.Init(characterSpots);
        _enemiesSpots = characterSpots.FindAll(c => !c.IsPlayerCharacter);
    }
}
