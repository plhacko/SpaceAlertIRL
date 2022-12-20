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
    public abstract string GetDescription();
}

sealed class SimpleAttack : EnemyAction
{
    public override void ExecuteAction() { Zone.TakeDmage(Damage); }

    int Damage;
    Zone Zone;
    public SimpleAttack(int damage, Zone zone, float timeSpan)
    { Damage = damage; Zone = zone; TimeSpan = timeSpan; }

    public override string GetDescription() => $"Simple attack ({Damage})";
}

sealed class Wait : EnemyAction
{
    public override void ExecuteAction() { }

    public override string GetDescription() => $"Wait";

    public Wait(float waitTime) { TimeSpan = waitTime; }
}

sealed class LaunchRocket : EnemyAction
{
    public override string GetDescription() => $"Shoot Rocket ";
    public override void ExecuteAction()
    {
        Enemy rocket = EnemySpawner.SpawnEnemy("Rocket");
        rocket?.SetDistance(LaunchFrom.Distance - 0.2f);
    }
    EnemySpawner EnemySpawner;
    Enemy LaunchFrom;
    public LaunchRocket(EnemySpawner enemySpawner, Enemy launchFrom, float timeSpan)
    { EnemySpawner = enemySpawner; LaunchFrom = launchFrom; TimeSpan = timeSpan; }
}

sealed class TeleportAllPlayers : EnemyAction
{
    public override string GetDescription() => "teleport all players";
    public override void ExecuteAction()
    {
        GameObject[] Rooms = GameObject.FindGameObjectsWithTag("Room");
        foreach (Player player in Player.GetAllPlayers())
        {
            int rndId = Random.Range(0, Rooms.Length);
            player.RequestChangingRoom(roomName: Rooms[rndId].name, conectToPanel: false, ignoreRestrictions: true);

            AudioManager.GetAudioManager().RequestPlayingSentenceOnClient("youHaveBeenTeleported_r");
        }
    }
    public TeleportAllPlayers(float timeSpan)
    { TimeSpan = timeSpan; }
}

sealed class CloseAllDoors : EnemyAction
{
    public override string GetDescription() => "closes sll doors";
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

sealed class DepleteShields : EnemyAction
{
    public override string GetDescription() => $"{EnergyToDeplete} damage to all shields";
    public override void ExecuteAction()
    {
        GameObject[] Zones = GameObject.FindGameObjectsWithTag("Zone");

        foreach (var z in Zones)
        {
            z.GetComponent<Zone>().DepleteEnergyShiealds(EnergyToDeplete);
        }
    }

    int EnergyToDeplete;
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
        EnergyPool[] energyPools = Zone.transform.Find("ShipCanvas").GetComponentsInChildren<EnergyPool>();

        int _energyToDeplete = EnergyToDeplete;
        foreach (var e in energyPools)
        {
            _energyToDeplete -= e.PullEnergyUpTo(_energyToDeplete);
        }
    }
    public override string GetDescription() => $"deplete {EnergyToDeplete} energy";

    int EnergyToDeplete;
    Zone Zone;
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
    public override string GetDescription() => $"Speeds up by {SpeedUpValue}";

    float SpeedUpValue;
    Enemy Enemy;
    public SpeedUp(Enemy enemy, float speedUpValue)
    {
        SpeedUpValue = speedUpValue;
        Enemy = enemy;
    }
}

#endif

