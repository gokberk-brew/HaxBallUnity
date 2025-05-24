using UnityEngine;

namespace Quantum
{
    using Photon.Deterministic;

    public class HoxBallGameConfig : AssetObject
    {
        public AssetRef<EntityPrototype> BallPrototype;
        public Material RedPlayerMaterial;
        public Material BluePlayerMaterial;
    }
}
