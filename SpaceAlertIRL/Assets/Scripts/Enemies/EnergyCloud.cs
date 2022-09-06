using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCloud : Enemy<EnergyCloud>
{
    protected override int StratingHPConst => 1;
    protected override int MaxEnergyShieldConst => 9;
    protected override float StartingSpeedConst => 1.0f;
    protected override float StartingDistanceConst => 100.0f;
    protected override float EnergyShieldRegenerationTimeConst => 2.0f;
    bool parity = false;
    protected override EnemyAction DecideNextAction()
    {
        if (parity)
        { return new DepleteEnergy(2,20.0f); }
        else
        { return new DepleteShields(2,20.0f); }
    }
}
