using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyCandidatesScreenView : AbstractScreen
{
    [SerializeField] private UIListDisplay _candidatesListDisplay;
    [SerializeField] private UICharacterViewController _selectedCharacterController;
    [SerializeField] private Button _btnBuy;
    [SerializeField] private TextMeshProUGUI _txtBuyLabel;

    private void Awake()
    {
        _btnBuy.onClick.AddListener(BuyCharacter);
    }

    protected override void InitScreen()
    {
        var gameState = GameManager.Instance.GameState;

        var candidates = gameState.Candidates;
        _candidatesListDisplay.SetItems(candidates, HandleCandidateSelected);
    }

    private void HandleCandidateSelected(UIItemController controller)
    {
        var character = controller.GetItem<CharacterUnit>();

        _selectedCharacterController.SetItem(character);

        _txtBuyLabel.text = $"Buy ({character.BaseCharacterSO.RecruitmentCost})";
    }

    private void BuyCharacter()
    {
        if (_selectedCharacterController.CharacterUnit == null) return;
        

    }
}
