using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;
using TMPro;
using Unity.Collections;
using UnityEngine.UI;
using System.Net;
using System;
using System.Collections.Generic;

public class Player : NetworkBehaviour, IRestart
{
    const string BasePlayerName = "NotSure";
    const string StartingRoom = "B0";

    // this singals, where the player is currently loccated
    // the important part is, that the players can't change this variable themself, they must request the server
    private NetworkVariable<FixedString32Bytes> _CurrentRoomName = new NetworkVariable<FixedString32Bytes>(StartingRoom);
    private NetworkVariable<FixedString32Bytes> _Name = new NetworkVariable<FixedString32Bytes>(BasePlayerName);
    private NetworkVariable<bool> _IsDead = new NetworkVariable<bool>(false);

    public FixedString32Bytes CurrentRoomName { get => _CurrentRoomName.Value; private set { _CurrentRoomName.Value = value; } }
    public FixedString32Bytes Name { get => _Name.Value; private set { _Name.Value = value; } }
    public bool IsDead { get => _IsDead.Value; private set { _IsDead.Value = value; } }

    public UpdateUIActions UIActions;
    public UpdateUIActions UIActions_name;

    public string Status { get => _IsDead.Value ? "dead" : "alive"; }

    public static Player[] GetAllPlayers()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        Player[] players = new Player[playerObjects.Length];
        for (int i = 0; i < playerObjects.Length; i++)
        { players[i] = playerObjects[i].GetComponent<Player>(); }
        return players;
    }
    public static Player[] GetAllPlayersInZone(Zone zone)
    {
        Player[] allPlayers = GetAllPlayers();
        Room[] roomsInZone = zone.GetComponentsInChildren<Room>();

        List<Player> playersInZone = new List<Player>();
        foreach (Player p in allPlayers)
        {
            string playerRoomName = p.CurrentRoomName.ToString();
            foreach (Room r in roomsInZone)
            {
                if (playerRoomName == r.name)
                { playersInZone.Add(p); break; }
            }
        }
        return playersInZone.ToArray();
    }
    public static Player GetLocalPlayer()
    {
        foreach (Player player in Player.GetAllPlayers())
        {
            if (player.OwnerClientId == NetworkManager.Singleton.LocalClientId)
            { return player; }
        }
        return null;
    }

    public void RequestChangingRoom(string roomName, bool ignoreRestrictions = false, bool silent = false)
    {
        if (roomName == CurrentRoomName.Value) { return; }

        ulong clientId = NetworkManager.Singleton.LocalClientId;
        ChangeRoomServerRpc(newRoomName: roomName, clientId: clientId, ignoreRestrictions: ignoreRestrictions, silent: silent);
    }

    void Start()
    {
        UIActions_name = new UpdateUIActions();
        UIActions_name.AddOnValueChangeDependency(_Name);

        UIActions = new UpdateUIActions();
        UIActions.AddOnValueChangeDependency(_CurrentRoomName);
        UIActions.AddOnValueChangeDependency(_IsDead);
        if (IsLocalPlayer)
        { UIActions.AddAction(RestartScene); }

        // UI
        foreach (var pis in FindObjectsOfType<PlayerIconSpawner>())
        { pis.SpawnAllPlayerIcons(); }
    }

    void OnDisable()
    {
        // UI
        foreach (var pis in FindObjectsOfType<PlayerIconSpawner>())
        { pis.SpawnAllPlayerIcons(); }
    }

    [ServerRpc]
    void ChangeRoomServerRpc(FixedString32Bytes newRoomName, ulong clientId, bool ignoreRestrictions = false, bool silent = false, ServerRpcParams rpcParams = default)
    {
        // deatch Check
        if (IsDead)
        {
            if (!silent)
            { AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.fail, clientId: clientId); }
            return;
        }

        // going through the teleport
        if (CurrentRoomName.Value == "Teleport")
        {
            ignoreRestrictions = true;
            if (!silent)
            {
                AudioManager.Instance.RequestPlayingSentenceOnClient("youHaveBeenTeleported_r", clientId: clientId);
                AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.success, clientId: clientId);
            }
        }

        // set new room name
        if (ignoreRestrictions)
        {
            if (!silent) { AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.success, clientId: clientId); }
            CurrentRoomName = newRoomName; return;
        }

        if (CurrentRoomName.Value == newRoomName)
        { return; }

        List<Door> doors = FindGoThroughDoors();
        if (doors.Count == 0)
        {
            if (!silent)
            {
                AudioManager.Instance.RequestPlayingSentenceOnClient("notConnectedByDoors_r", clientId: clientId);
                AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.fail, clientId: clientId);
            }
            return;
        }
        if (AreAllDoorsClosed(doors))
        {
            if (!silent)
            {
                AudioManager.Instance.RequestPlayingSentenceOnClient("doorsAreClosed_r", clientId: clientId);
                AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.fail, clientId: clientId);
            }
            return;
        }
        else
        {
            CurrentRoomName = newRoomName;
            AudioManager.Instance.RequestVibratingSentenceOnClient(VibrationDuration.success, clientId: clientId);
            return;
        }

        bool AreAllDoorsClosed(List<Door> doors)
        {
            foreach (Door d in doors)
            { if (d.IsOpen) { return false; } }
            return true;
        }

        // returns list of doors that lead to the next room
        List<Door> FindGoThroughDoors()
        {
            List<Door> result = new List<Door>();

            // going through the doors
            Room r = GameObject.Find(CurrentRoomName.Value.ToString()).GetComponent<Room>();
            foreach (var d in r.Doors)
            {
                if (d.RoomA.Name == newRoomName || d.RoomB.Name == newRoomName)
                {
                    result.Add(d);
                }
            }
            return result;
        }
    }

    void RestartScene()
    {
        SceneManager.LoadScene("RoomScene");
    }

    public void RequetsSettingLocalPlayerName(string playerName)
    {
        RequestSettingPlayerNameServerRpc(playerName);
    }

    [ServerRpc]
    void RequestSettingPlayerNameServerRpc(string playerName)
    {
        Name = playerName;
    }

    public void Restart()
    {
        CurrentRoomName = StartingRoom;
        IsDead = false;
    }

    internal void Kill()
    {
        IsDead = true;
    }
}
