using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Unity.Netcode;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class PowerGeneratorActionPanel : AmenityActionPanel<PowerGenerator>
{
    TextMeshProUGUI Status, Energy, PowerCell;
    Image BurnPowerCellButton;

    private void Awake()
    {
        Status = transform.Find("Status").GetComponentInChildren<TextMeshProUGUI>();
        Energy = transform.Find("Energy").GetComponentInChildren<TextMeshProUGUI>();
        PowerCell = transform.Find("PowerCell").GetComponentInChildren<TextMeshProUGUI>();

        BurnPowerCellButton = transform.Find("BurnPowerCellButton").GetComponent<Image>();
    }
    protected override void UpdateUI()
    {
        var _energyStorage = Amenity.EnergyStorage;
        var _maxEnergyStorage = Amenity.MaxEnergyStorage;
        var _energyPowerCellCount = Amenity.EnergyPowerCellCount;

        Status.text = "Status : good";
        Energy.text = $"Energy : {_energyStorage}/{_maxEnergyStorage}";
        PowerCell.text = $"Power Cell : {_energyPowerCellCount}";


        Color c = BurnPowerCellButton.color;
        c.a = _energyStorage < _maxEnergyStorage ? 1f : 0.6f;
        BurnPowerCellButton.color = c;
    }

    public void RequestBurningOfPowerCell()
    {
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        Amenity.RequestBurningPowerCellServerRpc(clientId: clientId);
    }
}
