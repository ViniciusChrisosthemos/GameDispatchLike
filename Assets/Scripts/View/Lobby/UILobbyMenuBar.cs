using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyMenuBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _txtCompanyName;
    [SerializeField] private TextMeshProUGUI _txtBalance;
    [SerializeField] private Slider _sliderReputation;

    [Header("Sub Screens")]
    [SerializeField] private AbstractScreen _defaultScreen;

    private AbstractScreen _currentScreen;

    public void OpenScreen()
    {
        var gameState = GameManager.Instance.GameState;

        _txtCompanyName.text = gameState.Guild.PlayerName;
        _txtBalance.text = $"Balance: {gameState.Guild.Balance}";
        _sliderReputation.value = gameState.Guild.Reputation;
        
        OpenSubScreen(_defaultScreen);
    }

    public void OpenSubScreen(AbstractScreen screen)
    {
        if (_currentScreen != null)
        {
            _currentScreen.CloseScreen();
        }

        _currentScreen = screen;

        _currentScreen.OpenScreen();
    }
}
