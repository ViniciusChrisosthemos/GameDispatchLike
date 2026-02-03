using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyCandidatesScreenView : AbstractScreen
{
    [SerializeField] private UIListDisplay _candidatesListDisplay;
    [SerializeField] private UICharacterSOView _selectedCharacterController;
    [SerializeField] private Button _btnBuy;
    [SerializeField] private TextMeshProUGUI _txtBuyLabel;
    [SerializeField] private GameObject _selectedCharacterPlaceHolder;

    private void Awake()
    {
        _btnBuy.onClick.AddListener(BuyCharacter);
    }

    protected override void InitScreen()
    {
        UpdateList();
        _selectedCharacterPlaceHolder.SetActive(true);
    }

    private void HandleCandidateSelected(UIItemController controller)
    {
        var character = controller.GetItem<CharacterSO>();

        _selectedCharacterController.SetItem(character);

        _txtBuyLabel.text = $"Buy ({character.RecruitmentCost})";
        _btnBuy.interactable = GameManager.Instance.CanAffordCharacter(character);

        _selectedCharacterPlaceHolder.SetActive(false);
    }

    private void BuyCharacter()
    {
        GameManager.Instance.BuyCharacter(_selectedCharacterController.CharacterSO);

        UpdateMainScreen();

        _selectedCharacterPlaceHolder.SetActive(true);
        UpdateList();
    }

    private void UpdateList()
    {
        var availableCharacters = GameManager.Instance.GetAvailableCandidates();

        _candidatesListDisplay.SetItems(availableCharacters, HandleCandidateSelected);
    }
}
