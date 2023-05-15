using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;
using TMPro;
using Unity.Collections;
using UnityEngine.UI;
using System.Net;

public class Player : NetworkBehaviour, IRestart
{
    const string BasePlayerName = "noName";
    const string StartingRoom = "B0";

    // this singals, where the player is currently loccated
    // the important part is, that the players can't change this variable themself, they must request the server
    public NetworkVariable<FixedString32Bytes> CurrentRoomName = new NetworkVariable<FixedString32Bytes>(StartingRoom);
    
    private NetworkVariable<FixedString32Bytes> _Name = new NetworkVariable<FixedString32Bytes>(BasePlayerName);

    public UpdateUIActions UIActions;

    public string Status { get => "alive"; }
    public string Name { get => _Name.Value.ToString(); }

    public static Player[] GetAllPlayers()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        Player[] players = new Player[playerObjects.Length];
        for (int i = 0; i < playerObjects.Length; i++)
        { players[i] = playerObjects[i].GetComponent<Player>(); }
        return players;
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

    public void RequestChangingRoom(string roomName, bool ignoreRestrictions = false)
    {
        if (roomName == CurrentRoomName.Value) { return; }

        ulong clientId = NetworkManager.Singleton.LocalClientId;
        ChangeRoomServerRpc(roomName, clientId, ignoreRestrictions);
    }

    void Start()
    {
        UIActions = new UpdateUIActions();
        UIActions.AddOnValueChangeDependency(CurrentRoomName);
        if (IsLocalPlayer)
        { UIActions.AddAction(RestartScene);}

        UIActions = new UpdateUIActions();
        UIActions.AddOnValueChangeDependency(_Name);
    }

    [ServerRpc]
    void ChangeRoomServerRpc(FixedString32Bytes newRoomName, ulong clientId, bool ignoreRestrictions = false, ServerRpcParams rpcParams = default)
    {
        // going through the teleport
        if (CurrentRoomName.Value == "Teleport")
        {
            ignoreRestrictions = true;
            AudioManager.GetAudioManager().RequestPlayingSentenceOnClient("youHaveBeenTeleported_r", clientId: clientId);
        }

        // set new room name
        if (ignoreRestrictions)
        { CurrentRoomName.Value = newRoomName; }
        else if (CurrentRoomName.Value == newRoomName)
        { }
        else if (!CanGoThroughDoors())
        {
            // if there are no doors to go through, give the player audio feed back
            AudioManager.GetAudioManager().RequestPlayingSentenceOnClient("youShellNotPass_r doorsAreClosed_r", clientId: clientId);
        }
        else
        {
            CurrentRoomName.Value = newRoomName;
        }

        bool CanGoThroughDoors()
        {
            // going through the doors
            Room r = GameObject.Find(CurrentRoomName.Value.ToString()).GetComponent<Room>();
            foreach (var d in r.Doors)
            {
                if (d.IsOpen && (d.RoomA.Name == newRoomName || d.RoomB.Name == newRoomName))
                {
                    return true;
                }
            }
            return false;
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
        _Name.Value = playerName;
    }

    public void Restart()
    {
        CurrentRoomName.Value = StartingRoom;
        _Name.Value = BasePlayerName;
    }
}
