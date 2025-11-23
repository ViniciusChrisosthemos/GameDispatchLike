using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static MissionSO;
using static StatManager;

public class UIRequirementTextViewController : MonoBehaviour
{
    private const string PATTERN = @"<([^>]+)>";

    [SerializeField] private TextMeshProUGUI _txtDescription;
    [SerializeField] private GameObject _txtItemPointer;
    [SerializeField] private Image _imgStat;
    [SerializeField] private List<StatInfoSO> _statInfoSOs;

    private StatType _statType;

    public void SetText(MissionRequirement requirement, Color color)
    {
        var formattedDescription = requirement.Description;

        foreach (var keyword in StringUtils.ExtractStringsInAngleBrackets(requirement.Description, PATTERN))
        {
            var formattedKeyword = string.Format("<color=#{0}>{1}</color>", color.ToHexString(), keyword);

            formattedDescription = formattedDescription.Replace($"<{keyword}>", formattedKeyword);
        }

        _statType = requirement.StatType;
        _txtDescription.text = formattedDescription;
    }

    public void SetText(string label)
    {
        _txtDescription.text = label;
    }

    public void SetStatView(bool showStats)
    {
        if (showStats)
        {
            var statInfo = _statInfoSOs.Find(s => s.Type == _statType);

            _imgStat.sprite = statInfo.Sprite;
            _imgStat.gameObject.SetActive(true);
            _txtItemPointer.SetActive(false);
        }
        else
        {
            _imgStat.gameObject.SetActive(false);
            _txtItemPointer.SetActive(true);
        }
    }
}
