using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Enemy<Fighter>
{
    protected override int StratingHPConst => 4;
    protected override int MaxEnergyShieldConst => 2;
    protected override float StartingSpeedConst => 4.0f;
    protected override float EnergyShieldRegenerationTimeConst => 2.0f;


    const RangeEnum RocketRange = RangeEnum.Mid;
    const float RocketActionTime = 3.0f;
    const float SimpleAttackActionTime = 15.0f;
    const int SimpleAttackDamage = 3;

    int RocketCount = 1;

    protected override EnemyAction DecideNextAction()
    {
        if (RocketCount > 0 && (float)RocketRange + RocketActionTime > this.Distance)
        {
            RocketCount--;
            return new LaunchRocket(GetComponentInParent<EnemySpawner>(), this, RocketActionTime);
        }
        else
        { return new SimpleAttack(SimpleAttackDamage, Zone, SimpleAttackActionTime); }
    }
}
