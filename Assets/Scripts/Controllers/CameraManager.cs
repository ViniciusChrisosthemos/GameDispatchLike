using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private Camera _globalCamera;

    public void EnableGlobalCamera()
    {
        _globalCamera.gameObject.SetActive(true);
    }

    public void DisableGlobalCamera()
    {
        _globalCamera.gameObject.SetActive(false);
    }
}
