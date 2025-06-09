using System.Collections.Generic;
using Quantum.Collections;
using UnityEngine.SocialPlatforms.Impl;

namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class GameStateSystem : SystemMainThread, ISignalOnGameStarted, ISignalOnGameEnded
    {
        public override void OnInit(Frame f)
        {
            f.SetSingleton(new GameState {
                ScoreLeft = 0,
                ScoreRight = 0,
                ScoreLimit = 3,
                TimeLimit = 0,
                IsGoalPending = false,
                RespawnCountdown = 120,
                IsGameActive = false,
                IsSystemInitialized = true
            });

            var playerStateList = f.AllocateList<PlayerState>();
            f.SetSingleton(new PlayerStateSingleton(){
                List = playerStateList,
            });

            f.Events.OnSystemInitialized();
        }
        
        public void OnGameStarted(Frame f)
        {
            if(f.Unsafe.TryGetPointerSingleton<GameState>(out var gameState))
            {
                SpawnPlayers(f);
                gameState->IsGameActive = true;
                int simulationRate = (int)(1 / f.DeltaTime.AsFloat);
                gameState->RemainingTimeTicks = gameState->TimeLimit * 60 * simulationRate;
                f.Events.OnGameStarted();
            }
        }

        private void SpawnPlayers(Frame f)
        {
            if (f.Unsafe.TryGetPointerSingleton<PlayerStateSingleton>(out var playerStateSingleton))
            {
                var playerStateList = f.ResolveList(playerStateSingleton->List);

                foreach (var playerState in playerStateList)
                {
                    if(playerState.Team == Team.Spec)
                        continue;
                    
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
            if (f.Unsafe.TryGetPointerSingleton<GameState>(out var gameState))
            {
                if (!gameState->IsGameActive)
                    return;

                if (gameState->IsEnding && !gameState->IsGoalPending)
                {
                    f.Signals.OnGameEnded();
                    EndGame(f);
                    return;
                }
                
                if (gameState->TimeLimit > 0 && !gameState->IsGoalPending)
                {
                    gameState->RemainingTimeTicks--;

                    if (gameState->RemainingTimeTicks <= 0)
                    {
                        gameState->RemainingTimeTicks = 0;
                        gameState->IsGoalPending = true;
                        return;
                    }
                }
                else
                {
                    gameState->RemainingTimeTicks++;
                }
                
                if (gameState->IsGoalPending)
                {
                    gameState->RespawnCountdown--;
                    if (gameState->RespawnCountdown <= 0)
                    {
                        RespawnPlayers(f);
                        RespawnPuck(f);
                        gameState->IsGoalPending = false;
                        gameState->RespawnCountdown = 120;
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

        private void EndGame(Frame f)
        {
            if(!f.Unsafe.TryGetPointerSingleton<GameState>(out var gameState))
                return;
            
            if(!gameState->IsGameActive)
                return;
            
            foreach (var (entityRef, _) in f.Unsafe.GetComponentBlockIterator<PlayerLink>())
            {
                f.Destroy(entityRef);
            }

            foreach (var (entityRef, _) in f.Unsafe.GetComponentBlockIterator<PuckTag>())
            {
                f.Destroy(entityRef);
            }
            
            gameState->IsGameActive = false;
            gameState->IsEnding = false;
            gameState->ScoreLeft = 0;
            gameState->ScoreRight = 0;
            
            f.Events.OnGameEnded(GameEndReason.Score);
        }

        public void OnGameEnded(Frame f)
        {
            if (f.Unsafe.TryGetPointerSingleton<GameState>(out var gameState))
            {
                gameState->IsEnding = true;
            }
        }
    }
}
