#define SERVER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Zone : NetworkBehaviour
{
    public UpdateUIActions UIActions = new UpdateUIActions();

    const int MaxHPConst = 5;

    [SerializeField]
    private NetworkVariable<int> _HP = new NetworkVariable<int>(MaxHPConst);

    public int HP { get => _HP.Value; }
    public int MaxHP { get => MaxHPConst; }

    [SerializeField]
    protected List<Enemy> EnemyList = new List<Enemy>();
    public IList<Enemy> GetEnemyList() => EnemyList.AsReadOnly();

    public void AddEnemy(Enemy e) { EnemyList.Add(e); }

    private void Start()
    {
        UIActions.AddOnValueChangeDependency(_HP);
    }

#if (SERVER)
    public void TakeDmage(int damage, Enemy enemy)
    {
        if (!NetworkManager.Singleton.IsServer)
        { throw new System.Exception("this method should be called only on server"); }

        // reduce damage by shiealds
        // TODO:

        // do mamage to ship
        int _hp = _HP.Value - damage;
        if (_hp > 0) { _HP.Value = _hp; }
        else { _HP.Value = 0; Die(); }
    }

    private void Die()
    {
        throw new System.NotImplementedException();
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
