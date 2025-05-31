using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Quantum;
using UnityEngine;
using UnityEngine.Serialization;
using Button = UnityEngine.UI.Button;

public class LobbyUIHandler : MonoBehaviour
{
    [SerializeField] Transform BlueContainer;
    [SerializeField] Transform RedContainer;
    [SerializeField] Transform SpectatorContainer;
    [SerializeField] PlayerUI playerUIPrefab;
    [SerializeField] List<PlayerUI> playerUIList = new List<PlayerUI>();
    [SerializeField] private Button RedButton;
    [SerializeField] private Button SpecButton;
    [SerializeField] private Button BlueButton;
    
    private void Start()
    {
        QuantumEvent.Subscribe<EventOnPlayerJoined>(this, OnPlayerJoined);
        QuantumEvent.Subscribe<EventOnPlayerLeft>(this, OnPlayerLeft);
        QuantumEvent.Subscribe<EventOnPlayerChangeTeam>(this, OnPlayerChangeTeam);
        RedButton.onClick.AddListener(() => PlayerTeamUpdateRequest(Team.Left));
        SpecButton.onClick.AddListener(() => PlayerTeamUpdateRequest(Team.Spec));
        BlueButton.onClick.AddListener(() => PlayerTeamUpdateRequest(Team.Right));
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

        QuantumRunner.Default.Game.SendCommand(new CommandChangeTeam {
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
