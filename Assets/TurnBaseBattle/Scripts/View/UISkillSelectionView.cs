using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISkillSelectionView : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private Image _imgCharacter;
    [SerializeField] private Button _btnPass;
    [SerializeField] private UIListDisplay _skillListDisplay;

    private List<UIBattleCharacterView> _playerBattleCharactersControllers;
    private List<UIBattleCharacterView> _enemyBattleCharactersControllers;

    public UnityEvent OnPassAction;

    private enum SelectionState
    {
        SelectingSkill,
        SelectingTarget
    }

    private void Awake()
    {
        _btnPass.onClick.AddListener(Pass);
    }

    public void SetTeams(List<UIBattleCharacterView> playerCahractersControllers, List<UIBattleCharacterView> enemyCharactersControllers)
    {
        _playerBattleCharactersControllers = playerCahractersControllers;
        _enemyBattleCharactersControllers = enemyCharactersControllers;

        _playerBattleCharactersControllers.ForEach(c => c.OnSelected.AddListener(HandleCharacterViewSelected));
        _enemyBattleCharactersControllers.ForEach(c => c.OnSelected.AddListener(HandleCharacterViewSelected));
    }

    public void SetCharacter(BattleCharacter character)
    {
        _view.SetActive(true);

        _imgCharacter.sprite = character.BaseCharacter.BodyArt;

        var items = character.BaseCharacter.Skills.Select(s => (object)s).ToList();
        _skillListDisplay.SetItems(items, HandleSkillSelected);
    }

    private void HandleSkillSelected(UIItemController controller)
    {
        var skill = controller.GetItem<BaseSkillSO>();

        switch (skill.SkillTargetType)
        {
            case SkillTargetType.Ally: HandleTargetSkill(skill, _playerBattleCharactersControllers); break;
            case SkillTargetType.Enemy: HandleTargetSkill(skill, _enemyBattleCharactersControllers); break;
            default: break;
        }
    }

    private void HandleTargetSkill(BaseSkillSO skill, List<UIBattleCharacterView> targetControllers)
    {
        targetControllers.ForEach(c => c.SetTarget());
    }

    private void HandleCharacterViewSelected(UIBattleCharacterView characterView)
    {

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
