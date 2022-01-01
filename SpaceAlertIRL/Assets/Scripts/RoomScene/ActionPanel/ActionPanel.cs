using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject RoomActionPanelPrefab;
    [SerializeField]
    private GameObject PowerGeneratorActionPanelPrefab;
    [SerializeField]
    private GameObject EnergyPoolActionPanelPrefab;
    [SerializeField]
    private GameObject EnergyNodeActionPanelPrefab;

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

    public void DisplayThis(EnergyNode energyNode)
    {
        ResetSelf();
        GameObject _go = Instantiate(EnergyNodeActionPanelPrefab, transform.position, transform.rotation, transform);
        _go.GetComponent<EnergyNodeActionPanel>().Initialise(energyNode);
    }

    public void DisplayThis(Player player)
    {
        // TODO: ...
    }
    public void DisplayThis(EnergyPool energyPool)
    {
        ResetSelf();
        GameObject _go = Instantiate(EnergyPoolActionPanelPrefab, transform.position, transform.rotation, transform);
        _go.GetComponent<EnergyPoolActionPanel>().Initialise(energyPool);
    }
    public void DisplayThis(PowerGenerator powerGenerator)
    {
        ResetSelf();
        GameObject _go = Instantiate(PowerGeneratorActionPanelPrefab, transform.position, transform.rotation, transform);
        _go.GetComponent<PowerGeneratorActionPanel>().Initialise(powerGenerator);
    }

    private void Start()
    {
        ResetSelf();
    }
}
