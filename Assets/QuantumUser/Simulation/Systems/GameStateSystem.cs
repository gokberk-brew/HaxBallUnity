using System.Collections.Generic;
using Quantum.Collections;
using UnityEngine.SocialPlatforms.Impl;

namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class GameStateSystem : SystemMainThread
    {
        public override void OnInit(Frame f)
        {
            f.SetSingleton(new ScoreState {
                ScoreLeft = 0,
                ScoreRight = 0,
                ScoreLimit = 3,
                IsGoalPending = false,
                RespawnCountdown = 120,
                GameEnded = false
            });

            var playerStateList = f.AllocateList<PlayerState>();
            f.SetSingleton(new PlayerList{
                PlayerStates = playerStateList,
            });
        }

        public override void Update(Frame f)
        {
            if (f.Unsafe.TryGetPointerSingleton<ScoreState>(out var state))
            {
                if (state->GameEnded)
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
