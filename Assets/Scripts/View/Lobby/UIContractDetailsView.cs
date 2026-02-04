using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIContractDetailsView : UIItemController
{
    [SerializeField] private TextMeshProUGUI _txtContractTitle;
    [SerializeField] private TextMeshProUGUI _txtContractDescription;
    [SerializeField] private TextMeshProUGUI _txtMoneyReward;
    [SerializeField] private TextMeshProUGUI _txtExperienceReward;
    [SerializeField] private Image _imgRankBackground;
    [SerializeField] private TextMeshProUGUI _txtRank;

    private ContractMissionRuntime _contractRuntime;

    protected override void HandleInit(object obj)
    {
        _contractRuntime = obj as ContractMissionRuntime;

        var reward = _contractRuntime.GetReward();

        _txtContractTitle.text = _contractRuntime.ContractMisionSO.Name;
        _txtContractDescription.text = _contractRuntime.ContractMisionSO.Description;
        _txtMoneyReward.text = reward.Money.ToString();
        _txtExperienceReward.text = reward.Experience.ToString();
        _imgRankBackground.color = _contractRuntime.ContractMisionSO.Rank.BackgroundColor;
        _txtRank.text = _contractRuntime.ContractMisionSO.Rank.Description;
    }

    public ContractMissionRuntime ContractMissionRuntime => _contractRuntime;
}
