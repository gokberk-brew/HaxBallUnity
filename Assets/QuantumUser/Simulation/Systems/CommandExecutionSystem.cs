namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class CommandExecutionSystem : SystemMainThread
    {
        public override void Update(Frame frame)
        {
            for (int i = 0; i < frame.PlayerConnectedCount; i++)
            {
                var command = frame.GetPlayerCommand(i) as CommandChangeTeam;
                command?.Execute(frame);
            }
        }
    }
}
