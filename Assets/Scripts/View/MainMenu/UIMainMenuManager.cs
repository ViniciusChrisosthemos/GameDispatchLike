using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _view;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private UIGuildViewManager _guildViewManager;

    [Header("UI")]
    [SerializeField] private Button _btnNewGame;
    [SerializeField] private Button _btnLoadGame;
    [SerializeField] private Button _btnDeleteGame;
    [SerializeField] private Button _btnQuitGame;
    [SerializeField] private TextMeshProUGUI _txtSelectedSave;

    [Header("UI/Saves")]
    [SerializeField] private Transform _savesParent;
    [SerializeField] private UIItemController _saveItemControllerPrefab;


    [Header("New Game")]
    [SerializeField] private GameObject _newGameView;
    [SerializeField] private TMP_InputField _inputNewGameName;
    [SerializeField] private Button _btnCancelNewGame;
    [SerializeField] private Button _btnConfirmNewGame;
    
    private string _currentSave;

    private void Start()
    {
        _btnNewGame.onClick.AddListener(HandleNewGame);
        _btnLoadGame.onClick.AddListener(HandleLoadGame);
        _btnDeleteGame.onClick.AddListener(HandleDeleteSave);

        _btnConfirmNewGame.onClick.AddListener(HandleConfirmNewGame);
        _btnCancelNewGame.onClick.AddListener(HandleCancelNewGame);
        _inputNewGameName.onValueChanged.AddListener(HandleInputGuildNameChanged);

        _btnQuitGame.onClick.AddListener(HandleQuitGame);

        OpenScreen();
    }

    public void Init(List<string> saves)
    {
        _txtSelectedSave.text = "-----";
        _btnLoadGame.interactable = false;
        _btnDeleteGame.interactable = false;

        _savesParent.ClearChilds();

        foreach (var save in saves)
        {
            var controller = Instantiate(_saveItemControllerPrefab, _savesParent);
            controller.Init(save, HandleSaveSelected);
        }

        _newGameView.SetActive(false);
        _txtSelectedSave.text = "";
    }

    private void HandleSaveSelected(UIItemController itemControllerSelected)
    {
        _currentSave = itemControllerSelected.GetItem<string>();

        _txtSelectedSave.text = _currentSave;

        _btnLoadGame.interactable = true;
        _btnDeleteGame.interactable = true;
    }

    private void HandleNewGame()
    {
        _newGameView.gameObject.SetActive(true);

        _inputNewGameName.text = "";
    }

    private void HandleCancelNewGame()
    {
        _newGameView.gameObject.SetActive(false);
    }

    private void HandleConfirmNewGame()
    {
        var guildName = _inputNewGameName.text;

        GameManager.Instance.NewGame(guildName);

        _guildViewManager.OpenScreen();
        CloseScreen();
    }

    private void HandleLoadGame()
    {
        _gameManager.LoadSave(_currentSave);

        _guildViewManager.OpenScreen();
        CloseScreen();
    }

    private void HandleInputGuildNameChanged(string newName)
    {
        _btnConfirmNewGame.interactable = newName != string.Empty;
    }

    private void HandleDeleteSave()
    {
        GameManager.Instance.DeleteSave(_currentSave);

        OpenScreen();
    }

    public void CloseScreen()
    {
        _view.SetActive(false);
    }

    public void OpenScreen()
    {
        _view.SetActive(true);

        Init(GameManager.Instance.GetSaves());
    }

    private void HandleQuitGame()
    {
        Application.Quit();
    }
}
