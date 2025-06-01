using UnityEngine;

namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class GoalDetectionSystem : SystemSignalsOnly, ISignalOnTriggerEnter2D
    {
        public void OnTriggerEnter2D(Frame f, TriggerInfo2D info)
        {
            if (!f.Has<GoalPostTag>(info.Entity))
                return; // not a goal trigger — skip

            if (!f.Has<PuckTag>(info.Other))
                return; // not a puck — skip

            var goal = f.Get<GoalPostTag>(info.Entity);
            var scoringTeam = goal.Side == GoalPostSide.Left ? Team.Right : Team.Left;
            ScoreGoal(f, scoringTeam);
        }
        
        private void ScoreGoal(Frame f, Team scoringTeam)
        {
            var scoreState = f.Unsafe.GetPointerSingleton<GameState>();

            if (scoringTeam == Team.Left)
            {
                scoreState->ScoreLeft++;
                if (scoreState->ScoreLeft == scoreState->ScoreLimit)
                {
                    scoreState->IsGameActive = false;
                    scoreState->WinningTeam = Team.Left;
                }
            }
            else
            {
                scoreState->ScoreRight++;
                if (scoreState->ScoreRight == scoreState->ScoreLimit)
                {
                    scoreState->IsGameActive = false;
                    scoreState->WinningTeam = Team.Right;
                }
            }
            scoreState ->IsGoalPending = true;
            
            f.Events.OnGoalScored(scoringTeam, new GameState
            {
                ScoreLeft = scoreState->ScoreLeft,
                ScoreRight = scoreState->ScoreRight,
                IsGameActive = scoreState->IsGameActive
            });
        } }
}
