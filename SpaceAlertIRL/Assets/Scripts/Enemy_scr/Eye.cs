using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Eye : Enemy<Eye>
{
    protected override int StratingHPConst => 12;
    protected override int MaxEnergyShieldConst => 0;
    protected override float StartingSpeedConst => 4.0f;

    int Iterator = 0;
    protected override EnemyAction DecideNextAction()
    {
        Iterator++;
        switch (Iterator)
        {
            case 1:
                return new CloseAllDoorsInZone(Zone);
            case 2:
                return new TeleportAllPlayersToRandomDestination();
            case 3:
                return new CombinedAction(
                    new LockAllDoorsInZoneForTime(10, Zone), // 10sec
                    new TeleportAllPlayersToThisZone(Zone));
            default:
                return new Wait();
        }
    }
}
