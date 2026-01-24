using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSpot : MonoBehaviour, ITimelineElement, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("References")]
    public Transform ActionSelectionCameraSpot;
    public Transform SkillSelectionCameraSpot;
    public Transform ActionSelectionCanvasSpot;
    public Transform SkillSelectionCanvasSpot;
    public Transform TargetPosition;
    public Slider SliderHPBar;
    [SerializeField] private Transform _modelParent;

    [Header("Animation")]
    [SerializeField] private Animator _characterAnimator;
    [SerializeField] private string _takeDamageTriggerName = "TakeDamage";
    [SerializeField] private string _dieTriggerName = "Die";


    [Header("Events")]
    public UnityEvent<CharacterSpot> OnCharacterHoverEnter;
    public UnityEvent<CharacterSpot> OnCharacterHoverExit;
    public UnityEvent<CharacterSpot> OnCharacterSelected;

    private bool _isPlayerCharacter;
    private BattleCharacter _battleCharacter;

    public void SetBattleCharacter(bool isPlayerCharacter, BattleCharacter character)
    {
        _isPlayerCharacter = isPlayerCharacter;
        _battleCharacter = character;

        if (SliderHPBar != null)
        {
            SliderHPBar.maxValue = _battleCharacter.MaxHealth;
            SliderHPBar.value = _battleCharacter.Health;
        }

        _modelParent.ClearChilds();
        
        var instance = Instantiate(_battleCharacter.CharacterUnit.BaseCharacterSO.Model, _modelParent);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;
    }

    public int GetPriority()
    {
        return _battleCharacter.GetPriority();
    }

    public bool IsActive()
    {
        return _battleCharacter.IsActive();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnCharacterSelected?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnCharacterHoverEnter?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnCharacterHoverExit?.Invoke(this);
    }

    public void UpdateHP()
    {
        if (SliderHPBar == null) return;

        if (SliderHPBar.value <= 0) return;

        if (SliderHPBar.value != _battleCharacter.Health)
        {
            if (_battleCharacter.Health <= 0)
            {
                _characterAnimator.SetTrigger(_dieTriggerName);
            }
            else
            {
                _characterAnimator.SetTrigger(_takeDamageTriggerName);
            }
        }

        SliderHPBar.value = _battleCharacter.Health;
    }

    public BattleCharacter Character => _battleCharacter;
    public bool IsPlayerCharacter => _isPlayerCharacter;
}
