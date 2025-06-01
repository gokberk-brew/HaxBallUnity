using System.Collections.Generic;
using Quantum.Collections;
using UnityEngine.SocialPlatforms.Impl;

namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class GameStateSystem : SystemMainThread, ISignalOnGameStarted
    {
        public override void OnInit(Frame f)
        {
            f.SetSingleton(new GameState {
                ScoreLeft = 0,
                ScoreRight = 0,
                ScoreLimit = 3,
                IsGoalPending = false,
                RespawnCountdown = 120,
                IsGameActive = false
            });

            var playerStateList = f.AllocateList<PlayerState>();
            f.SetSingleton(new PlayerStateSingleton(){
                List = playerStateList,
            });
        }
        
        public void OnGameStarted(Frame f)
        {
            if(f.Unsafe.TryGetPointerSingleton<GameState>(out var gameState))
            {
                SpawnPlayers(f);
                gameState->IsGameActive = true;
            }
        }

        private void SpawnPlayers(Frame f)
        {
            if (f.Unsafe.TryGetPointerSingleton<PlayerStateSingleton>(out var playerStateSingleton))
            {
                var playerStateList = f.ResolveList(playerStateSingleton->List);

                foreach (var playerState in playerStateList)
                {
                     var data = f.RuntimeConfig.DefaultPlayerAvatar;
                     var entityPrototypeAsset = f.FindAsset(data);
                     var playerEntity = f.Create(entityPrototypeAsset);
                     f.Add(playerEntity, new PlayerLink { PlayerRef = playerState.Player });
                     AssignToTeam(f, playerEntity, playerState);
                }
            }
        }
        
        private void AssignToTeam(Frame frame, EntityRef playerEntity, PlayerState playerState)
        {
            frame.Add(playerEntity, new PlayerState()
            {
                Player = playerState.Player,
                Team = playerState.Team,
                SpawnPosition = new FPVector2(playerState.Team == Team.Left ? -2 : 2, 0)
            });

            var transform = frame.Unsafe.GetPointer<Transform2D>(playerEntity);
            transform -> Position = frame.Get<PlayerState>(playerEntity).SpawnPosition;
        }

        public override void Update(Frame f)
        {
            if (f.Unsafe.TryGetPointerSingleton<GameState>(out var state))
            {
                if (!state->IsGameActive)
                    return;
                
                if (state->IsGoalPending)
                {
                    state->RespawnCountdown--;
                    if (state->RespawnCountdown <= 0)
                    {
                        RespawnPlayers(f);
                        RespawnPuck(f);
                        state->IsGoalPending = false;
                        state->RespawnCountdown = 120;
                    }
                }
            }
        }
        
        private void RespawnPlayers(Frame f)
        {
            foreach (var playerLink in f.Unsafe.GetComponentBlockIterator<PlayerLink>())
            {
                f.Unsafe.TryGetPointer<Transform2D>(playerLink.Entity, out var value);
                f.Unsafe.TryGetPointer<PlayerState>(playerLink.Entity, out var playerState);
                f.Unsafe.TryGetPointer<PhysicsBody2D>(playerLink.Entity, out var body);
                value->Position = playerState->SpawnPosition;
                body->Velocity = FPVector2.Zero;
                body->AngularVelocity = 0;
            }
        }
        
        private void RespawnPuck(Frame f)
        {
            var filtered = f.Filter<Transform2D, PhysicsBody2D, PuckTag>();
            
            while (filtered.NextUnsafe(out _, out var transform2D, out var body, out _)) {
                transform2D->Position = new FPVector2(0, 0);
                body->Velocity = FPVector2.Zero;
                body->AngularVelocity = 0;
            }
        }
    }
}
