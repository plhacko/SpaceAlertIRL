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
    private void RechargeEnergyShield(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        EnergyNode energySource = Room.GetEnergySource(); // might me a child of EnergyNode
        int requestedEnergy = Mathf.Max(MaxShieldValue.Value - ShieldValue.Value, 0); // requestedEnergy must be more than 0

        int receivedEnergy = energySource.PullEnergyUpTo(requestedEnergy);
        ShieldValue.Value = ShieldValue.Value + receivedEnergy;

        if (receivedEnergy == 0)
        {
            // message for the player, tahat there was not enough energy
            GameObject.Find("AudioManager").GetComponent<AudioManager>().RequestPlayingSentenceOnClient("netEnoughEnergy_r", clientId);
        }
    }

    public void AbsorbDamage(ref int dmg)
    {
        int damageReduction = Mathf.Min(dmg, ShieldValue.Value);

        ShieldValue.Value = ShieldValue.Value - damageReduction;
        dmg = dmg - damageReduction;
    }
#endif

    [ServerRpc(RequireOwnership = false)]
    public void RequestRechargeEnergyShieldServerRpc(ulong clientId)
    {
        RechargeEnergyShield(clientId);
    }

    protected override void Start()
    {
        base.Start();

        ShieldValue = new NetworkVariable<int>(ShieldValueConst);
        MaxShieldValue = new NetworkVariable<int>(MaxShieldValueConst);

        UIActions.AddOnValueChangeDependency(ShieldValue, MaxShieldValue);
    }
}
