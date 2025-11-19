using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkillTreeSkillDisplay : UIItemController
{
    [SerializeField] private Image _imgSkillArt;
    [SerializeField] private Image _imgBtnBackground;
    [SerializeField] private Image _imgLevelBackground;
    [SerializeField] private TextMeshProUGUI _txtLevel;
    [SerializeField] private Button _btnButton;

    [SerializeField] private Color _colorNormalLevel = Color.white;
    [SerializeField] private Color _colorMaxLevel = Color.yellow;

    private SkillUnit _skillUnit;

    protected override void HandleInit(object obj)
    {
        _skillUnit = obj as SkillUnit;

        var currentLevel = _skillUnit.CurrentLevel;

        _imgSkillArt.sprite = _skillUnit.Art;
        _txtLevel.text = $"{currentLevel}";
    }

    public void UpdateLevel(bool isMaxLevel)
    {
        _imgBtnBackground.color = isMaxLevel ? _colorMaxLevel : _colorNormalLevel;
        _imgLevelBackground.color = isMaxLevel ? _colorMaxLevel : _colorNormalLevel;
        _txtLevel.color = isMaxLevel ? _colorMaxLevel : _colorNormalLevel;
    }
}
