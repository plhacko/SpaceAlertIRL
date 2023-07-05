using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drednot : Enemy<Drednot>
{
    protected override int StratingHPConst => 6;
    protected override int MaxEnergyShieldConst => 2;
    protected override float StartingSpeedConst => 2.0f;


    int Iterator = 0;
    protected override EnemyAction DecideNextAction()
    {
        Iterator++;
        switch (Iterator)
        {
            case 1:
                return new SimpleAttack(3, Zone);
            case 2:
                return new SimpleAttack(3, Zone);
            case 3:
                return new SimpleAttack(5, Zone);
            default:
                return new Wait("");
        }
    }
}
