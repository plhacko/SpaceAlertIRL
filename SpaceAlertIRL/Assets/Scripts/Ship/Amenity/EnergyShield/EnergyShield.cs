#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnergyShield : Amenity<EnergyShield>
{
    protected const int MaxShieldValueConst = 3;
    protected const int ShieldValueConst = 2;

    NetworkVariable<int> MaxShieldValue = new NetworkVariable<int>(ShieldValueConst);
    NetworkVariable<int> ShieldValue = new NetworkVariable<int>(MaxShieldValueConst);

    public int GetMaxShieldValue() => MaxShieldValue.Value;
    public int GetShieldValue() => ShieldValue.Value;

#if (SERVER)
    private void RechargeEnergyShield(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        int requestedEnergy = Mathf.Max(MaxShieldValue.Value - ShieldValue.Value, 0); // requestedEnergy must be more than 0

        int receivedEnergy = Room.EnergySource.PullEnergyUpTo(requestedEnergy);
        ShieldValue.Value = ShieldValue.Value + receivedEnergy;

        if (receivedEnergy == 0)
        {
            // message for the player, tahat there was not enough energy
            GameObject.Find("AudioManager").GetComponent<AudioManager>().RequestPlayingSentenceOnClient("netEnoughEnergy_r", clientId: clientId);
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

        UIActions.AddOnValueChangeDependency(ShieldValue, MaxShieldValue);
    }

    public override void Restart()
    {
        MaxShieldValue.Value = ShieldValueConst;
        ShieldValue.Value = MaxShieldValueConst;
    }
}
