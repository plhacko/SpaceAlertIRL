using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class Icon : MonoBehaviour
{
    protected Action UpdateUIAction;


    abstract protected void UpdateUI();

    abstract protected void OnDisable();



}
