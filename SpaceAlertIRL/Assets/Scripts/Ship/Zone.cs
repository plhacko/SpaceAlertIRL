#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Zone : NetworkBehaviour
{
    public NetworkVariable<int> TmpDmgTaken = new NetworkVariable<int> (0);
    protected List<Enemy> EnemyList = new List<Enemy>();

    public IList<Enemy> GetEnemyList() => EnemyList.AsReadOnly();

#if (SERVER)
    public void TakeDmage(int damage)
    {
        if (!NetworkManager.Singleton.IsServer)
        { throw new System.Exception("this method should be called only on server");  }

        TmpDmgTaken.Value = TmpDmgTaken.Value + damage;
        //throw new System.NotImplementedException();
    }
#endif

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
