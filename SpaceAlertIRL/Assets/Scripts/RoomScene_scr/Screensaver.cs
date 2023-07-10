using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screensaver : MonoBehaviour
{
    const float StartTimeConst = 20.0f; // 20 s

    float TimeToCloseScreensaver = StartTimeConst; // time to close screensaver

    void FixedUpdate()
    {
        float newTime = TimeToCloseScreensaver - Time.deltaTime;
        if (newTime > 0.0f) { TimeToCloseScreensaver = newTime; }
        else
        {
            GameObject.Destroy(gameObject);
        }
    }
}
