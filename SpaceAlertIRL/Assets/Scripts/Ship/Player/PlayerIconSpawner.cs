using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIconSpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject PlayerIconPrefab;

    // Start is called before the first frame update
    void Start()
    {
        SpawnAllPlayerIcons();
    }

    void SpawnAllPlayerIcons()
    {
        // remove old ones
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // spawns new playerIcon Objects
        foreach (Player player in Player.GetAllPlayers())
        {
            GameObject _go = Instantiate(PlayerIconPrefab, transform.position, transform.rotation, transform);
            _go.GetComponent<PlayerIcon>().Initialise(player);
        }
    }
}
