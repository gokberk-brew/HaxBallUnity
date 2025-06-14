// <auto-generated>
// This code was auto-generated by a tool, every time
// the tool executes this code will be reset.
//
// If you need to extend the classes generated to add
// fields or methods to them, please create partial
// declarations in another file.
// </auto-generated>
#pragma warning disable 0109
#pragma warning disable 1591


namespace Quantum {
  using Photon.Deterministic;
  using Quantum;
  using Quantum.Core;
  using Quantum.Collections;
  using Quantum.Inspector;
  using Quantum.Physics2D;
  using Quantum.Physics3D;
  using Byte = System.Byte;
  using SByte = System.SByte;
  using Int16 = System.Int16;
  using UInt16 = System.UInt16;
  using Int32 = System.Int32;
  using UInt32 = System.UInt32;
  using Int64 = System.Int64;
  using UInt64 = System.UInt64;
  using Boolean = System.Boolean;
  using String = System.String;
  using Object = System.Object;
  using FlagsAttribute = System.FlagsAttribute;
  using SerializableAttribute = System.SerializableAttribute;
  using MethodImplAttribute = System.Runtime.CompilerServices.MethodImplAttribute;
  using MethodImplOptions = System.Runtime.CompilerServices.MethodImplOptions;
  using FieldOffsetAttribute = System.Runtime.InteropServices.FieldOffsetAttribute;
  using StructLayoutAttribute = System.Runtime.InteropServices.StructLayoutAttribute;
  using LayoutKind = System.Runtime.InteropServices.LayoutKind;
  #if QUANTUM_UNITY //;
  using TooltipAttribute = UnityEngine.TooltipAttribute;
  using HeaderAttribute = UnityEngine.HeaderAttribute;
  using SpaceAttribute = UnityEngine.SpaceAttribute;
  using RangeAttribute = UnityEngine.RangeAttribute;
  using HideInInspectorAttribute = UnityEngine.HideInInspector;
  using PreserveAttribute = UnityEngine.Scripting.PreserveAttribute;
  using FormerlySerializedAsAttribute = UnityEngine.Serialization.FormerlySerializedAsAttribute;
  using MovedFromAttribute = UnityEngine.Scripting.APIUpdating.MovedFromAttribute;
  using CreateAssetMenu = UnityEngine.CreateAssetMenuAttribute;
  using RuntimeInitializeOnLoadMethodAttribute = UnityEngine.RuntimeInitializeOnLoadMethodAttribute;
  #endif //;
  
