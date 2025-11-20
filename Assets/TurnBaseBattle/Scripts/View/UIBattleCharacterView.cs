using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIBattleCharacterView : UIItemController
{
    [SerializeField] private Image _imgCharacterArt;
    [SerializeField] private Button _btnButton;

    [SerializeField] private GameObject _killedChracterOverlay;
    [SerializeField] private GameObject _targetOverlay;
    [SerializeField] private GameObject _confirmedTargetOverlay;

    [Header("Health Elements")]
    [SerializeField] private TextMeshProUGUI _txtHealth;
    [SerializeField] private Slider _sliderGreenBar;
    [SerializeField] private Slider _sliderRedBar;
    [SerializeField] private float _healthUpdateTime = 0.3f;
    [SerializeField] private float _healthBackgroundDelay = 0.5f;

    [Header("Events")]
    public UnityEvent<UIBattleCharacterView> OnSelected;


    private BattleCharacter _character;
    private bool _isTarget = false;

    private void Start()
    {
        Clear();

        _btnButton.onClick.AddListener(() =>
        {
            SelectItem();
            OnSelected?.Invoke(this);
        });
    }

    public void Clear()
    {
        if (_character != null)
        {
            _killedChracterOverlay.SetActive(!_character.IsAlive());
        }
        else
        {
            _killedChracterOverlay.SetActive(false);
        }
            
        _targetOverlay.SetActive(false);
        _confirmedTargetOverlay.SetActive(false);

        _isTarget = false;
    }

    public void UpdateHealth()
    {
        //Animation
        Sequence seq = DOTween.Sequence();

        var currentValue = _sliderGreenBar.value;
        var nextValue = _character.GetNormalizedHealth();

        _sliderGreenBar.value = _sliderRedBar.value = currentValue;

        seq.Append(_sliderGreenBar.DOValue(nextValue, _healthUpdateTime).SetEase(Ease.InQuad));
        seq.AppendCallback(() =>
        {
            _txtHealth.text = $"{_character.Health}/{_character.MaxHealth}";
        });
        seq.Append(_sliderRedBar.DOValue(nextValue, _healthBackgroundDelay).SetEase(Ease.InQuad));
    }

    public void KillCharacter()
    {
        _killedChracterOverlay.SetActive(true);
    }

    protected override void HandleInit(object obj)
    {
        _character = (BattleCharacter)obj;

        _imgCharacterArt.sprite = _character.BaseCharacter.FaceArt;
        
        _killedChracterOverlay.SetActive(false);

        _sliderRedBar.value = 1f;
        _sliderGreenBar.value = 1f;
        _txtHealth.text = $"{_character.Health}/{_character.MaxHealth}";
    }

    public void SetTarget()
    {
        _targetOverlay.SetActive(true);

        _confirmedTargetOverlay.SetActive(false);
    }

    public void SetConfirmedTarget()
    {
        _targetOverlay.SetActive(false);
        _confirmedTargetOverlay.SetActive(true);

        _isTarget = true;
    }

    public void ClearSelection()
    {
        _targetOverlay.SetActive(false);
        _confirmedTargetOverlay.SetActive(_isTarget);
    }

    public BattleCharacter BattleCharacter => _character;
}
