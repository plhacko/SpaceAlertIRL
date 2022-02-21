#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Zone : NetworkBehaviour
{
    public NetworkVariable<int> TmpDmgTaken = new NetworkVariable<int> (0);

    [SerializeField]
    protected List<Enemy> EnemyList = new List<Enemy>();

    public IList<Enemy> GetEnemyList() => EnemyList.AsReadOnly();

    public void AddEnemy(Enemy e) { EnemyList.Add(e); }

#if (SERVER)
    public void TakeDmage(int damage)
    {
        if (!NetworkManager.Singleton.IsServer)
        { throw new System.Exception("this method should be called only on server");  }

        TmpDmgTaken.Value = TmpDmgTaken.Value + damage;
        //throw new System.NotImplementedException();
    }

    public Enemy GetClosestEnemy()
    {
        if (!NetworkManager.Singleton.IsServer)
        { throw new System.Exception("this method should be called only on server"); }
        
        Enemy[] Enemies = GetComponentsInChildren<Enemy>();

        // TODO: sort them by distance
        var enemy = Enemies.Length != 0 ? Enemies[0] : null;

        return enemy;
    }
#endif

}
