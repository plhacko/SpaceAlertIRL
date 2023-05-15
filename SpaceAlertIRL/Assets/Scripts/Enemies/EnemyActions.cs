#define SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;


#if SERVER

public abstract class EnemyAction
{
    public float TimeSpan { get; protected set; }

    public abstract void ExecuteAction();
    public abstract string GetDescription();
}

sealed class SimpleAttack : EnemyAction
{
    public override void ExecuteAction() { Zone.TakeDmage(Damage); }

    readonly int Damage;
    readonly Zone Zone;
    public SimpleAttack(int damage, Zone zone, float timeSpan)
    { Damage = damage; Zone = zone; TimeSpan = timeSpan; }

    public override string GetDescription() => $"deals {Damage} damage";
}

sealed class Wait : EnemyAction
{
    public override void ExecuteAction() { }

    public override string GetDescription() => $"waiting";

    public Wait(float waitTime) { TimeSpan = waitTime; }
}

sealed class LaunchRocket : EnemyAction
{
    public override string GetDescription() => $"shoots a rocket";
    public override void ExecuteAction()
    {
        Enemy rocket = EnemySpawner.SpawnEnemy("Rocket");
        rocket?.SetDistance(LaunchFrom.Distance - 0.2f);
    }

    readonly EnemySpawner EnemySpawner;
    readonly Enemy LaunchFrom;
    public LaunchRocket(EnemySpawner enemySpawner, Enemy launchFrom, float timeSpan)
    { EnemySpawner = enemySpawner; LaunchFrom = launchFrom; TimeSpan = timeSpan; }
}

sealed class TeleportAllPlayersToRandomDestination : EnemyAction
{
    public override string GetDescription() => "teleports all players to random rooms";
    public override void ExecuteAction()
    {
        GameObject[] Rooms = GameObject.FindGameObjectsWithTag("Room");
        foreach (Player player in Player.GetAllPlayers())
        {
            int rndId = Random.Range(0, Rooms.Length);
            player.RequestChangingRoom(roomName: Rooms[rndId].name, ignoreRestrictions: true);

            AudioManager.GetAudioManager().RequestPlayingSentenceOnClient("youHaveBeenTeleported_r");
        }
    }
    public TeleportAllPlayersToRandomDestination(float timeSpan)
    { TimeSpan = timeSpan; }
}

sealed class TeleportAllPlayersToTisZone : EnemyAction
{
    public override string GetDescription() => "teleports all players to this zone";
    public override void ExecuteAction()
    {
        foreach (Player player in Player.GetAllPlayers())
        {
            int rndId = Random.Range(0, Rooms.Length);
            player.RequestChangingRoom(roomName: Rooms[rndId].name, ignoreRestrictions: true);

            AudioManager.GetAudioManager().RequestPlayingSentenceOnClient("youHaveBeenTeleported_r");
        }
    }
    readonly Room[] Rooms;
    public TeleportAllPlayersToTisZone(float timeSpan, Zone zone)
    { TimeSpan = timeSpan; Rooms = zone.GetComponentsInChildren<Room>(); }
}

sealed class CloseAllDoors : EnemyAction
{
    public override string GetDescription() => "closes all doors";
    public override void ExecuteAction()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

        foreach (var d in doors)
        {
            d.GetComponent<Door>().RequestClosingFully();
        }
    }
    public CloseAllDoors(float timeSpan)
    { TimeSpan = timeSpan; }
}

sealed class CloseAllDoorsInZone : EnemyAction
{
    public override string GetDescription() => "closes all doors in current zone";
    public override void ExecuteAction()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

        foreach (var go in doors)
        {
            Door d = go.GetComponent<Door>();
            if (Rooms.Contains(d.RoomA) || Rooms.Contains(d.RoomB))
                d.RequestClosingFully();
        }
    }
    readonly Room[] Rooms;
    public CloseAllDoorsInZone(float timeSpan, Zone zone)
    { TimeSpan = timeSpan; Rooms = zone.GetComponentsInChildren<Room>(); }
}


sealed class DepleteShields : EnemyAction
{
    public override string GetDescription() => $"depletes _{EnergyToDeplete}_ from energy shields in each zone";
    public override void ExecuteAction()
    {
        GameObject[] Zones = GameObject.FindGameObjectsWithTag("Zone");

        foreach (var z in Zones)
        {
            z.GetComponent<Zone>().DepleteEnergyShiealds(EnergyToDeplete);
        }
    }

    readonly int EnergyToDeplete;
    public DepleteShields(int eneryToDeplete, float timeSpan)
    {
        TimeSpan = timeSpan;
        EnergyToDeplete = eneryToDeplete;
    }
}

sealed class DepleteEnergy : EnemyAction
{
    public override void ExecuteAction()
    {
        EnergyPool[] energyPools = Zone.transform.parent.GetComponentsInChildren<EnergyPool>();

        int _energyToDeplete = EnergyToDeplete;
        foreach (var e in energyPools)
        {
            _energyToDeplete -= e.PullEnergyUpTo(_energyToDeplete);
        }
    }
    public override string GetDescription() => $"depletes _{EnergyToDeplete}_ energy in each zone";

    readonly int EnergyToDeplete;
    readonly Zone Zone;
    public DepleteEnergy(int eneryToDeplete, float timeSpan, Zone zone)
    {
        TimeSpan = timeSpan;
        EnergyToDeplete = eneryToDeplete;
        Zone = zone;
    }
}

sealed class SpeedUp : EnemyAction
{
    public override void ExecuteAction() => Enemy.Speed += SpeedUpValue;
    public override string GetDescription() => $"Speeds up from _{Enemy.Speed}_ to _{Enemy.Speed + SpeedUpValue}_";

    readonly float SpeedUpValue;
    readonly Enemy Enemy;
    public SpeedUp(Enemy enemy, float speedUpValue)
    {
        SpeedUpValue = speedUpValue;
        Enemy = enemy;
    }
}

#endif

