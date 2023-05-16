using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

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
            AudioManager.Instance.PlaySentenceLoclaly("accessDenied_r actionPanelIsDisabled_r");
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

    [SerializeField] protected GameObject EnemyInfoPanelPrefab;
    [SerializeField] GameObject DistanceMeterIcon;
    RectTransform DistanceMeter;
    private Line UILine;
    TextMeshProUGUI Distance_text;
    Image Distance_image;

    private void Awake()
    {
        Distance_text = transform.Find("Distance").GetComponentInChildren<TextMeshProUGUI>();
        Distance_image = transform.Find("Distance").GetComponent<Image>();
    }

    public void Initialise(T enemy)
    {
        Enemy = enemy;
        UILine = GetComponentInChildren<Line>();
        DistanceMeter = transform.parent.GetComponent<EnemyIconSpawner>().GetDistanceMeter().GetComponent<RectTransform>();

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
            Enemy.UIActions.RemoveAction(UpdateUIAction);

        if (DistanceMeterIcon != null)
            Destroy(DistanceMeterIcon);
    }

    public void SpawnInfoPanel()
    {
        var actionPanel = GameObject.Find("ActionPanel");
        if (actionPanel == null)
        {
            // audio message
            AudioManager.Instance.PlaySentenceLoclaly("accessDenied_r actionPanelIsDisabled_r");
            return;
        }

        GameObject _go = Instantiate(EnemyInfoPanelPrefab, parent: actionPanel.transform);
    }

    protected virtual string GetEnemyNemeLine() => $"{Enemy.GetName()}";
    protected virtual string GetEnemyActionDescriptionLine() => $"{Enemy.NextActionDescription} in {Enemy.NextActionTime.ToString("0.0")}s";
    protected override void UpdateUI()
    {
        if (Enemy != null)
        {
            try
            {
                // set HP and Shield
                var HP = transform.Find("HP").GetComponentInChildren<TextMeshProUGUI>();
                HP.text = Enemy.HP.ToString();
                var Shield = transform.Find("Shield").GetComponentInChildren<TextMeshProUGUI>();
                Shield.text = Enemy.EnergyShield.ToString();
            }
            catch (Exception)
            {
                Debug.Log($"No HP or Shield in icon of enemy \"{Enemy.GetName()}\"");
            }


            // set description
            string line1 = GetEnemyNemeLine();
            string line2 = GetEnemyActionDescriptionLine();
            var description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
            description.text = line1 + '\n' + line2;

            try
            {
                // set DistanceMeterIcon
                if (DistanceMeterIcon.transform.parent != DistanceMeter.transform)
                    DistanceMeterIcon.transform.SetParent(DistanceMeter);

                float offset = DistanceMeter.sizeDelta.y / 2;
                var distance = Enemy.Distance / (int)RangeEnum.Far;

                DistanceMeterIcon.transform.localPosition = new Vector3(0, offset - 2 * offset * distance, 0);

                // set DistanceMeterIcon color
                DistanceMeterIcon.GetComponent<Image>().color = ProjectColors.GetColorForDistance(Enemy.Distance);
                // set Distance

                Distance_text.text = Enemy.Distance.ToString("0.");
                Distance_image.color = ProjectColors.GetColorForDistance(Enemy.Distance);

                // set line
                UILine.UpdateUI(DistanceMeterIcon.transform, transform);
            }
            catch (Exception)
            {
                Debug.Log($"No DistanceMeterIcon setup in icon of enemy \"{Enemy.GetName()}\"");
            }
        }
    }
}
