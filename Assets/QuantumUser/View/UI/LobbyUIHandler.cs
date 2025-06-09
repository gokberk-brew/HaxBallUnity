using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Quantum;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class LobbyUIHandler : MonoBehaviour
{
    [SerializeField] Transform BlueContainer;
    [SerializeField] Transform RedContainer;
    [SerializeField] Transform SpectatorContainer;
    [SerializeField] private GameObject InGameButtonsParent;
    [SerializeField] PlayerUI playerUIPrefab;
    [SerializeField] List<PlayerUI> playerUIList = new List<PlayerUI>();
    
    [Header("Buttons")]
    [SerializeField] private Button RedButton;
    [SerializeField] private Button SpecButton;
    [SerializeField] private Button BlueButton;
    [SerializeField] private Button StartButton;
    [SerializeField] private Button PauseButton;
    [SerializeField] private Button StopButton;
    
    
    [Header("Dropdowns")]
    [SerializeField] private TMP_Dropdown TimeDropdown;
    [SerializeField] private TMP_Dropdown ScoreDropdown;
    
    private bool _isServerUIUpdate;
    private bool _uiReady;
    
    private void Start()
    {
        QuantumEvent.Subscribe<EventOnPlayerJoined>(this, OnPlayerJoined);
        QuantumEvent.Subscribe<EventOnPlayerLeft>(this, OnPlayerLeft);
        QuantumEvent.Subscribe<EventOnPlayerChangeTeam>(this, OnPlayerChangeTeam);
        QuantumEvent.Subscribe<EventOnGameStarted>(this, OnGameStarted);
        QuantumEvent.Subscribe<EventOnGameEnded>(this, OnGameEnded);
        QuantumEvent.Subscribe<EventOnScoreDropdownChanged>(this, UpdateScoreDropdown);
        QuantumEvent.Subscribe<EventOnTimeDropdownChanged>(this, UpdateTimeDropdown);
        QuantumEvent.Subscribe<EventOnSystemInitialized>(this, InitUI);
        
        RedButton.onClick.AddListener(() => PlayerTeamUpdateRequest(Team.Left));
        SpecButton.onClick.AddListener(() => PlayerTeamUpdateRequest(Team.Spec));
        BlueButton.onClick.AddListener(() => PlayerTeamUpdateRequest(Team.Right));
        StartButton.onClick.AddListener(OnStartGameButtonClicked);
        TimeDropdown.onValueChanged.AddListener( HandleTimeDropdownValueChange);
        ScoreDropdown.onValueChanged.AddListener(HandleScoreDropdownValueChange);
        IsQuantumInitialized();
    }

    private void OnGameEnded(EventOnGameEnded callback)
    {
        InGameButtonsParent.SetActive(false);
        StartButton.gameObject.SetActive(true);
    }

    private void IsQuantumInitialized()
    {
        // ✅ Try to access the GameState now in case system already initialized
        var game = QuantumRunner.DefaultGame;
        if (game?.Frames?.Verified != null &&
            game.Frames.Verified.TryGetSingleton<GameState>(out var gameState))
        {
            if(gameState.IsSystemInitialized)
                InitUI(null);
        }
    }

    private void InitUI([CanBeNull] EventOnSystemInitialized callback)
    {
        if(_uiReady)
            return;
        
        var game = QuantumRunner.DefaultGame;
        var frame = game.Frames.Verified;

        if (frame.TryGetSingleton<GameState>(out var gameState))
        {
            _isServerUIUpdate = true;
            ScoreDropdown.value = gameState.ScoreLimit;
            TimeDropdown.value = gameState.TimeLimit;
            _isServerUIUpdate = false;

            if (gameState.IsGameActive)
            {
                InGameButtonsParent.SetActive(true);
                StartButton.gameObject.SetActive(false);
            }
        }

        _uiReady = true;
    }

    private void UpdateTimeDropdown(EventOnTimeDropdownChanged callback)
    {
        _isServerUIUpdate = true;
        TimeDropdown.value = callback.TimeLimit;
        _isServerUIUpdate = false;
    }

    private void UpdateScoreDropdown(EventOnScoreDropdownChanged callback)
    {
        _isServerUIUpdate = true;
        ScoreDropdown.value = callback.ScoreLimit;
        _isServerUIUpdate = false;
    }

    private void HandleScoreDropdownValueChange(int score)
    {
        if(_isServerUIUpdate) return;
        
        QuantumRunner.DefaultGame.SendCommand(new ScoreDropdownCommand()
        {
            ScoreLimit = (byte)score
        });
    }

    private void HandleTimeDropdownValueChange(int time)
    {
        if(_isServerUIUpdate) return;
        
        QuantumRunner.DefaultGame.SendCommand(new TimeDropdownCommand()
        {
            TimeLimit = (byte)time
        });
    }

    private void OnGameStarted(EventOnGameStarted callback)
    {
        InGameButtonsParent.SetActive(true);
        StartButton.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void OnExitButtonClicked()
    {
        gameObject.SetActive(false);
    }

    private void OnStartGameButtonClicked()
    {
        QuantumRunner.Default.Game.SendCommand(new StartGameCommand());
    }

    private void OnPlayerChangeTeam(EventOnPlayerChangeTeam callback)
    {
        var playerUI = playerUIList.FirstOrDefault(x => x.PlayerRef == callback.PlayerRef);
        if (playerUI)
            AssignTeam(playerUI, callback.Team);
    }

    private void AssignTeam(PlayerUI playerUI, Team team)
    {
        switch (team)
        {
            case Team.Left:
                playerUI.gameObject.transform.SetParent(RedContainer);
                break;
            case Team.Right:
                playerUI.gameObject.transform.SetParent(BlueContainer);
                break;
            case Team.Spec:
                playerUI.gameObject.transform.SetParent(SpectatorContainer);
                break;
        }
    }
    
    private void OnPlayerJoined(EventOnPlayerJoined callback)
    {
        var game = QuantumRunner.Default.Game;
        var frame = game.Frames.Predicted;
        
        var playerList = frame.GetSingleton<PlayerStateSingleton>();
        var resolvedList = frame.ResolveList(playerList.List);

        foreach (var playerState in resolvedList)
        {
            if(playerUIList.Any(x => x.PlayerRef == playerState.Player)) continue;
            
            var playerUI = Instantiate(playerUIPrefab, SpectatorContainer);
            playerUI.Init(playerState.Nickname, playerState.Player);
            playerUIList.Add(playerUI);
            AssignTeam(playerUI, playerState.Team);
        }
    }

    private void PlayerTeamUpdateRequest(Team newTeam)
    {
        var localPlayer = QuantumRunner.Default.Game.GetLocalPlayers()[0];

        QuantumRunner.Default.Game.SendCommand(new ChangeTeamCommand {
            Player = localPlayer,
            Team = newTeam
        });
    }
    
    private void OnPlayerLeft(EventOnPlayerLeft callback)
    {
        var playerUI = playerUIList.FirstOrDefault(x => x.PlayerRef == callback.LeftPlayer);
        
        if (playerUI)
        {
            playerUIList.Remove(playerUI);
            Destroy(playerUI.gameObject);
        }
    }
}
