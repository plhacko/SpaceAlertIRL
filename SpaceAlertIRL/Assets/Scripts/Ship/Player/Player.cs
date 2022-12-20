using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;
using TMPro;
using Unity.Collections;
using UnityEngine.UI;

public class Player : NetworkBehaviour, IRestart
{
    const string BasePlayerName = "pName";
    const string StartingRoom = "B0";

    // this singals, where the player is currently loccated
    // the important part is, that the players can't change this variable themself, they must request the server
    public NetworkVariable<FixedString32Bytes> CurrentRoomName = new NetworkVariable<FixedString32Bytes>(StartingRoom);
    public UpdateUIActions CurrentRoomNameUIActions;

    // if is connected to the panel the player canot change rooms until he/she disconnects
    public NetworkVariable<bool> IsConnectedToPanel = new NetworkVariable<bool>(true);
    public UpdateUIActions IsConnectedToPanelUIActions;

    private NetworkVariable<FixedString32Bytes> _Name = new NetworkVariable<FixedString32Bytes>(BasePlayerName);
    public UpdateUIActions NameUIActions;

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

    public void RequestChangingRoom(string roomName, bool conectToPanel, bool ignoreRestrictions = false)
    {
        if (roomName == CurrentRoomName.Value && conectToPanel == IsConnectedToPanel.Value) { return; }

        ulong clientId = NetworkManager.Singleton.LocalClientId;
        ChangeRoomServerRpc(roomName, conectToPanel, clientId, ignoreRestrictions);
    }

    void Start()
    {
        CurrentRoomNameUIActions = new UpdateUIActions();
        CurrentRoomNameUIActions.AddOnValueChangeDependency(CurrentRoomName);
        CurrentRoomNameUIActions.AddOnValueChangeDependency(IsConnectedToPanel);
        if (IsLocalPlayer)
        { CurrentRoomNameUIActions.AddAction(RestartScene); }

        NameUIActions = new UpdateUIActions();
        NameUIActions.AddOnValueChangeDependency(_Name);

        IsConnectedToPanelUIActions = new UpdateUIActions();
        IsConnectedToPanelUIActions.AddOnValueChangeDependency(IsConnectedToPanel);

    }

    [ServerRpc]
    void ChangeRoomServerRpc(FixedString32Bytes newRoomName, bool conectToPanel, ulong clientId, bool ignoreRestrictions = false, ServerRpcParams rpcParams = default)
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
        else if (IsConnectedToPanel.Value)
        {
            // if the player is still connected to the panel and is trying to change rooms, give the player audio feed back
            AudioManager.GetAudioManager().RequestPlayingSentenceOnClient("youShellNotPass_r disconnectThePanel_r", clientId: clientId);
        }
        else if (!CanGoThroughDoors())
        {
            // if there are no doors to go through, give the player audio feed back
            AudioManager.GetAudioManager().RequestPlayingSentenceOnClient("youShellNotPass_r doorsAreClosed_r", clientId: clientId);
        }
        else
        {
            CurrentRoomName.Value = newRoomName;
        }

        // sets if the player is connected to panel (only if the tag is the same as the room the player is right now in)
        if (CurrentRoomName.Value == newRoomName)
        { IsConnectedToPanel.Value = conectToPanel; }

        bool CanGoThroughDoors()
        {
            // going through the doors
            Room r = GameObject.Find(CurrentRoomName.Value.ToString()).GetComponent<Room>();
            foreach (var d in r.Doors)
            {
                if (d.IsOpen.Value && (d.RoomA.Name == newRoomName || d.RoomB.Name == newRoomName))
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
        RequestChngeingPlayerNameServerRpc(playerName);
    }

    [ServerRpc]
    void RequestChngeingPlayerNameServerRpc(string playerName)
    {
        _Name.Value = playerName;
    }

    public void Restart()
    {
        CurrentRoomName.Value = StartingRoom;
        IsConnectedToPanel.Value = true;
        _Name.Value = BasePlayerName;
    }
}
