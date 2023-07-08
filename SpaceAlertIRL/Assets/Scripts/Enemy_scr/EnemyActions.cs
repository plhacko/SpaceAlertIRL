#define SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;
using UnityEditor;
using System.Text;


#if SERVER

public abstract class EnemyAction
{
    public abstract void ExecuteAction();
    public abstract string GetDescription();
}

sealed class CombinedAction : EnemyAction
{
    public override void ExecuteAction()
    {
        foreach (EnemyAction ea in EnemyActions)
        { ea.ExecuteAction(); }
    }

    public override string GetDescription()
    {
        StringBuilder sb = new StringBuilder();
        foreach (EnemyAction ea in EnemyActions)
        { sb.AppendLine(ea.GetDescription()); }

        return sb.ToString();
    }

    EnemyAction[] EnemyActions;
    public CombinedAction(params EnemyAction[] enemyActions)
    {
        EnemyActions = enemyActions;
    }
}

sealed class SimpleAttack : EnemyAction
{
    public override void ExecuteAction() { Zone.TakeDmage(Damage); }

    readonly int Damage;
    readonly Zone Zone;
    public SimpleAttack(int damage, Zone zone)
    { Damage = damage; Zone = zone; }

    public override string GetDescription() => $"Deals {Damage} damage";
}

sealed class Wait : EnemyAction
{
    public override void ExecuteAction() { }

    public override string GetDescription() => CustomMessage;
    string CustomMessage;
    public Wait(string customMessage = "Waiting") { CustomMessage = customMessage; }
}

sealed class LaunchRocket : EnemyAction
{
    public override string GetDescription() => $"Shoots a rocket";
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
    public override string GetDescription() => "Teleports all players to random rooms";
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
    public TeleportAllPlayersToRandomDestination() { }
}

sealed class TeleportAllPlayersToThisZone : EnemyAction
{
    public override string GetDescription() => "Teleports all players to this zone";
    public override void ExecuteAction()
    {
        Room[] rooms = Zone.GetComponentsInChildren<Room>();
        foreach (Player player in Player.GetAllPlayers())
        {
            int rndId = Random.Range(0, rooms.Length);
            player.RequestChangingRoom(roomName: rooms[rndId].name, ignoreRestrictions: true);

            AudioManager.Instance.RequestPlayingSentenceOnClient("youHaveBeenTeleported_r");
        }
    }
    Zone Zone;
    public TeleportAllPlayersToThisZone(Zone zone)
    { Zone = zone; }
}

sealed class CloseAllDoors : EnemyAction
{
    public override string GetDescription() => "Closes all doors";
    public override void ExecuteAction()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

        foreach (var d in doors)
        {
            d.GetComponent<Door>().RequestClosingFully();
        }
    }
    public CloseAllDoors() { }
}

sealed class CloseAllDoorsInZone : EnemyAction
{
    public override string GetDescription() => "Closes all doors in current zone";
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
    public override string GetDescription() => $"Locks all doors in zone for {WaitTime.ToString("0.0")}s";
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
    public override string GetDescription() => $"Locks {NumberOfDoorsToLock} random doors for {WaitTime.ToString("0.0")}s";
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
    public override string GetDescription() => $"Depletes {EnergyToDeplete} from energy shields in each zone";
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
    public override string GetDescription() => $"Depletes all energy on the ship";
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

    public DepleteAllEnergy() { }
}

sealed class DepleteEnergyInZone : EnemyAction
{
    public override void ExecuteAction()
    {
        EnergyPool[] energyPools = Zone.transform.GetComponentsInChildren<EnergyPool>();

        int energyToDeplete = EnergyToDeplete;
        foreach (var e in energyPools)
        {
            energyToDeplete -= e.PullEnergyUpTo(energyToDeplete);
        }
    }
    public override string GetDescription() => $"Depletes {EnergyToDeplete} energy in {Zone.name}";

    readonly int EnergyToDeplete;
    readonly Zone Zone;
    public DepleteEnergyInZone(int eneryToDeplete, Zone zone)
    {

        EnergyToDeplete = eneryToDeplete;
        Zone = zone;
    }
}

sealed class SpeedUp : EnemyAction
{
    public override void ExecuteAction() => Enemy.Speed += SpeedUpValue;
    public override string GetDescription() => $"Speeds up from {Enemy.Speed} to {Enemy.Speed + SpeedUpValue}";

    readonly float SpeedUpValue;
    readonly Enemy Enemy;
    public SpeedUp(Enemy enemy, float speedUpValue)
    {
        SpeedUpValue = speedUpValue;
        Enemy = enemy;
    }
}

sealed class Heal : EnemyAction
{
    public override void ExecuteAction() => Enemy.HP = System.Math.Min(Enemy.MaxHP, Enemy.HP + HealValue);
    public override string GetDescription() => $"Heals for {HealValue} hp";

    readonly int HealValue;
    readonly Enemy Enemy;
    public Heal(Enemy enemy, int heal)
    {
        HealValue = heal;
        Enemy = enemy;
    }
}

sealed class KillEveryoneInZone : EnemyAction
{
    public override string GetDescription() => "Kills everyone in zone";
    public override void ExecuteAction()
    {
        foreach (var p in Player.GetAllPlayersInZone(Zone))
        {
            p.Kill();
        }
    }
    readonly Zone Zone;
    public KillEveryoneInZone(Zone zone)
    { Zone = zone; }
} 

#endif

