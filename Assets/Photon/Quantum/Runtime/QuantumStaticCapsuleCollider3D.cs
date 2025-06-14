namespace Quantum {
  using Photon.Deterministic;
  using UnityEngine;

  /// <summary>
  /// The script will create a static 3D capsule collider during Quantum map baking.
  /// </summary>
  public class QuantumStaticCapsuleCollider3D : QuantumMonoBehaviour {
#if QUANTUM_ENABLE_PHYSICS3D && !QUANTUM_DISABLE_PHYSICS3D
    /// <summary>
    /// Link a Unity capsule collider to copy its size and position of during Quantum map baking.
    /// </summary>
    public CapsuleCollider SourceCollider;
    /// <summary>
    /// The radius of the capsule.
    /// </summary>
    [InlineHelp, DrawIf("SourceCollider", 0)] 
    public FP Radius;
    /// <summary>
    /// The height of the capsule.
    /// </summary>
    [InlineHelp, DrawIf("SourceCollider", 0)] 
    public FP Height;
    /// <summary>
    /// The position offset added to the <see cref="Transform.position"/> during baking.
    /// </summary>
    [InlineHelp, DrawIf("SourceCollider", 0)]
    public FPVector3 PositionOffset;
    /// <summary>
    /// The rotation offset added to the <see cref="Transform.rotation"/> during baking.
    /// </summary>
    [InlineHelp] 
    public FPVector3 RotationOffset;
    /// <summary>
    /// Additional static collider settings.
    /// </summary>
    [InlineHelp, DrawInline, Space]
    public QuantumStaticColliderSettings Settings = new QuantumStaticColliderSettings();

    internal CapsuleDirection3D Direction = CapsuleDirection3D.Y;
    

    private void OnValidate() {
      Radius = FPMath.Clamp(Radius, 0, Radius);
      Height = FPMath.Clamp(Height, 0, Height);
      UpdateFromSourceCollider();
    }

    /// <summary>
    /// Copy collider configuration from source collider if exist. 
    /// </summary>
    public void UpdateFromSourceCollider() {
      if (SourceCollider == null) {
        return;
      }
      switch (SourceCollider.direction) {
        case 0:
          Direction = CapsuleDirection3D.X;
          break;
        case 1:
          Direction = CapsuleDirection3D.Y;
          break;
        case 2:
          Direction = CapsuleDirection3D.Z;
          break;
      }
      Radius = SourceCollider.radius.ToFP();
      Height = SourceCollider.height.ToFP();
      PositionOffset = SourceCollider.center.ToFPVector3();
      Settings.Trigger = SourceCollider.isTrigger;
    }

    /// <summary>
    /// Callback before baking the collider.
    /// </summary>
    public virtual void BeforeBake() {
      UpdateFromSourceCollider();
    }
#endif
  }
}