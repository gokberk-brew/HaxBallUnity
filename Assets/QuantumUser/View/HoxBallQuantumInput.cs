namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine;

    public class HoxBallQuantumInput : MonoBehaviour
    {
        private void OnEnable()
        {
            QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
        }

        public void PollInput(CallbackPollInput callback)
        {
            Quantum.Input input = new Quantum.Input();
            var direction = new Vector2();
            
            direction.x = UnityEngine.Input.GetAxisRaw("Horizontal");
            direction.y = UnityEngine.Input.GetAxisRaw("Vertical");
            input.Shoot = UnityEngine.Input.GetKey(KeyCode.Space);
            
            // convert to fixed point
            input.Direction = direction.ToFPVector2();
            callback.SetInput(input, DeterministicInputFlags.Repeatable);
        }
    }
}
