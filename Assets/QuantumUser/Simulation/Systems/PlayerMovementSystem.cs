namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class PlayerMovementSystem : SystemMainThreadFilter<PlayerMovementSystem.Filter>
    {
        public override void Update(Frame frame, ref Filter filter)
        {
            Input* input = frame.GetPlayerInput(filter.PlayerLink->PlayerRef);
            UpdatePlayerMovement(frame, ref filter, input);
        }

        private void UpdatePlayerMovement(Frame frame, ref Filter filter, Input* input)
        {
            if (input->Direction.SqrMagnitude == 0)
                return; // no input, no movement

            // Optional: normalize direction (if using raw input)
            var moveDir = input->Direction.Normalized;

            // Define your force strength
            FP moveForce = 10; // You can expose this via a MoveSpeed component or const

            // Apply force
            filter.PhysicsBody->AddForce(moveDir * moveForce);
        }

        public struct Filter
        {
            public EntityRef Entity;
            public PhysicsBody2D* PhysicsBody;
            public PlayerLink* PlayerLink;
            public PlayerTag* PlayerTag;
        }
    }
}
