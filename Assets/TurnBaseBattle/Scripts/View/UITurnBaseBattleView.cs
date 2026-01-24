using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UITurnBaseBattleView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TurnBaseBattleController _battleController;
    [SerializeField] private BattleCameraController _battleCameraController;
    [SerializeField] private CharacterSpotManager _characterSpotManager;

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

    private void Awake()
    {
        _btnPassTurn.onClick.AddListener(PassTurn);
        _btnRollDices.onClick.AddListener(HandleSkillSelection);

        _battleController.OnCharacterTurn.AddListener(HandleCharacterTurnChanged);
    }

    private void Start()
    {
        var characterSpots = _characterSpotManager.GetCharacterSpots();

        _targetSelectionController.Init(characterSpots);
        _enemiesSpots = characterSpots.FindAll(c => !c.IsPlayerCharacter);
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
        _currentCharacter = _characterSpotManager.GetCharacter(battleCharacter);

        SetActionSelection(_currentCharacter);
    }

    private void SetActionSelection(CharacterSpot characterSpot)
    {
        ShowActionSelectionView();

        _battleCameraController.MoveCameraTo(_currentCharacter.ActionSelectionCameraSpot);

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

        _battleCameraController.MoveCameraTo(_currentCharacter.SkillSelectionCameraSpot);

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

        _battleCameraController.MoveCameraTo(_defaultTargetCameraPosition);

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
        //_battleController.PassTurn();
    }

    public IEnumerator AnimateAction(bool isPlayer, SkillActionResult actionResult)
    {
        yield return null;
    }

    public void RollDices()
    {
        return;
    }
}
