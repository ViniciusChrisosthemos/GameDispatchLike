using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UILobbyViewManager : MonoBehaviour
{
    [Header("Screen References")]
    [SerializeField] private GameObject _view;
    [SerializeField] private Button _btnQuit;

    [Header("Screens")]
    [SerializeField] private UILobbyMenuBarView _uiLobbyMenuBar;
    [SerializeField] private UICalendarView _uiCalendarView;

    [Header("Settings")]
    [SerializeField] private GameSettingsSO _gameSettingsSO;
    [SerializeField] private ScreenConfigurationSO _screenConfigurationSO;

    [Header("Events")]
    public UnityEvent OnScreenOpened;

    private Company _guild;

    private void Start()
    {
        Init();
    }

    public void StartDay()
    {
        CustomSceneManager.Instance.LoadGameScene();
    }

    public void QuitGame()
    {
        GameManager.Instance.Quit();
    }

    public void Init()
    {
        _uiLobbyMenuBar.OpenScreen();

        var gameState = GameManager.Instance.GameState;

        OnScreenOpened?.Invoke();

        _uiCalendarView.SetNormalDay(gameState.Day, null);

        SoundManager.Instance.PlayMusic(_screenConfigurationSO.MusicBackground, _screenConfigurationSO.MusicVolume, _screenConfigurationSO.InLoop);
    }
}