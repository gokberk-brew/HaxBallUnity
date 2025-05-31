using System.Linq;

namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class PlayerSpawnSystem : SystemSignalsOnly, ISignalOnPlayerAdded,ISignalOnPlayerDisconnected
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
            
            //TODO: Create player entity
            // Direct reference to runtime config on debug on scene
            var data = f.RuntimeConfig.DefaultPlayerAvatar;
            var entityPrototypeAsset = f.FindAsset(data);
            var playerEntity = f.Create(entityPrototypeAsset);
            f.Add(playerEntity, new PlayerLink { PlayerRef = player });
            AssignToTeam(f, playerEntity, player);
        }

        private void AssignToTeam(Frame frame, EntityRef playerEntity, PlayerRef player)
        {
            frame.Add(playerEntity, new PlayerState()
            {
                Player = player,
                Team = frame.PlayerConnectedCount % 2 == 0 ? Team.Left : Team.Right,
                SpawnPosition = new FPVector2(frame.PlayerConnectedCount % 2 == 0 ? -2 : 2, 0)
            });

            var transform = frame.Unsafe.GetPointer<Transform2D>(playerEntity);
            transform -> Position = frame.Get<PlayerState>(playerEntity).SpawnPosition;
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
    }
}
