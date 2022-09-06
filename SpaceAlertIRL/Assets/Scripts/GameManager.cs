using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public InputField IPAddressToJoin;

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

    public void ServerGame()
    {
        NetworkManager.Singleton.StartServer();
    }

}
