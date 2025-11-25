using TMPro;
using UnityEngine;

public class IzukuMidoriyaIndividualityView : AbstractIndividualityView
{
    [SerializeField] private TextMeshProUGUI _txtOFA;

    private IBattleCharacter _character;
    private IzukuMidoriyaIndividuality _individualityData;

    public override void InitView(IBattleCharacter character, AbstractIndividuality individuality)
    {
        _character = character;
        _individualityData = individuality as IzukuMidoriyaIndividuality;

        _individualityData.Init(_character);

        UpdateView();
    }

    public override void OnTurnEnd()
    {
        _individualityData.OnTurnEnd();
    }

    public override void OnTurnStart()
    {
        _individualityData.OnTurnStart();
        UpdateView();
    }

    public override void UpdateView()
    {
        _txtOFA.text = _individualityData.GetOFALevel().ToString();
    }
}
