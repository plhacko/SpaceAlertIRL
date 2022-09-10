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

    public NetworkVariable<FixedString32Bytes> Name = new NetworkVariable<FixedString32Bytes>(BasePlayerName);
    public UpdateUIActions NameUIActions;   

    public string Status { get => "alive"; }

    public void RequestChangingRoom(string roomName, bool checkForDoors = true)
    {
        if (roomName == CurrentRoomName.Value) { return; }

        ulong clientId = NetworkManager.Singleton.LocalClientId;
        ChangeRoomServerRpc(roomName, clientId, checkForDoors);
    }

    [ServerRpc]
    void ChangeRoomServerRpc(FixedString32Bytes roomName, ulong clientId, bool checkForDoors = true, ServerRpcParams rpcParams = default)
    {
        // going through the teleport
        if (!checkForDoors || CurrentRoomName.Value == "Teleport")
        {
            CurrentRoomName.Value = roomName;
            return;
        }

        // going through the doors
        Room r = GameObject.Find(CurrentRoomName.Value.ToString()).GetComponent<Room>();
        foreach (var d in r.Doors)
        {
            if (d.IsOpen.Value && (d.RoomA.Name == roomName || d.RoomB.Name == roomName))
            {
                CurrentRoomName.Value = roomName;
                return;
            }
        }
        // if there are no doors to go through, , give the player audio feed back
        GameObject.Find("AudioManager").GetComponent<AudioManager>().RequestPlayingSentenceOnClient("youShellNotPass_r doorsAreClosed_r", clientId: clientId);
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

    void Start()
    {
        CurrentRoomNameUIActions = new UpdateUIActions();
        CurrentRoomNameUIActions.AddOnValueChangeDependency(CurrentRoomName);
        if (IsLocalPlayer)
        { CurrentRoomNameUIActions.AddAction(RestartScene); }

        NameUIActions = new UpdateUIActions();
        NameUIActions.AddOnValueChangeDependency(Name);
    }

}
