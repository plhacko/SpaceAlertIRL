#define SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if SERVER

abstract class EnemyAction
{
    public abstract override string ToString();
    public abstract void ExecuteAction();
}

sealed class SimpleAttack : EnemyAction
{
    public override string ToString() => $"Attack({Damage})";

    public override void ExecuteAction()
    {
        Zone.TakeDmage(Damage);
    }
    int Damage;
    Zone Zone;
    SimpleAttack(int damage, Zone zone)
    {
        Damage = damage;
        Zone = zone;
    }
}

#endif

