using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMissionInfoController : MonoBehaviour
{
    [SerializeField] private GameObject _view;

    public void OpenScreen(MissionUnit mission)
    {
        _view.SetActive(true);
    }

    public void CloseScreen()
    {
        _view.SetActive(false);
    }
}
