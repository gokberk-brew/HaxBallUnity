using System.Collections;
using System.Collections.Generic;
using Quantum;
using UnityEngine;
using UnityEngine.Serialization;
using Input = UnityEngine.Input;

public class UIManager : MonoBehaviour
{
    [FormerlySerializedAs("goalUIHandler")] [SerializeField] private InGameUIHandler inGameUIHandler;
    [SerializeField] private LobbyUIHandler lobbyUIHandler;
    
    void Start()
    {
        QuantumEvent.Subscribe<EventOnSystemInitialized>(this, Init);
        QuantumEvent.Subscribe<EventOnGameEnded>(this, OnGameEnded);
    }

    private void Init(EventOnSystemInitialized callback)
    {
        var game = QuantumRunner.DefaultGame;
        var frame = game.Frames.Verified;

        if (frame.TryGetSingleton<GameState>(out var gameState))
        {
            lobbyUIHandler.gameObject.SetActive(!gameState.IsGameActive);
        }
    }
    
    private void OnGameEnded(EventOnGameEnded callback)
    {
        inGameUIHandler.Reset();
        lobbyUIHandler.gameObject.SetActive(true);
    }

    public void OnLobbyButtonPressed()
    {
        lobbyUIHandler.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleLobby();
        }
    }

    private void ToggleLobby()
    {
        lobbyUIHandler.gameObject.SetActive(!lobbyUIHandler.gameObject.activeInHierarchy);
    }
}
