using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPanelSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject DoorActionPanelPrefab;
    [SerializeField]
    private GameObject PowerGeneratorActionPanelPrefab;
    [SerializeField]
    private GameObject EnergyPoolActionPanelPrefab;
    [SerializeField]
    private GameObject EnergyNodeActionPanelPrefab;
    [SerializeField]
    private GameObject EnergyShieldActionPanelPrefab;
    [SerializeField]
    private GameObject LaserActionPanelPrefab;

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
        GameObject _go = Instantiate(DoorActionPanelPrefab, transform.position, transform.rotation, transform);
        _go.GetComponent<DoorActionPanel>().Door = door;
    }

    public void DisplayThis(EnergyShield energyShield)
    {
        ResetSelf();
        GameObject _go = Instantiate(EnergyShieldActionPanelPrefab, transform.position, transform.rotation, transform);
        _go.GetComponent<EnergyShieldActionPanel>().Initialise(energyShield);
    }

    public void DisplayThis(Laser laser)
    {
        ResetSelf();
        GameObject _go = Instantiate(LaserActionPanelPrefab, transform.position, transform.rotation, transform);
        _go.GetComponent<LaserActionPanel>().Initialise(laser);
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
        throw new System.NotImplementedException();
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
