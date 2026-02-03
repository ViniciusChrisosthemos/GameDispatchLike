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

    protected override void HandleInit(object obj)
    {
        var contractRuntime = obj as ContractMissionRuntime;

        var reward = contractRuntime.GetReward();

        _txtContractTitle.text = contractRuntime.ContractMisionSO.Name;
        _txtContractDescription.text = contractRuntime.ContractMisionSO.Description;
        _txtMoneyReward.text = reward.Money.ToString();
        _txtExperienceReward.text = reward.Experience.ToString();
        _imgRankBackground.color = contractRuntime.ContractMisionSO.Rank.BackgroundColor;
        _txtRank.text = contractRuntime.ContractMisionSO.Rank.Description;
    }
}
