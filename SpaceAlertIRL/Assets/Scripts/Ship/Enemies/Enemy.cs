using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class Enemy : NetworkBehaviour
{
    abstract protected int StratingHPConst { get; }
    abstract protected int StartingEnergyShieldsConst { get; }

    protected NetworkVariable<int> HP;
    protected NetworkVariable<int> EnergyShields;

    protected Zone Zone;

    public UpdateUIActions UIActions = new UpdateUIActions();

    public abstract void SpawnIconAsChild(GameObject parent);

    public virtual void Start()
    {
        Zone = GetComponentInParent<Zone>();

        HP = new NetworkVariable<int>(StratingHPConst);
        EnergyShields = new NetworkVariable<int>(StratingHPConst);
    }

    abstract public void TakeDamage(Weapon w); // the weapon is needed if there is a special exception
    abstract public void Die();


}
