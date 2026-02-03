using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyCandidatesScreenView : AbstractScreen
{
    [SerializeField] private UIListDisplay _candidatesListDisplay;
    [SerializeField] private UICharacterSOView _selectedCharacterController;
    [SerializeField] private Button _btnBuy;
    [SerializeField] private TextMeshProUGUI _txtBuyLabel;
    [SerializeField] private GameObject _characterSelectedPlaceHolder;

    private void Awake()
    {
        _btnBuy.onClick.AddListener(BuyCharacter);
    }

    protected override void InitScreen()
    {
        UpdateCharacterList();
        _characterSelectedPlaceHolder.SetActive(true);
    }

    private void HandleCandidateSelected(UIItemController controller)
    {
        var character = controller.GetItem<CharacterSO>();

        _selectedCharacterController.SetItem(character);

        _txtBuyLabel.text = $"Buy ({character.RecruitmentCost})";
        _characterSelectedPlaceHolder.SetActive(false);

        _btnBuy.interactable = GameManager.Instance.CanAfford(character);
    }

    private void BuyCharacter()
    {
        var character = _selectedCharacterController.CharacterSO;

        GameManager.Instance.BuyCharacter(character);

        _characterSelectedPlaceHolder.SetActive(true);

        UpdateCharacterList();
        UpdateMainScreen();
    }

    private void UpdateCharacterList()
    {
        var availableCharacters = GameManager.Instance.CharacterShopController.GetAvailableCharacters();

        _candidatesListDisplay.SetItems(availableCharacters, HandleCandidateSelected);
    }
}
