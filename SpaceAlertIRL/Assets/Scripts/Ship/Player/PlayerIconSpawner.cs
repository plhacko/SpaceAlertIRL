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
        foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            GameObject _go = Instantiate(PlayerIconPrefab, transform.position, transform.rotation, transform);
            Player _player = playerObject.GetComponent<Player>();
            _go.GetComponent<PlayerIcon>().Initialise(_player);
        }
    }
}
