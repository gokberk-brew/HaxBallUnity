using System.Collections;
using System.Collections.Generic;
using Quantum;
using UnityEngine;
using Input = UnityEngine.Input;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GoalUIHandler goalUIHandler;
    [SerializeField] private LobbyUIHandler lobbyUIHandler;
    
    void Start()
    {
        var game = QuantumRunner.DefaultGame;
        var frame = game.Frames.Verified;

        if (frame.TryGetSingleton<GameState>(out var gameState))
        {
            lobbyUIHandler.gameObject.SetActive(!gameState.IsGameActive);
        }
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
