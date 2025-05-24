namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class PlayerKickSystem : SystemMainThreadFilter<PlayerKickSystem.Filter>
    {
        public override void Update(Frame frame, ref Filter filter)
        {
            var input = frame.GetPlayerInput(filter.PlayerLink -> PlayerRef);

            if (!input->Shoot)
            {
                filter.PlayerState->ShootIndicator = false;
                return;
            }

            filter.PlayerState->ShootIndicator = true;
            
            var playerPos = filter.Transform->Position.XY;
            FP kickForce = 100;
            FP kickRadius = 1; // 🎯 must be close to puck

            // Loop through all puck entities
            var puckQuery = frame.GetComponentIterator<PuckTag>();
            foreach (var puckEntity in puckQuery)
            {
                var puckTransform = frame.Unsafe.GetPointer<Transform2D>(puckEntity.Entity);
                var puckBody = frame.Unsafe.GetPointer<PhysicsBody2D>(puckEntity.Entity);

                var toPuck = puckTransform->Position.XY - playerPos;
                var distance = toPuck.Magnitude;

                if (distance <= kickRadius)
                {
                    // 🎯 Direction: either toward puck or based on input
                    var kickDirection = toPuck.Normalized; // Or input->Direction.Normalized

                    // Apply force
                    puckBody->AddForce(kickDirection * kickForce);

                    // Optional: only kick one puck
                    break;
                }
            }
            
        }

        public struct Filter
        {
            public EntityRef Entity;
            public Transform2D* Transform;
            public PhysicsBody2D* PhysicsBody;
            public PlayerLink* PlayerLink;
            public PlayerTag* PlayerTag;
            public PlayerState* PlayerState;
        }
    }
}

