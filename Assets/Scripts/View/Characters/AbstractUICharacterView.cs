using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractUICharacterView: UIItemController
{
    [Header("AbstractUICharacterView Fields")]
    [SerializeField] private Image _imgCharacterView;
    [SerializeField] private CharacterArtType _characterArtType;

    [Header("(optional)")]
    [SerializeField] private Button _btnButton;
    [SerializeField] private TextMeshProUGUI _txtName;
    [SerializeField] private Image _imgColorBackground;
    [SerializeField] private TextMeshProUGUI _txtIndividualityTitle;
    [SerializeField] private TextMeshProUGUI _txtIndividualityDescription;
    [SerializeField] private TextMeshProUGUI _txtBuy;
    [SerializeField] private TextMeshProUGUI _txtSalary;

    [Header("(optionl) Rank")]
    [SerializeField] private Image _imgRankBackground;
    [SerializeField] private TextMeshProUGUI _txtRank;

    private void Awake()
    {
        if (_btnButton != null)
        {
            _btnButton.onClick.AddListener(SelectItem);
        }
    }

    protected void SetCharacterSO(CharacterSO character)
    {
        _imgCharacterView.sprite = character.GetArt(_characterArtType);

        if (_txtName != null)
        {
            _txtName.text = character.Name;
        }

        if (_imgColorBackground != null)
        {
            _imgColorBackground.color = character.ColorBackground;
        }

        if (_txtIndividualityTitle != null && character.Individuality != null)
        {
            _txtIndividualityTitle.text = character.Individuality.IndividualityName;
        }

        if (_txtIndividualityDescription != null && character.Individuality != null)
        {
            _txtIndividualityDescription.text = character.Individuality.IndividualityDescription;
        }

        if (_txtBuy != null)
        {
            _txtBuy.text = character.RecruitmentCost.ToString();
        }

        if (_txtSalary != null)
        {
            _txtSalary.text = character.Salary.ToString();
        }

        if (_imgRankBackground != null && _txtRank != null && character.Rank != null)
        {
            _imgRankBackground.color = character.Rank.BackgroundColor;
            _txtRank.text = character.Rank.Description;
        }
    }
}