  public unsafe partial class Frame {
    public unsafe partial struct FrameEvents {
      static partial void GetEventTypeCountCodeGen(ref Int32 eventCount) {
        eventCount = 10;
      }
      static partial void GetParentEventIDCodeGen(Int32 eventID, ref Int32 parentEventID) {
        switch (eventID) {
          default: break;
        }
      }
      static partial void GetEventTypeCodeGen(Int32 eventID, ref System.Type result) {
        switch (eventID) {
          case EventOnSystemInitialized.ID: result = typeof(EventOnSystemInitialized); return;
          case EventOnGoalScored.ID: result = typeof(EventOnGoalScored); return;
          case EventOnGameStarted.ID: result = typeof(EventOnGameStarted); return;
          case EventOnGameEnded.ID: result = typeof(EventOnGameEnded); return;
          case EventOnPlayerJoined.ID: result = typeof(EventOnPlayerJoined); return;
          case EventOnPlayerLeft.ID: result = typeof(EventOnPlayerLeft); return;
          case EventOnPlayerChangeTeam.ID: result = typeof(EventOnPlayerChangeTeam); return;
          case EventOnTimeDropdownChanged.ID: result = typeof(EventOnTimeDropdownChanged); return;
          case EventOnScoreDropdownChanged.ID: result = typeof(EventOnScoreDropdownChanged); return;
          default: break;
        }
      }
      public EventOnSystemInitialized OnSystemInitialized() {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnSystemInitialized>(EventOnSystemInitialized.ID);
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnGoalScored OnGoalScored(Team ScoredTeam, GameState GameState) {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnGoalScored>(EventOnGoalScored.ID);
        ev.ScoredTeam = ScoredTeam;
        ev.GameState = GameState;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnGameStarted OnGameStarted() {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnGameStarted>(EventOnGameStarted.ID);
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnGameEnded OnGameEnded(GameEndReason Reason) {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnGameEnded>(EventOnGameEnded.ID);
        ev.Reason = Reason;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnPlayerJoined OnPlayerJoined(PlayerRef JoinedPlayer) {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnPlayerJoined>(EventOnPlayerJoined.ID);
        ev.JoinedPlayer = JoinedPlayer;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnPlayerLeft OnPlayerLeft(PlayerRef LeftPlayer) {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnPlayerLeft>(EventOnPlayerLeft.ID);
        ev.LeftPlayer = LeftPlayer;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnPlayerChangeTeam OnPlayerChangeTeam(PlayerRef PlayerRef, Team Team) {
        var ev = _f.Context.AcquireEvent<EventOnPlayerChangeTeam>(EventOnPlayerChangeTeam.ID);
        ev.PlayerRef = PlayerRef;
        ev.Team = Team;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnTimeDropdownChanged OnTimeDropdownChanged(Byte TimeLimit) {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnTimeDropdownChanged>(EventOnTimeDropdownChanged.ID);
        ev.TimeLimit = TimeLimit;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnScoreDropdownChanged OnScoreDropdownChanged(Byte ScoreLimit) {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnScoreDropdownChanged>(EventOnScoreDropdownChanged.ID);
        ev.ScoreLimit = ScoreLimit;
        _f.AddEvent(ev);
        return ev;
      }
    }
  }
  public unsafe partial class EventOnSystemInitialized : EventBase {
    public new const Int32 ID = 1;
    protected EventOnSystemInitialized(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnSystemInitialized() : 
        base(1, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 41;
        return hash;
      }
    }
  }
  public unsafe partial class EventOnGoalScored : EventBase {
    public new const Int32 ID = 2;
    public Team ScoredTeam;
    public GameState GameState;
    protected EventOnGoalScored(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnGoalScored() : 
        base(2, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 43;
        hash = hash * 31 + ScoredTeam.GetHashCode();
        hash = hash * 31 + GameState.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnGameStarted : EventBase {
    public new const Int32 ID = 3;
    protected EventOnGameStarted(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnGameStarted() : 
        base(3, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 47;
        return hash;
      }
    }
  }
  public unsafe partial class EventOnGameEnded : EventBase {
    public new const Int32 ID = 4;
    public GameEndReason Reason;
    protected EventOnGameEnded(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnGameEnded() : 
        base(4, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 53;
        hash = hash * 31 + Reason.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnPlayerJoined : EventBase {
    public new const Int32 ID = 5;
    public PlayerRef JoinedPlayer;
    protected EventOnPlayerJoined(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnPlayerJoined() : 
        base(5, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 59;
        hash = hash * 31 + JoinedPlayer.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnPlayerLeft : EventBase {
    public new const Int32 ID = 6;
    public PlayerRef LeftPlayer;
    protected EventOnPlayerLeft(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnPlayerLeft() : 
        base(6, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 61;
        hash = hash * 31 + LeftPlayer.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnPlayerChangeTeam : EventBase {
    public new const Int32 ID = 7;
    public PlayerRef PlayerRef;
    public Team Team;
    protected EventOnPlayerChangeTeam(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnPlayerChangeTeam() : 
        base(7, EventFlags.Server|EventFlags.Client) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 67;
        hash = hash * 31 + PlayerRef.GetHashCode();
        hash = hash * 31 + Team.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnTimeDropdownChanged : EventBase {
    public new const Int32 ID = 8;
    public Byte TimeLimit;
    protected EventOnTimeDropdownChanged(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnTimeDropdownChanged() : 
        base(8, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 71;
        hash = hash * 31 + TimeLimit.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnScoreDropdownChanged : EventBase {
    public new const Int32 ID = 9;
    public Byte ScoreLimit;
    protected EventOnScoreDropdownChanged(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnScoreDropdownChanged() : 
        base(9, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 73;
        hash = hash * 31 + ScoreLimit.GetHashCode();
        return hash;
      }
    }
  }
}
#pragma warning restore 0109
#pragma warning restore 1591
