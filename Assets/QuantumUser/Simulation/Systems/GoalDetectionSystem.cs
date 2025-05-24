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
            if (!f.Has<Goal>(info.Entity))
                return; // not a goal trigger — skip

            if (!f.Has<PuckTag>(info.Other))
                return; // not a puck — skip

            var goal = f.Get<Goal>(info.Entity);
            var scoringTeam = goal.Team == Team.Left ? Team.Right : Team.Left;
            ScoreGoal(f, scoringTeam);
        }
        
        private void ScoreGoal(Frame f, Team scoringTeam)
        {
            var scoreState = f.Unsafe.GetPointerSingleton<ScoreState>();

            if (scoringTeam == Team.Left)
            {
                scoreState->ScoreLeft++;
                if (scoreState->ScoreLeft == scoreState->ScoreLimit)
                {
                    scoreState->GameEnded = true;
                    scoreState->WinningTeam = Team.Left;
                }
            }
            else
            {
                scoreState->ScoreRight++;
                if (scoreState->ScoreRight == scoreState->ScoreLimit)
                {
                    scoreState->GameEnded = true;
                    scoreState->WinningTeam = Team.Right;
                }
            }
            scoreState ->IsGoalPending = true;
            
            f.Events.OnGoalScored(scoringTeam, new ScoreState()
            {
                ScoreLeft = scoreState->ScoreLeft,
                ScoreRight = scoreState->ScoreRight,
                GameEnded = scoreState->GameEnded
            });
        } }
}
