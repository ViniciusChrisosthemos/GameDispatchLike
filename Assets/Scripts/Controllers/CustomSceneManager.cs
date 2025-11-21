using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : Singleton<CustomSceneManager>
{
    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(Scenes.MainMenuScene);
    }

    public void LoadLobbyScene()
    {
        SceneManager.LoadScene(Scenes.LobbyScene);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(Scenes.GameScene);
    }

    public void LoadBattleScene()
    {
        SceneManager.LoadScene(Scenes.BattleScene, LoadSceneMode.Additive);
    }

    public void UnloadBattleScene()
    {
        SceneManager.UnloadSceneAsync(Scenes.BattleScene);
    }
}
