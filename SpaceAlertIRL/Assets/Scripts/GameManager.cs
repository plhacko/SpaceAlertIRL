//rm using MLAPI;
//rm using MLAPI.Transports.UNET;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HelloWorld //TODO: pøejhmenovat nebo smazat namespace
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
                //StartButtons();
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
            // TODO: add changing IP address
            // NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = IPAddressToJoin.text;
            NetworkManager.Singleton.StartClient();
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
                if (NetworkManager.Singleton.ConnectedClients.TryGetValue(NetworkManager.Singleton.LocalClientId,
                    out var networkedClient))
                {
                    var player = networkedClient.PlayerObject.GetComponent<Player>();
                    if (player)
                    {
                        player.Move();
                    }
                }

            }
        }
    }
}