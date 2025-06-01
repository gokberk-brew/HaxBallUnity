using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using UnityEngine;
using BitStream = Photon.Deterministic.BitStream;

namespace Quantum
{
    public unsafe class TimeDropdownCommand : DeterministicCommand
    {
        public byte TimeLimit;
        
        public override void Serialize(BitStream stream)
        {
            stream.Serialize(ref TimeLimit);
        }

        public void Execute(Frame f)
        {
            if (f.Unsafe.TryGetPointerSingleton<GameState>(out var gameState))
            {
                gameState->TimeLimit = TimeLimit;
                f.Events.OnTimeDropdownChanged(TimeLimit);
            }
        }
    }
}
