// SPDX-FileCopyrightText: 2025 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 2025 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.Movement.Events;
using Content.Shared.Movement.Systems;
using Robust.Shared.Timing;

namespace Content.Shared.Something;

public sealed class SharedSomethingSystem : EntitySystem
{
    [Dependency] private readonly MovementSpeedModifierSystem _movementSpeedModifierSystem = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SomethingComponent, RefreshMovementSpeedModifiersEvent>(OnRefreshMovementSpeed);
        SubscribeLocalEvent<SomethingComponent, MoveInputEvent>(OnInputMoveEvent);
    }

    private void OnInputMoveEvent(Entity<SomethingComponent> ent, ref MoveInputEvent args)
    {
        Log.Debug("Direction: " + args.Dir);
    }

    private void OnRefreshMovementSpeed(Entity<SomethingComponent> ent, ref RefreshMovementSpeedModifiersEvent args)
    {
        args.ModifySpeed(ent.Comp.SpeedBonus);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<SomethingComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (comp.StartTime == TimeSpan.Zero)
                comp.StartTime = _timing.CurTime;
            var idk = 0.15f * (float) (_timing.CurTime - comp.StartTime).TotalSeconds;
            comp.SpeedBonus = comp.MinSpeedBonus + idk % (comp.MaxSpeedBonus - comp.MinSpeedBonus);
            Dirty(uid, comp);
            _movementSpeedModifierSystem.RefreshMovementSpeedModifiers(uid);
        }
    }
}
