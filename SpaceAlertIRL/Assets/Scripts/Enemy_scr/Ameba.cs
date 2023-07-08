using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ameba : Enemy<Ameba>
{
    protected override int StratingHPConst => 6;
    protected override int MaxEnergyShieldConst => 3;
    protected override float StartingSpeedConst => 2.0f;

    private bool WasDamaged = false;

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (damage > 0)
        { WasDamaged = true; }

        Iterator--;
        NextEnemyAction = DecideNextAction();
        NextActionDescription = NextEnemyAction.GetDescription() ?? "no action";

    }

    int Iterator = 0;
    protected override EnemyAction DecideNextAction()
    {
        Iterator++;
        switch (Iterator)
        {
            case 1:
                return WasDamaged ? (EnemyAction)new Heal(this, 4) : (EnemyAction)new SpeedUp(this, 2.0f);
            case 2:
                return WasDamaged ? (EnemyAction)new CombinedAction(new SimpleAttack(3, Zone), (EnemyAction)new SpeedUp(this, 2.0f)) : (EnemyAction)new SpeedUp(this, 2.0f);
            case 3:
                return WasDamaged ? (EnemyAction)new CombinedAction(new SimpleAttack(4, Zone), (EnemyAction)new KillEveryoneInZone(Zone)) : (EnemyAction)new KillEveryoneInZone(Zone);
            default:
                return new Wait("");
        }
    }
}
