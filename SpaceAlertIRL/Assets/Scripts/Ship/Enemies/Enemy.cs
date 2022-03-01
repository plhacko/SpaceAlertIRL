using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class Enemy : NetworkBehaviour
{
    [SerializeField]
    protected GameObject IconPrefab;

    abstract protected int StratingHPConst { get; }
    abstract protected int StartingEnergyShieldsConst { get; }

    protected NetworkVariable<int> _HP;
    protected NetworkVariable<int> _EnergyShields;

    protected Zone Zone;

    public UpdateUIActions UIActions = new UpdateUIActions();

    public abstract void SpawnIconAsChild(GameObject parent);

    protected virtual void Start()
    {
        Zone = GetComponentInParent<Zone>();
        Zone.AddEnemy(this);

        // TODO: this used to be problem elsewere (If I remember correctly) -> this needs to be set up only on server
        _HP = new NetworkVariable<int>(StratingHPConst);
        _EnergyShields = new NetworkVariable<int>(StartingEnergyShieldsConst);

        UIActions.AddOnValueChangeDependency(_HP);
        UIActions.AddOnValueChangeDependency(_EnergyShields);
    }

    abstract public void TakeDamage(int damage, Weapon w); // the weapon is needed if there is a special exception
    virtual public void Die()
    {
        GetComponent<NetworkObject>().Despawn(true);
    }



    // geters for UI
    public int HP { get => _HP.Value; }
    public int MaxHP { get => StratingHPConst; }
    public int EnergyShields { get => _EnergyShields.Value; }
    public int MaxEnergyShields { get => StartingEnergyShieldsConst; }


}
