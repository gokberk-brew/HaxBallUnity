using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using UnityEngine;
using BitStream = Photon.Deterministic.BitStream;

namespace Quantum
{
    public unsafe class ScoreDropdownCommand : DeterministicCommand
    {
        public byte ScoreLimit; 
        
        public override void Serialize(BitStream stream)
        {   
            stream.Serialize(ref ScoreLimit);
        }
        public void Execute(Frame f)
        {
            if (f.Unsafe.TryGetPointerSingleton<GameState>(out var gameState))
            {
                gameState->ScoreLimit = ScoreLimit;
                f.Events.OnScoreDropdownChanged(ScoreLimit);
            }
        }
    }
}
