using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMissionInfoViewController : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private List<Image> _imgCallerArts;
    [SerializeField] private Image _imgMissionBackground;
    [SerializeField] private TextMeshProUGUI _txtMissionType;
    [SerializeField] private TextMeshProUGUI _txtMissionDescription;

    private void Start()
    {
        Close();
    }

    public void UpdateMissionInfo(MissionUnit missionUnit)
    {
        _view.SetActive(true);

        _imgCallerArts.ForEach(img => img.sprite = missionUnit.MissionSO.ClientArt);
        _imgMissionBackground.sprite = missionUnit.MissionSO.EnvironmentArt;
        _txtMissionType.text = missionUnit.MissionSO.Type.ToString();
        _txtMissionDescription.text = missionUnit.MissionSO.Description;
    }

    public void Close()
    {
        _view.SetActive(false);
    }

    public void CloseWithoutNotify()
    {
        _view.SetActive(false);
    }
}
