using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;
using TMPro;
using Unity.Collections;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    const string BasePlayerName = "pName";
    const string StartingRoom = "B0";

    // this singals, where the player is currently loccated
    // the important part is, that the players can't change this variable themself, they must request the server
    public NetworkVariable<FixedString32Bytes> CurrentRoomName = new NetworkVariable<FixedString32Bytes>(StartingRoom);
    public UpdateUIActions CurrentRoomNameUIActions;

    // if is connected to the panel the player canot change rooms until he/she disconnects
    public NetworkVariable<bool> IsConnectedToPanel = new NetworkVariable<bool>(true);

    public NetworkVariable<FixedString32Bytes> Name = new NetworkVariable<FixedString32Bytes>(BasePlayerName);
    public UpdateUIActions NameUIActions;

    public string Status { get => "alive"; }

    public static Player GetLocalPlayer()
    {
        Player player;
        foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            player = playerObject.GetComponent<Player>();
            if (player.OwnerClientId == NetworkManager.Singleton.LocalClientId)
            {
                return player;
            }
        }
        return null;
    }

    public void RequestChangingRoom(string roomName, bool conectToPanel, bool ignoreRestrictions = false)
    {
        if (roomName == CurrentRoomName.Value) { return; }

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
        NameUIActions.AddOnValueChangeDependency(Name);
    }

    [ServerRpc]
    void ChangeRoomServerRpc(FixedString32Bytes newRoomName, bool conectToPanel, ulong clientId, bool ignoreRestrictions = false, ServerRpcParams rpcParams = default)
    {

        // going through the teleport
        if (CurrentRoomName.Value == "Teleport")
        { ignoreRestrictions = true; }

        // set new room name
        if (ignoreRestrictions)
        { CurrentRoomName.Value = newRoomName; }
        else if (!IsConnectedToPanel.Value)
        { GoThroughDoors(); }

        // sets if the player is connected to panel
        IsConnectedToPanel.Value = conectToPanel;

        void GoThroughDoors()
        {
            // going through the doors
            Room r = GameObject.Find(CurrentRoomName.Value.ToString()).GetComponent<Room>();
            foreach (var d in r.Doors)
            {
                if (d.IsOpen.Value && (d.RoomA.Name == newRoomName || d.RoomB.Name == newRoomName))
                {
                    CurrentRoomName.Value = newRoomName;
                    return;
                }
            }
            // if there are no doors to go through, give the player audio feed back
            GameObject.Find("AudioManager").GetComponent<AudioManager>().RequestPlayingSentenceOnClient("youShellNotPass_r doorsAreClosed_r", clientId: clientId);
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
        Name.Value = playerName;
    }

}
