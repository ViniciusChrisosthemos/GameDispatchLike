using UnityEngine;
using UnityEngine.UI;

public class UIBattleCharacterView : MonoBehaviour
{
    [SerializeField] private Image _imgCharacterArt;
    [SerializeField] private Image _imgHealthBar;

    [SerializeField] private GameObject _killedChracterOverlay;

    public void Init(BattleCharacter character)
    {
        _imgCharacterArt.sprite = character.BaseCharacter.FaceArt;
        _imgHealthBar.fillAmount = 1f;

        _killedChracterOverlay.SetActive(false);
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
}
