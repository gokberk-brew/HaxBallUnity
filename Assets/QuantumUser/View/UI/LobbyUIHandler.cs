using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Quantum;
using UnityEngine;
using UnityEngine.Serialization;

public class LobbyUIHandler : MonoBehaviour
{
    [SerializeField] Transform BlueContainer;
    [SerializeField] Transform RedContainer;
    [SerializeField] Transform SpectatorContainer;
    [SerializeField] PlayerUI playerUIPrefab;
    [SerializeField] List<PlayerUI> playerUIs = new List<PlayerUI>();
    
    private void Start()
    {
        QuantumEvent.Subscribe<EventOnPlayerJoined>(this, OnPlayerJoined);
        QuantumEvent.Subscribe<EventOnPlayerLeft>(this, OnPlayerLeft);
    }
    private void OnPlayerJoined(EventOnPlayerJoined callback)
    {
        var game = QuantumRunner.Default.Game;
        var frame = game.Frames.Verified;
        
        var playerList = frame.GetSingleton<PlayerStateSingleton>();
        var resolvedList = frame.ResolveList(playerList.List);

        foreach (var playerState in resolvedList)
        {
            if(playerUIs.Any(x => x.PlayerRef == playerState.Player))
                continue;
            
            var playerUI = Instantiate(playerUIPrefab, SpectatorContainer);
            playerUI.Init(playerState.Nickname, playerState.Player);
            playerUIs.Add(playerUI);
        }
    }
    
    private void OnPlayerLeft(EventOnPlayerLeft callback)
    {
        // var playerUI = playerUIs.FirstOrDefault(x => x.PlayerRef == callback.PlayerRef);
        //
        // if (playerUI)
        // {
        //     playerUIs.Remove(playerUI);
        //     Destroy(playerUI.gameObject);
        // }
    }
}
