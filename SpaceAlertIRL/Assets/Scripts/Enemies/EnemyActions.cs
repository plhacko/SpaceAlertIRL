#define SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;
using UnityEditor;


#if SERVER

public abstract class EnemyAction
{
    public abstract void ExecuteAction();
    public abstract string GetDescription();
}

sealed class SimpleAttack : EnemyAction
{
    public override void ExecuteAction() { Zone.TakeDmage(Damage); }

    readonly int Damage;
    readonly Zone Zone;
    public SimpleAttack(int damage, Zone zone)
    { Damage = damage; Zone = zone; }

    public override string GetDescription() => $"deals {Damage} damage";
}

sealed class Wait : EnemyAction
{
    public override void ExecuteAction() { }

    public override string GetDescription() => CustomMessage;
    string CustomMessage;
    public Wait(string customMessage = "waiting") { CustomMessage = customMessage; }
}

sealed class LaunchRocket : EnemyAction
{
    public override string GetDescription() => $"shoots a rocket";
    public override void ExecuteAction()
    {
        Enemy rocket = EnemySpawner.SpawnEnemy("Rocket");
        rocket.Distance = LaunchFrom.Distance - 0.2f;
    }

    readonly EnemySpawner EnemySpawner;
    readonly Enemy LaunchFrom;
    public LaunchRocket(EnemySpawner enemySpawner, Enemy launchFrom)
    { EnemySpawner = enemySpawner; LaunchFrom = launchFrom; }
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

            AudioManager.Instance.RequestPlayingSentenceOnClient("youHaveBeenTeleported_r");
        }
    }
    public TeleportAllPlayersToRandomDestination()
    { }
}

sealed class TeleportAllPlayersToThisZone : EnemyAction
{
    public override string GetDescription() => "teleports all players to this zone";
    public override void ExecuteAction()
    {
        foreach (Player player in Player.GetAllPlayers())
        {
            int rndId = Random.Range(0, Rooms.Length);
            player.RequestChangingRoom(roomName: Rooms[rndId].name, ignoreRestrictions: true);

            AudioManager.Instance.RequestPlayingSentenceOnClient("youHaveBeenTeleported_r");
        }
    }
    readonly Room[] Rooms;
    public TeleportAllPlayersToThisZone(Zone zone)
    { Rooms = zone.GetComponentsInChildren<Room>(); }
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
    public CloseAllDoors()
    { }
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
    public CloseAllDoorsInZone(Zone zone)
    { Rooms = zone.GetComponentsInChildren<Room>(); }
}

sealed class LockAllDoorsInZoneForTime : EnemyAction
{
    public override string GetDescription() => $"locks all doors in zone for {WaitTime.ToString("0.0")}s";
    public override void ExecuteAction()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

        foreach (var go in doors)
        {
            Door d = go.GetComponent<Door>();
            if (Rooms.Contains(d.RoomA) || Rooms.Contains(d.RoomB))
            {
                d.RequestLocking();
                d.StartCoroutine(UnlockDoorAfterWhile(d, WaitTime));
            }
        }
    }
    IEnumerator UnlockDoorAfterWhile(Door d, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        d.RequestUnlocking();
    }

    readonly Room[] Rooms;
    readonly long WaitTime;
    public LockAllDoorsInZoneForTime(long waitTime, Zone zone)
    { Rooms = zone.GetComponentsInChildren<Room>(); WaitTime = waitTime; }
}

sealed class LockRandomNDoorsForTime : EnemyAction
{
    public override string GetDescription() => $"locks _{NumberOfDoorsToLock}_ random doors for {WaitTime.ToString("0.0")}s";
    public override void ExecuteAction()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

        for (int i = 0; i < NumberOfDoorsToLock; i++)
        {
            Door d = doors[Random.Range(0, doors.Length)].GetComponent<Door>();
            LockAndUnlock(d);
        }


        void LockAndUnlock(Door d)
        {
            d.RequestLocking();
            d.StartCoroutine(UnlockDoorAfterWhile(d, WaitTime));
        }
    }
    IEnumerator UnlockDoorAfterWhile(Door d, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        d.RequestUnlocking();
    }

    readonly float WaitTime;
    readonly int NumberOfDoorsToLock;
    public LockRandomNDoorsForTime(float waitTime, int numberOfDoorsToLock)
    { WaitTime = waitTime; NumberOfDoorsToLock = numberOfDoorsToLock; }
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
    public DepleteShields(int eneryToDeplete)
    { EnergyToDeplete = eneryToDeplete; }
}

sealed class DepleteAllEnergy : EnemyAction
{
    public override string GetDescription() => $"depletes all energy on the ship";
    public override void ExecuteAction()
    {
        GameObject[] Zones = GameObject.FindGameObjectsWithTag("Zone");

        foreach (var z in Zones)
        {
            z.GetComponent<Zone>().DepleteEnergyShiealds(int.MaxValue);
            EnergyPool[] energyPools = z.GetComponentsInChildren<EnergyPool>();
            foreach (var ep in energyPools)
            { ep.PullEnergyUpTo(int.MaxValue); }
        }
    }

    public DepleteAllEnergy()
    {
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
    public DepleteEnergy(int eneryToDeplete, Zone zone)
    {

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

