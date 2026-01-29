using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterViewController : UIItemController
{
    [SerializeField] private Image _imgCharacterView;
    [SerializeField] private CharacterArtType _characterArtType;

    [Header("(optional)")]
    [SerializeField] private UIRadarChartController _radarChartStatController;
    [SerializeField] private Button _btnButton;
    [SerializeField] private TextMeshProUGUI _txtName;
    [SerializeField] private Image _imgColorBackground;
    [SerializeField] private TextMeshProUGUI _txtIndividualityTitle;
    [SerializeField] private TextMeshProUGUI _txtIndividualityDescription;
    [SerializeField] private TextMeshProUGUI _txtSalary;

    private CharacterUnit _characterUnit;

    private void Awake()
    {
        if (_btnButton != null)
        {
            _btnButton.onClick.AddListener(SelectItem);
        }
    }

    protected override void HandleInit(object obj)
    {
        _characterUnit = obj as CharacterUnit;

        _imgCharacterView.sprite = _characterUnit.GetArt(_characterArtType);

        if (_txtName != null)
        {
            _txtName.text = _characterUnit.Name;
        }

        if (_radarChartStatController != null)
        {
            var values = _characterUnit.StatManager.GetValues();
            _radarChartStatController.UpdateStats(values);
        }

        if (_imgColorBackground != null)
        {
            _imgColorBackground.color = _characterUnit.HeroBackgroundColor;
        }

        if (_txtIndividualityTitle != null && _characterUnit.BaseCharacterSO.Individuality != null)
        {
            _txtIndividualityTitle.text = _characterUnit.BaseCharacterSO.Individuality.IndividualityName;
        }

        if (_txtIndividualityDescription != null && _characterUnit.BaseCharacterSO.Individuality != null)
        {
            _txtIndividualityDescription.text = _characterUnit.BaseCharacterSO.Individuality.IndividualityDescription;
        }

        if (_txtSalary != null)
        {
            _txtSalary.text = _characterUnit.BaseCharacterSO.Salary.ToString();
        }
    }

    public CharacterUnit CharacterUnit => _characterUnit;
}