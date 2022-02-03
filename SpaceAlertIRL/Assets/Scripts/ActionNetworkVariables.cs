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

    public void AddOnValueChangeDependency(NetworkVariable<int> nv)
    { nv.OnValueChanged = UpdateUI<int>; }
    public void AddOnValueChangeDependency(NetworkVariable<float> nv)
    { nv.OnValueChanged = UpdateUI<float>; }
    public void AddOnValueChangeDependency(NetworkVariable<bool> nv)
    { nv.OnValueChanged = UpdateUI<bool>; }
    public void AddOnValueChangeDependency(NetworkVariable<FixedString32Bytes> nv)
    { nv.OnValueChanged = UpdateUI<FixedString32Bytes>; }

    public void AddAction(Action a)
    {
        Body += a;
    }

    public void RemoveAction(Action a)
    {
        Body -= a;
    }

    public void UpdateUI()
    {
        Body();
    }

    void UpdateUI<T>(T t1, T t2)
    {
        Body();
    }
}
