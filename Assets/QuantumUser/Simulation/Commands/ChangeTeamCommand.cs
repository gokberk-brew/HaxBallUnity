using Photon.Deterministic;

namespace Quantum {
    public class ChangeTeamCommand : DeterministicCommand {
        public PlayerRef Player;
        public Team Team;

        public override void Serialize(BitStream stream) {
            stream.Serialize(ref Player);

            byte teamByte = (byte)Team;
            stream.Serialize(ref teamByte);

            if (!stream.Writing)
                Team = (Team)teamByte;
        }

        public void Execute(Frame f) {
            f.Signals.OnPlayerTeamUpdated(Player, Team);
        }
    }
}