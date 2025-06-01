using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using UnityEngine;
using BitStream = Photon.Deterministic.BitStream;

namespace Quantum
{
    public class StartCommand : DeterministicCommand
    {
        public override void Serialize(BitStream stream)
        {
        }

        public void Execute(Frame f) {
            f.Signals.OnGameStarted();
        }
    }
}
