using UnityEngine.Serialization;

namespace Quantum
{
    using UnityEngine;

    public class PlayerEntityViewComponent : QuantumEntityViewComponent
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private SpriteRenderer _playerIndicator;
        [SerializeField] private SpriteRenderer _shootIndicator;
        private bool _initialized;
        [SerializeField] private Color _targetColor;
        public override void OnUpdateView()
        {
            if (!_initialized || _playerIndicator == null || QuantumRunner.Default == null)
                return;

            var game = QuantumRunner.Default.Game;
            var frame = game.Frames.Verified;

            var playerRef = frame.Get<PlayerLink>(EntityRef).PlayerRef;
            if (game.PlayerIsLocal(playerRef) && !CameraManager.VCam.Follow) 
            {
                CameraManager.VCam.Follow = transform;
            }

            if (!frame.Has<PlayerState>(EntityRef))
                return;
            
            _targetColor = frame.Get<PlayerState>(EntityRef).ShootIndicator ? Color.white : Color.black;

            _shootIndicator.color = _targetColor;
        }

        public override void OnLateUpdateView()
        {
            if (_initialized || _renderer == null || QuantumRunner.Default == null)
                return;

            var game = QuantumRunner.Default.Game;
            var frame = game.Frames.Verified;

            if (frame.Has<PlayerState>(EntityRef))
            {
                var state = frame.Get<PlayerState>(EntityRef);
                HoxBallGameConfig gameConfig = frame.FindAsset(frame.RuntimeConfig.GameConfig);
                
                if (gameConfig == null)
                {
                    Debug.LogError("Missing HoxBallGameConfig.");
                    return;
                }

                var material = state.Team == Team.Left
                    ? gameConfig.RedPlayerMaterial
                    : gameConfig.BluePlayerMaterial;

                
                _playerIndicator.gameObject.SetActive(game.PlayerIsLocal(state.Player));
                
                if (material != null)
                {
                    _renderer.material = material;
                    _initialized = true;
                }
            }
        }
        
        public override void OnDeactivate()
        {
            if (_playerIndicator != null)
            {
                Destroy(_playerIndicator);
            }
        }
    }
}
