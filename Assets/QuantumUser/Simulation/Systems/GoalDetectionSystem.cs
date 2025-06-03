using UnityEngine;

namespace Quantum {
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class GoalDetectionSystem : SystemSignalsOnly, ISignalOnTriggerEnter2D {
        public void OnTriggerEnter2D(Frame f, TriggerInfo2D info) {
            if (!f.Has<GoalPostTag>(info.Entity) || !f.Has<PuckTag>(info.Other))
                return;

            var goal = f.Get<GoalPostTag>(info.Entity);
            var scoringTeam = goal.Side == GoalPostSide.Left ? Team.Right : Team.Left;

            HandleGoal(f, scoringTeam);
        }

        private void HandleGoal(Frame f, Team scoringTeam) {
            
            var state = f.Unsafe.GetPointerSingleton<GameState>();

            // Increment score
            switch (scoringTeam) {
                case Team.Left:
                    state->ScoreLeft++;
                    if (state->ScoreLeft >= state->ScoreLimit)
                        EndGame(f, state, Team.Left);
                    break;

                case Team.Right:
                    state->ScoreRight++;
                    if (state->ScoreRight >= state->ScoreLimit)
                        EndGame(f, state, Team.Right);
                    break;
            }

            state->IsGoalPending = true;

            f.Events.OnGoalScored(scoringTeam, new GameState {
                ScoreLeft    = state->ScoreLeft,
                ScoreRight   = state->ScoreRight,
                IsGameActive = state->IsGameActive
            });
        }

        private void EndGame(Frame f, GameState* state, Team winner) {
            state->IsGameActive = false;
            state->WinningTeam  = winner;

            f.Events.OnGameEnded(GameEndReason.Score);
        }
    }
}
