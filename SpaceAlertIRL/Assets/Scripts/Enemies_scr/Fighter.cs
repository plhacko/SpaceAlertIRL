using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Enemy<Fighter>
{
    protected override int StratingHPConst => 4;
    protected override int MaxEnergyShieldConst => 2;
    protected override float StartingSpeedConst => 4.0f;
    protected override float EnergyShieldRegenerationTimeConst => 2.0f;


    int Iterator = 0;
    protected override EnemyAction DecideNextAction()
    {
        Iterator++;
        switch (Iterator)
        {
            case 1:
                return new LaunchRocket(GetComponentInParent<EnemySpawner>(), this);
            case 2:
                return new SimpleAttack(2, Zone);
            case 3:
                return new SimpleAttack(4, Zone);
            default:
                return new Wait("");
        }
    }
}
