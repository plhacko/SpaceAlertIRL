//rm using MLAPI;
//rm using MLAPI.Transports.UNET;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HelloWorld //TODO: p?ejhmenovat nebo smazat namespace
{
    public class GameManager : MonoBehaviour
    {
        public InputField IPAddressToJoin;

        //
        // MLAPI and networking
        //

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                // StartButtons();
            }
            else
            {
                StatusLabels();

                SubmitNewPosition();
            }

            GUILayout.EndArea();
        }

        public void JoinGame()
        {
            try
            {
                NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = IPAddressToJoin.text;
                NetworkManager.Singleton.StartClient();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"unable to join ip address {IPAddressToJoin.text}\nthrown exception: {e.Message}");
            }
        }

        public void HostGame()
        {
            NetworkManager.Singleton.StartHost();
        }

        // TODO: rename?
        public void ServerGame()
        {
            NetworkManager.Singleton.StartServer();
        }

        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        //
        // actual game mechanics
        //

        
        static void SubmitNewPosition()
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
            {
                if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
                {
                    foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
                        NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<Player>().Move();
                }
                else
                {
                    var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                    var player = playerObject.GetComponent<Player>();
                    player.Move();
                }
            }
        }
    }
}