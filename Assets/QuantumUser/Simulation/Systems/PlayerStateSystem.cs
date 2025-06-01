using System.Linq;

namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class PlayerStateSystem : SystemSignalsOnly, ISignalOnPlayerAdded,ISignalOnPlayerRemoved,ISignalOnPlayerDisconnected, ISignalOnPlayerTeamUpdated
    {
        public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
        {
            var playerData = f.GetPlayerData(player);
            var nickname = playerData.PlayerNickname;

            if (f.Unsafe.TryGetPointerSingleton<PlayerStateSingleton>(out var playerList))
            {
                var playerStates = f.ResolveList(playerList->List);

                var playerState = new PlayerState
                {
                    Player = player,
                    Nickname = nickname,
                    Team = Team.Spec,
                };
                playerStates.Add(playerState);
                f.Events.OnPlayerJoined(player);
            }
        }
        
        public void OnPlayerTeamUpdated(Frame f, PlayerRef playerRef, Team team)
        {
            var playerStateSingleton = f.Unsafe.GetPointerSingleton<PlayerStateSingleton>();
            var playerStates = f.ResolveList(playerStateSingleton->List);

            Log.Info($"[Quantum] Signal received - PlayerStates Count: {playerStates.Count}");

            for (int i = 0; i < playerStates.Count; i++)
            {
                if (playerStates[i].Player == playerRef)
                {
                    var psPtr = playerStates.GetPointer(i);
                    psPtr->Team = team;
                    Log.Info($"[Quantum] Updated Player {playerRef} team to {team}");
                    f.Events.OnPlayerChangeTeam(playerRef, team);
                    break;
                }
            }
        }

        public void OnPlayerDisconnected(Frame f, PlayerRef player)
        {
            // Find all entities with PlayerLink component
            foreach (var (entityRef, playerLink) in f.Unsafe.GetComponentBlockIterator<PlayerLink>())
            {
                if (playerLink->PlayerRef == player)
                {
                    f.Destroy(entityRef);
                    Log.Info($"Player entity removed for PlayerRef: {player}");
                }
            }
        }

        public void OnPlayerRemoved(Frame f, PlayerRef player)
        {
            if (f.Unsafe.TryGetPointerSingleton<PlayerStateSingleton>(out var singleton))
            {
                var list = f.ResolveList(singleton->List);

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Player == player)
                    {
                        f.Events.OnPlayerLeft(player);
                        
                        list.RemoveAt(i);
                        Log.Info($"[Quantum] Removed PlayerRef {player} from PlayerStateSingleton.List");
                        break;
                    }
                }

                if (list.Count == 0)
                {
                    f.FreeList(singleton->List);
                    singleton->List = default;
                    Log.Info("[Quantum] All players removed — PlayerStateSingleton.List freed.");
                }
            }
        }
    }
}
