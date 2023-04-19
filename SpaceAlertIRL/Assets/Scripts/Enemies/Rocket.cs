using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

// expodes on contact with different enemy (in future enemies might shoot rockets as well)
public class Rocket : Enemy<Rocket>
{
    public const RangeEnum MaxRangeConst = RangeEnum.Mid;
    public const int DamageConst = 4;
    protected override int StratingHPConst => 1;
    protected override int MaxEnergyShieldConst => 0;
    protected override float StartingSpeedConst => 7.0f;
    protected override RangeEnum StartingDistanceConst => RangeEnum.Zero;
    protected override float EnergyShieldRegenerationTimeConst => 0.0f;

    public int Damage { get => DamageConst; }
    public float Range { get => (float)MaxRangeConst; }
    public void ChangeDirection() { _Speed.Value = -_Speed.Value; }
    protected override void DistanceChange()
    {
        float newDistance = _Distance.Value - Time.deltaTime * _Speed.Value;

        // checks for collisions
        foreach (Enemy e in Zone.GenrateSortedEnemyArray())
        {
            if (e == this || !e.IsTragetabeByRocket) { continue; }
            else if (System.Math.Abs(e.Distance - newDistance) < 0.1f)
            {
                e.TakeDamage(DamageConst);
                Impact();
                return;
            }
        }
        if (newDistance > (float)MaxRangeConst)
        { Impact(); }
        if (newDistance < 0) // the rocket has dmaged the players ship 
        { Zone.TakeDmage(DamageConst); Impact(); }

        _Distance.Value = newDistance;
    }
    public override void Die(bool silent = true)
    {
        base.Die(silent);
    }
    protected override EnemyAction DecideNextAction() => new Wait(float.MaxValue);

}
