using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class UIBattleCharacterView : UIItemController
{
    [SerializeField] private Image _imgCharacterArt;
    [SerializeField] private Image _imgHealthBar;
    [SerializeField] private Button _btnButton;

    [SerializeField] private GameObject _killedChracterOverlay;
    [SerializeField] private GameObject _targetOverlay;
    [SerializeField] private GameObject _confirmedTargetOverlay;

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

    public void UpdateHealth(BattleCharacter character)
    {
        _imgHealthBar.fillAmount = character.GetNormalizedHealth();
        
        if (_imgHealthBar.fillAmount >= 0.67f)
        {
            _imgHealthBar.color = Color.green;
        }
        else if (_imgHealthBar.fillAmount >= 0.34f)
        {
            _imgHealthBar.color = Color.yellow;
        }
        else
        {
            _imgHealthBar.color = Color.red;
        }
    }

    public void KillCharacter()
    {
        _killedChracterOverlay.SetActive(true);
    }

    protected override void HandleInit(object obj)
    {
        _character = (BattleCharacter)obj;

        _imgCharacterArt.sprite = _character.BaseCharacter.FaceArt;
        _imgHealthBar.fillAmount = 1f;

        _killedChracterOverlay.SetActive(false);
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
