using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

abstract public class Icon : MonoBehaviour
{
    protected Action UpdateUIAction;

    protected abstract void UpdateUI();
    abstract protected void OnDisable();
}

abstract public class AmenityIcon<T> : Icon where T : Amenity
{
    protected T Amenity;

    [SerializeField]
    protected GameObject ActionPanelPrefab;

    public void Initialise(T amenity)
    {
        Amenity = amenity;
        UpdateUIAction = UpdateUI;
        UpdateUIAction();
        Amenity.UIActions.AddAction(UpdateUIAction);
    }

    override protected void OnDisable()
    {
        if (Amenity != null)
        {
            Amenity.UIActions.RemoveAction(UpdateUIAction);
        }
    }

    public void SpawnActionPanel()
    {
        var actionPanel = GameObject.Find("ActionPanel");
        if (actionPanel == null)
        {
            // audio message
            GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySentenceLoclaly("accessDenied_r actionPanelIsDisabled_r");
            return;
        }
        var actionPanelSpawner = actionPanel.GetComponent<ActionPanelSpawner>();

        actionPanelSpawner.ResetSelf();
        GameObject _go = Instantiate(ActionPanelPrefab, parent: actionPanel.transform);
        _go.GetComponent<AmenityActionPanel<T>>().Initialise(Amenity);
    }
}

abstract public class EnemyIcon<T> : Icon where T : Enemy
{
    protected T Enemy;

    [SerializeField]
    protected GameObject EnemyInfoPanelPrefab;

    public void Initialise(T enemy)
    {
        Enemy = enemy;
        UpdateUIAction = DestroyThisIconWithEnemyDeath;
        UpdateUIAction += UpdateUI;
        UpdateUIAction();
        Enemy.UIActions.AddAction(UpdateUIAction);
    }

    private void DestroyThisIconWithEnemyDeath()
    {
        if (Enemy == null)
        { Destroy(gameObject); }
    }

    override protected void OnDisable()
    {
        if (Enemy != null)
        {
            Enemy.UIActions.RemoveAction(UpdateUIAction);
        }
    }

    public void SpawnInfoPanel()
    {
        var actionPanel = GameObject.Find("ActionPanel");
        if (actionPanel == null)
        {
            // audio message
            GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySentenceLoclaly("accessDenied_r actionPanelIsDisabled_r");
            return;
        }

        GameObject _go = Instantiate(EnemyInfoPanelPrefab, parent: actionPanel.transform);
    }


    protected virtual string GetEnemyNemeHpEsLine() => $"{Enemy.GetName()}, HP : {Enemy.HP}/{Enemy.MaxHP}, ES : {Enemy.EnergyShield}/{Enemy.MaxEnergyShield}";
    protected virtual string GetEnemyActionDescriptionLine() => $"{Enemy.NextActionDescription} in {Enemy.NextActionTime.ToString("0.00")}";
    protected virtual string GetEnemyDistanceLine() => $"Distance : {Enemy.Distance.ToString("0.00")}";
    protected override void UpdateUI()
    {
        if (Enemy != null)
        {
            string line1 = GetEnemyNemeHpEsLine();
            string line2 = GetEnemyActionDescriptionLine();
            string line3 = GetEnemyDistanceLine();
            GetComponentInChildren<TextMeshProUGUI>().text = line1 + '\n' + line2 + '\n' + line3;
        }
        else
        { Debug.Log("Missing Icon"); } // TODO: smazat else
    }
}
