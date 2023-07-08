using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ameba : Enemy<Ameba>
{
    protected override int StratingHPConst => 8;
    protected override int MaxEnergyShieldConst => 1;
    protected override float StartingSpeedConst => 2.0f;

    private bool WasDamaged => HP < MaxHP;

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (WasDamaged)
        {
            Iterator--;
            NextEnemyAction = DecideNextAction();
            NextActionDescription = NextEnemyAction.GetDescription() ?? "no action";
        }
    }

    int Iterator = 0;
    protected override EnemyAction DecideNextAction()
    {
        Iterator++;
        switch (Iterator)
        {
            case 1:
                if (WasDamaged)
                    return new CombinedAction(
                        new SpeedUp(this, 2.0f),
                        new Heal(this, 4));
                else
                    return new SpeedUp(this, 2.0f);
            case 2:
                if (WasDamaged)
                    return new CombinedAction(
                        new SpeedUp(this, 2.0f),
                        new SimpleAttack(3, Zone));
                else
                    return new SpeedUp(this, 2.0f);
            case 3:
                if (WasDamaged)
                    return new CombinedAction(
                        new KillEveryoneInZone(Zone),
                        new SimpleAttack(4, Zone));
                else
                    return new KillEveryoneInZone(Zone);
            default:
                return new Wait("");
        }
    }
}
