using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MissionTimer : NetworkBehaviour, IOnServerFixedUpdate, IRestart
{
    const float LengthOfTheMission = 300.0f;

    [SerializeField]
    NetworkVariable<float> TimeToEnd = new NetworkVariable<float>(LengthOfTheMission);
    public UpdateUIActions UIActions = new UpdateUIActions();

    public float GetTimeToEnd() => TimeToEnd.Value;

    private void Start()
    {
        UIActions.AddOnValueChangeDependency(TimeToEnd);

        ServerUpdater.Add(this.gameObject);
    }

    public void ServerFixedUpdate()
    {
        float newTime = TimeToEnd.Value - Time.deltaTime;

        if (newTime > 0.0f)
        { TimeToEnd.Value = newTime; }
        else
        {
            ServerUpdater.StopUpdating();
            GameObject.Find("SceneChanger").GetComponent<SceneChanger>().ChangeScene("EndScreen");
        }
    }

    public void Restart()
    {
        TimeToEnd.Value = LengthOfTheMission;
    }
}
