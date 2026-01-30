using UnityEngine;
using UnityEngine.UI;

public class UILobbyContractsScreenView : AbstractScreen
{
    [SerializeField] private Button _btnShowAvailableContracts;
    [SerializeField] private Button _btnShowAvailable;
    [SerializeField] private UIListDisplay _contractsListDisplay;
    [SerializeField] private UIListDisplay _characterListDisplay;
    [SerializeField] private UIContractDetailsView _selectedContractDetailsView;

    private bool _showOngoindContracts = false;

    protected override void InitScreen()
    {
        UpdateContractList();
        UpdateCharacterList();
    }

    private void UpdateCharacterList()
    {
        var gameState = GameManager.Instance.GameState;

        _characterListDisplay.SetItems(gameState.Company.AvailableCharacters, null);
    }

    private void UpdateContractList()
    {
        var gameState = GameManager.Instance.GameState;

        if (_showOngoindContracts)
        {
            _contractsListDisplay.SetItems(gameState.ContractManager.OngoingContracts, null);
        }
        else
        {
            _contractsListDisplay.SetItems(gameState.ContractManager.AvailableContracts, HandleContractSeleted);
        }
    }

    private void HandleContractSeleted(UIItemController controller)
    {
        var contractData = controller.GetItem<ContractMissionRuntime>();
        
        _selectedContractDetailsView.SetItem(contractData);
    }
}
