using System;
using System.Collections.Generic;

public class ContractManager
{
    private List<ContractMissionRuntime> _availableContracts;
    private List<ContractMissionRuntime> _ongoingContracts;

    public ContractManager(List<ContractMissionRuntime> availableContracts, List<ContractMissionRuntime> ongoingContracts)
    {
        _availableContracts = availableContracts;
        _ongoingContracts = ongoingContracts;
    }

    public void UpdateContracts(int currentDay)
    {
        _availableContracts.ForEach(contract => contract.UpdateContract(currentDay));
        _ongoingContracts.ForEach(contract => contract.UpdateContract(currentDay));

        _availableContracts = _availableContracts.FindAll(contract => !contract.IsExpired);
    }

    public List<ContractMissionRuntime> OngoingContracts => _ongoingContracts;

    public List<ContractMissionRuntime> AvailableContracts => _availableContracts;
}
