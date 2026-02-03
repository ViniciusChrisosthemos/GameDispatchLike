using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractUICharacterView : UIItemController
{
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

    [Header("(optional) Rank")]
    [SerializeField] private Image _imgRankBackground;
    [SerializeField] private TextMeshProUGUI _txtRank;

    private void Awake()
    {
        if (_btnButton != null)
        {
            _btnButton.onClick.AddListener(SelectItem);
        }
    }

    protected void SetCharacterInfo(CharacterSO characterSO)
    {
        Debug.Log($"{_imgCharacterView} {characterSO}");
        _imgCharacterView.sprite = characterSO.GetArt(_characterArtType);

        if (_txtName != null)
        {
            _txtName.text = characterSO.Name;
        }

        if (_imgColorBackground != null)
        {
            _imgColorBackground.color = characterSO.ColorBackground;
        }

        if (_txtIndividualityTitle != null && characterSO.Individuality != null)
        {
            _txtIndividualityTitle.text = characterSO.Individuality.IndividualityName;
        }

        if (_txtIndividualityDescription != null && characterSO.Individuality != null)
        {
            _txtIndividualityDescription.text = characterSO.Individuality.IndividualityDescription;
        }

        if (_txtBuy != null)
        {
            _txtBuy.text = characterSO.RecruitmentCost.ToString();
        }

        if (_txtSalary != null)
        {
            _txtSalary.text = characterSO.Salary.ToString();
        }

        if (_imgRankBackground != null && _txtRank != null && characterSO.Rank != null)
        {
            _imgRankBackground.color = characterSO.Rank.BackgroundColor;
            _txtRank.text = characterSO.Rank.Description;
        }
    }
}