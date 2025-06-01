namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public class CommandExecutionSystem : SystemMainThread
    {
        public override void Update(Frame frame)
        {
            for (int i = 0; i < frame.PlayerConnectedCount; i++)
            {
                var command = frame.GetPlayerCommand(i);

                switch (command)
                {
                    case ChangeTeamCommand changeTeam:
                        changeTeam.Execute(frame);
                        break;
                    case StartCommand start:
                        start.Execute(frame);
                        break;
                }
            }
        }

    }
}
