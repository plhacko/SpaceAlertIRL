using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCloud : Enemy<EnergyCloud>
{
    protected override int StratingHPConst => 1;
    protected override int MaxEnergyShieldConst => 9;
    protected override float StartingSpeedConst => 4.0f;
    protected override float EnergyShieldRegenerationTimeConst => 2.0f;
    
    int Iterator = 0;
    protected override EnemyAction DecideNextAction()
    {
        Iterator++;

        switch (Iterator)
        {
            case 1:
                return new DepleteEnergyInZone(5, Zone);
            case 2:
                return new DepleteShields(2);
            case 3:
                return new DepleteAllEnergy();
            default:
                return new Wait("");
        }
    }
}
