using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Eye : Enemy<Eye>
{
    protected override int StratingHPConst => 12;
    protected override int MaxEnergyShieldConst => 0;
    protected override float StartingSpeedConst => 0.1f;
    protected override float StartingDistanceConst => 42;
    protected override float EnergyShieldRegenerationTimeConst => float.PositiveInfinity;

    bool parity = true;
    protected override EnemyAction DecideNextAction()
    {
        parity = !parity;

        if (parity) { return new TeleportAllPlayers(20.0f); }
        else { return new SimpleAttack(3, Zone, 5.0f); }
    }
}
