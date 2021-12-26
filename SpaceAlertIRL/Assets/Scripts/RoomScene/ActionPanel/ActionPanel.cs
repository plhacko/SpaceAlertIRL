using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPanel : MonoBehaviour
{
    public GameObject RoomActionPanelPrefab;

    public void ResetSelf()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    // this method might be better placed on a differen space (may be it would be better to use extension?)
    public void DisplayThis(Door door)
    {
        ResetSelf();
        GameObject _go = Instantiate(RoomActionPanelPrefab, transform.position, transform.rotation, transform);
        _go.GetComponent<DoorActionPanel>().Door = door;
    }

    public void DisplayThis(Player player)
    {
        // TODO: ...
    }

    private void Start()
    {
        ResetSelf();
    }
}
