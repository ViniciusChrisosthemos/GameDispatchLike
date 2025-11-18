using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class UIBattleCharacterView : UIItemController
{
    [SerializeField] private Image _imgCharacterArt;
    [SerializeField] private Image _imgHealthBar;

    [SerializeField] private GameObject _killedChracterOverlay;

    private BattleCharacter _character;

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
}
