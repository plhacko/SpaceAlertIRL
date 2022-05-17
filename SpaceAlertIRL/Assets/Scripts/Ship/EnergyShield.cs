#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnergyShield : Amenity<EnergyShield>
{
    protected const int MaxShieldValueConst = 3;
    protected const int ShieldValueConst = 2;

    public NetworkVariable<int> MaxShieldValue;
    public NetworkVariable<int> ShieldValue;


#if (SERVER)
    private void RechargeEnergyShield()
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        EnergyNode energySource = Room.GetEnergySource(); // might me a child of EnergyNode
        int requestedEnergy = Mathf.Max(MaxShieldValue.Value - ShieldValue.Value, 0); // requestedEnergy must be more than 0

        int receivedEnergy = energySource.PullEnergyUpTo(requestedEnergy);
        ShieldValue.Value = ShieldValue.Value + receivedEnergy;

        if (receivedEnergy == 0)
        {
            //TODO: make message to the Player, that the transaction eneded with 0 energy added
            // energy source depleated, no energy transphered
        }
    }
#endif

    [ServerRpc(RequireOwnership = false)]
    public void RequestRechargeEnergyShieldServerRpc()
    {
        RechargeEnergyShield();
    }

    protected override void Start()
    {
        base.Start();

        ShieldValue = new NetworkVariable<int>(ShieldValueConst);
        MaxShieldValue = new NetworkVariable<int>(MaxShieldValueConst);

        UIActions.AddOnValueChangeDependency(ShieldValue, MaxShieldValue);
    }
}
