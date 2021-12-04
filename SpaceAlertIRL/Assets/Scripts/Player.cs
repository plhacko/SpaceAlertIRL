
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;


public class Player : NetworkBehaviour
{
    // this singals, where the player is currently loccated
    // the important part is, that the players can't change this variable themself, they must request the server
    public NetworkVariableString CurrentRoomName = new NetworkVariableString(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });


    public NetworkVariableString Name = new NetworkVariableString(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.OwnerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    


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
    }
}
