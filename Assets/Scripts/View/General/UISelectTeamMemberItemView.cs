using UnityEngine;
using UnityEngine.UI;

public class UISelectTeamMemberItemView : UIItemController
{
    [SerializeField] private Image _imgCharacter;
    [SerializeField] private CharacterArtType _characterArtType;
    [SerializeField] private GameObject _activeView;
    [SerializeField] private GameObject _inativeView;
    [SerializeField] private Button _selectedButton;

    [Header("(Optional)")]
    [SerializeField] private Image _imgCharacterBackground;

    private bool _hasInitialized = false;

    protected override void HandleInit(object obj)
    {
        if (obj == null)
        {
            SetActive(false);
        }
        else
        {
            SetActive(true);

            CharacterUnit = obj as CharacterUnit;

            _imgCharacter.sprite = CharacterUnit.GetArt(_characterArtType);
            _imgCharacterBackground.color = CharacterUnit.BaseCharacterSO.ColorBackground;
        }

        if (!_hasInitialized)
        {
            _selectedButton.onClick.AddListener(SelectItem);

            _hasInitialized = true;
        }
    }

    public void SetActive(bool isActive)
    {
        _activeView.SetActive(isActive);
        _inativeView.SetActive(!isActive);

        if (!isActive)
            CharacterUnit = null;
    }


    public CharacterUnit CharacterUnit { get; private set; } = null;
}
