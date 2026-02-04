using UnityEngine;
using UnityEngine.UI;

public class UILobbyContractsScreenView : AbstractScreen
{
    [SerializeField] private Button _btnShowAvailableContracts;
    [SerializeField] private Button _btnShowAvailable;
    [SerializeField] private UIListDisplay _contractsListDisplay;
    [SerializeField] private UIListDisplay _characterListDisplay;
    [SerializeField] private UIContractDetailsView _selectedContractDetailsView;
    [SerializeField] private UISelectTeamView _selectedTeamView;

    [SerializeField] private GameObject _selectTeamPlaceHolder;

    private bool _showOngoindContracts = false;

    protected override void InitScreen()
    {
        
        UpdateContractList();
        UpdateCharacterList();
        
        _selectTeamPlaceHolder.SetActive(true);
    }

    private void UpdateCharacterList()
    {
        var gameState = GameManager.Instance.GameState;

        _characterListDisplay.SetItems(gameState.Company.AvailableCharacters, HandleCharacterSelected);
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

    private void HandleCharacterSelected(UIItemController controller)
    {
        var character = controller.GetItem<CharacterUnit>();

        _selectedTeamView.AddMember(character);
    }

    private void HandleContractSeleted(UIItemController controller)
    {
        var contractData = controller.GetItem<ContractMissionRuntime>();

        if (contractData == _selectedContractDetailsView.ContractMissionRuntime) return;

        _selectedContractDetailsView.SetItem(contractData);

        _selectedTeamView.Setup(contractData.ContractMisionSO.MaxTeamSize, HandleMemberSelected);

        _selectTeamPlaceHolder.SetActive(false);
    }

    private void HandleMemberSelected(CharacterUnit character)
    {

    }
}
