using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;


public class UpdateUIActions
{
    private Action Body;

    // old
    // public UpdateUIActions(NetworkVariable<T> nv)
    // {
    //     Body = () => { };
    //     nv.OnValueChanged = UpdateUI;
    // }

    public UpdateUIActions()
    {
        Body = () => { };
    }

    public void AddOnValueChangeDependency<T>(NetworkVariable<T> nv)
    {
        nv.OnValueChanged = UpdateUI<T>;
    }

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

    // old
    // void UpdateUI(T t1, T t2)
    // {
    //     Body();
    // }
}
