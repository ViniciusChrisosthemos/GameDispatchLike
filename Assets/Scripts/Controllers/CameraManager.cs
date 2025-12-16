using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UIElements.InputSystem;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private GameObject _globalCamera;
    [SerializeField] private GameObject _globalEventSystem;

    public void EnableGlobalCamera()
    {
        _globalCamera.SetActive(true);
        _globalEventSystem.SetActive(true);
    }

    public void DisableGlobalCamera()
    {
        _globalCamera.SetActive(false);
        _globalEventSystem.SetActive(false);
    }

}
