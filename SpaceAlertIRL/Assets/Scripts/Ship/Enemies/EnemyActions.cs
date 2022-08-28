#define SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


#if SERVER

public abstract class EnemyAction
{
    public float TimeSpan { get; protected set; }

    public abstract string GetDescription();
    public abstract void ExecuteAction();
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
    public LaunchRocket(EnemySpawner enemySpawner, Enemy launchFrom, float timeSpan) //TODO: add changing rocket damage, speed, ...
    { EnemySpawner = enemySpawner; LaunchFrom = launchFrom; TimeSpan = timeSpan; }
}

sealed class TeleportAllPlayers : EnemyAction
{
    public override string GetDescription() => "teleport all players";
    public override void ExecuteAction()
    {
        GameObject[] Rooms = GameObject.FindGameObjectsWithTag("Room");
        foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            Player player = playerObject.GetComponent<Player>();
            int rndId = Random.Range(0, Rooms.Length);
            // player.CurrentRoomName.Value = Rooms[rndId].name; //TODO: rm
            player.RequestChangingRoom(Rooms[rndId].name, false);

            GameObject.Find("AudioManager").GetComponent<AudioManager>().RequestPlayingSentenceOnClient("youHaveBeenTeleported_r");
        }
    }
    public TeleportAllPlayers(float timeSpan)
    { TimeSpan = timeSpan; }
}

#endif

