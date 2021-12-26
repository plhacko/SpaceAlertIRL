
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : NetworkBehaviour
{
    const string BasePlayerName = "pName";
    const string StartingRoom = "B0";

    // this singals, where the player is currently loccated
    // the important part is, that the players can't change this variable themself, they must request the server
    public NetworkVariableString CurrentRoomName = new NetworkVariableString(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    }, StartingRoom);
    public UpdateUIActions CurrentRoomNameUIActions;

    public NetworkVariableString Name = new NetworkVariableString(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    }, BasePlayerName);
    public UpdateUIActions NameUIActions;

    // TODO: remove this placeholder
    public string Status { get => "alive"; }

    [ServerRpc]
    public void ChangeRoomServerRpc(string roomName, ServerRpcParams rpcParams = default)
    {
        Room r = GameObject.Find(CurrentRoomName.Value).GetComponent<Room>();
        foreach (var d in r.Doors)
        {
            if (d.IsOpen.Value && (d.RoomA.Name == roomName || d.RoomB.Name == roomName))
            {
                CurrentRoomName.Value = roomName;
                return;
            }
        }
        // TODO: add: if player canot go, give the player feed back
    }

    void RestartScene()
    {
        SceneManager.LoadScene("RoomScene");
    }

    private void Start()
    {
        CurrentRoomNameUIActions = new UpdateUIActions();
        CurrentRoomNameUIActions.AddOnValueChangeDependency(CurrentRoomName);
        if (IsLocalPlayer)
        { CurrentRoomNameUIActions.AddAction(RestartScene); }

        NameUIActions = new UpdateUIActions();
        NameUIActions.AddOnValueChangeDependency(Name);
    }



    // TODO: dselete this
    public NetworkVariableVector3 Position = new NetworkVariableVector3(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });


    // TODO: delete this
    public override void NetworkStart()
    {
        //todo: smazat
        //Move(); // you cannot move with object if you don't own the object (there should be a possibility to diable that, but I don't need it now)
    }

    // TODO: delete this
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
    [ServerRpc]
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

        // _Counter.Value++;
    }
}
