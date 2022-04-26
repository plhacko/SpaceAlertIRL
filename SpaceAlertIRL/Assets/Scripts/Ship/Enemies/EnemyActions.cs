#define SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

#if SERVER

public abstract class EnemyAction
{
    public float TimeSpan { get; protected set; }
    public abstract void ExecuteAction();
}

sealed class SimpleAttack : EnemyAction
{
    public override void ExecuteAction() { Zone.TakeDmage(Damage); }
    int Damage;
    Zone Zone;
    public SimpleAttack(int damage, Zone zone, float timeSpan)
    { Damage = damage; Zone = zone; TimeSpan = timeSpan; }
}

sealed class Wait : EnemyAction
{
    public override void ExecuteAction() { }
    public Wait(float waitTime) { TimeSpan = waitTime; }
}
#endif

