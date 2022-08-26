using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MissionTimer : NetworkBehaviour, IOnServerFixedUpdate
{
    [SerializeField]
    const float LengthOfTheMission = 600.0f;

    [SerializeField]
    NetworkVariable<float> TimeToEnd = new NetworkVariable<float>(LengthOfTheMission);

    public UpdateUIActions UIActions = new UpdateUIActions();

    public float GetTimeToEnd() => TimeToEnd.Value;

    private void Start()
    {
        UIActions.AddOnValueChangeDependency(TimeToEnd);
    }

    public void ServerFixedUpdate()
    {
        float newTime = TimeToEnd.Value - Time.deltaTime;

        if (newTime > 0.0f)
        {
            TimeToEnd.Value = newTime;
        }
        else
        {
            Debug.Log("You won. Congrats."); //TODO: add end screen
        }

    }
}
