using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// expodes on contact with different enemy (in future enemies might shoot rockets as well)
public class Rocket : Enemy<Rocket>
{
    const float MaxRangeConst = 50.0f;
    const int DamageConst = 4;
    protected override int StratingHPConst => 1;
    protected override int MaxEnergyShieldConst => 0;
    protected override float StartingSpeedConst => 1;
    protected override float StartingDistanceConst => 1;
    protected override float EnergyShieldRegenerationTimeConst => 0;

    public int Damage { get => DamageConst; }
    public float Range { get => MaxRangeConst; }

    protected override void DistanceChange()
    {
        float newDistance = _Distance.Value + Time.deltaTime * _Speed.Value;

        foreach (Enemy e in Zone.GetEnemyList())
        {
            if (e == this || !e.IsTragetabeByRocket()) { continue; }
            if (System.Math.Abs(e.Distance - newDistance) < 0.1f)
            {
                e.TakeDamage(DamageConst);
                Impact();
                return;
            }
        }
        if (newDistance > MaxRangeConst)
        { Impact(); }

        _Distance.Value = newDistance;
    }

    protected override EnemyAction DecideNextAction() => new Wait(float.MaxValue);
}
