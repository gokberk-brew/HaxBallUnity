namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class BallSpawnSystem : SystemSignalsOnly
    {
        public override void OnInit(Frame f)
        {
            SpawnBall(f);
        }

        public void SpawnBall(Frame f)
        {
            HoxBallGameConfig gameConfig = f.FindAsset(f.RuntimeConfig.GameConfig);
            var ball = f.Create(gameConfig.BallPrototype);
            f.Unsafe.TryGetPointer<Transform2D>(ball, out var value);
            value->Position = new FPVector2(0, 0);
        }
    }
}
