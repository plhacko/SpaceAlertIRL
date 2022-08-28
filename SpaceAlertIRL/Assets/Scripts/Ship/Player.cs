using Unity.Netcode;
//rm using MLAPI;
//rm using MLAPI.Messaging;
//rm using MLAPI.NetworkVariable;
using UnityEngine;
using UnityEngine.SceneManagement;
//rm using UnityEngine.CoreModule;
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

    // TODO: remove this placeholder
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
        // todo: rm
        // Name.Value = GameObject.Find("PlayerName").GetComponent<InputField>().text; // get the players name from a textbox

        CurrentRoomNameUIActions = new UpdateUIActions();
        CurrentRoomNameUIActions.AddOnValueChangeDependency(CurrentRoomName);
        if (IsLocalPlayer)
        { CurrentRoomNameUIActions.AddAction(RestartScene); }

        NameUIActions = new UpdateUIActions();
        NameUIActions.AddOnValueChangeDependency(Name);
    }



    // TODO: dselete this
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

    // TODO: rm
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Move();
        }
    }

    public void Move()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            var randomPosition = GetRandomPositionOnPlane();
            transform.position = randomPosition;
            Position.Value = randomPosition;
        }
        else
        {
            SubmitPositionRequestServerRpc();
        }
    }

    // TODO: delete this
    [ServerRpc(RequireOwnership = false)]
    void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        Position.Value = GetRandomPositionOnPlane();
    }

    // TODO: delete this
    static Vector3 GetRandomPositionOnPlane()
    {
        return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
    }

    // TODO: delete this
    void Update()
    {
        transform.position = Position.Value;

    }
}
