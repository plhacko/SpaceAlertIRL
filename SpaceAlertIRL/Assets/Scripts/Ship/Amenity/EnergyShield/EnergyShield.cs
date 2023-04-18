#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnergyShield : Amenity<EnergyShield>
{
    protected const int MaxShieldValueConst = 3;
    protected const int ShieldValueConst = 2;

    NetworkVariable<int> _MaxShieldValue = new NetworkVariable<int>(MaxShieldValueConst);
    NetworkVariable<int> _ShieldValue = new NetworkVariable<int>(ShieldValueConst);

    public int MaxShieldValue { get => _MaxShieldValue.Value; }
    public int ShieldValue { get => _ShieldValue.Value; }

    // UI
    BubbleProgressBar BubbleProgressBar;

#if (SERVER)
    private void RechargeEnergyShield(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer) { throw new System.Exception("Is not a server"); }

        int requestedEnergy = Mathf.Max(_MaxShieldValue.Value - _ShieldValue.Value, 0); // requestedEnergy must be more than 0

        int receivedEnergy = Room.EnergySource.PullEnergyUpTo(requestedEnergy);
        _ShieldValue.Value = _ShieldValue.Value + receivedEnergy;

        if (receivedEnergy == 0)
        {
            // message for the player, tahat there was not enough energy
            AudioManager.GetAudioManager().RequestPlayingSentenceOnClient("netEnoughEnergy_r", clientId: clientId);
        }
        else { GetComponentInParent<Zone>().UIActions.UpdateUI(); }
    }

    public void AbsorbDamage(ref int dmg)
    {
        int damageReduction = Mathf.Min(dmg, _ShieldValue.Value);

        _ShieldValue.Value = _ShieldValue.Value - damageReduction;
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

        BubbleProgressBar = GetComponent<BubbleProgressBar>();

        UIActions.AddOnValueChangeDependency(_ShieldValue, _MaxShieldValue);
        UIActions.AddAction(UpdateUI);
        UIActions.UpdateUI();
    }

    public override void Restart()
    {
        _MaxShieldValue.Value = MaxShieldValueConst;
        _ShieldValue.Value = ShieldValueConst;
    }

    void UpdateUI()
    {
        // spawn energy circles
        BubbleProgressBar.UpdateUI(ShieldValue, MaxShieldValue);
    }
}
