using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public void DestroySelf()
    {
        GameObject.Destroy(gameObject);
    }
}
