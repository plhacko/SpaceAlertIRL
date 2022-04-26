using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Enemy<Fighter>
{
    protected override int StratingHPConst => 4;
    protected override int MaxEnergyShieldConst => 2;
    protected override float StartingSpeedConst => 3;
    protected override float StartingDistanceConst => 10000.0f; // 42.0f;
    protected override float EnergyShieldRegenerationTimeConst => 2.0f;

    protected override EnemyAction DecideNextAction()
    {
        return new SimpleAttack(4, Zone, 10.0f);
    }
}
