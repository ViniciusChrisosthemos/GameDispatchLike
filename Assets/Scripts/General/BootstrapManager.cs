using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootstrapManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        CustomSceneManager.Instance.LoadMainMenuScene();
    }
}
