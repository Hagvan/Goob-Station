using Robust.Shared.GameStates;

namespace Content.Shared.Something;

[RegisterComponent, NetworkedComponent]
public sealed partial class SomethingComponent : Component
{
    [DataField]
    public float SpeedBonus = 0.8f;

    [DataField]
    public float MaxSpeedBonus = 1.3f;

    [DataField]
    public float MinSpeedBonus = 0.8f;

    [DataField]
    public TimeSpan StartTime = TimeSpan.Zero;
}
