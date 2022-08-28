using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
//rm using MLAPI;
//rm using MLAPI.NetworkVariable;


public class UpdateUIActions
{
    private Action Body;

    public UpdateUIActions()
    {
        Body = () => { };
    }
    public void AddOnValueChangeDependency<T>(params NetworkVariable<T>[] nv_array) where T : unmanaged
    { foreach ( var nv in nv_array) { nv.OnValueChanged = UpdateUI<T>; } }
    public void AddAction(Action a) { Body += a; }
    public void RemoveAction(Action a) { Body -= a; }

    public void UpdateUI() { Body(); }
    void UpdateUI<T>(T t1, T t2) { Body(); }
}
