using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screensaver : MonoBehaviour
{
    const float DurationTimeConst_max = 20.0f; // 20 s
    const float DurationTimeConst_min = 10.0f; // 10 s

    float TimeToCloseScreensaver = float.MaxValue; // time to close screensaver

    private void Start()
    {
        TimeToCloseScreensaver = Random.Range(DurationTimeConst_min, DurationTimeConst_max);
    }

    void FixedUpdate()
    {
        float newTime = TimeToCloseScreensaver - Time.deltaTime;
        if (newTime > 0.0f) { TimeToCloseScreensaver = newTime; }
        else { GameObject.Destroy(gameObject); }
    }
}
