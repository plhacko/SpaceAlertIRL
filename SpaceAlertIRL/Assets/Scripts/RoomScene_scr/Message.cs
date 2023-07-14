using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class Message : MonoBehaviour
{
    [SerializeField] float VisibleTime = 3.0f;

    TextMeshProUGUI Text;
    public void Initialise(string message)
    {
        Text = GetComponentInChildren<TextMeshProUGUI>();
        Text.text = message;

        StartCoroutine(WaitAndFade());
    }

    IEnumerator WaitAndFade()
    {
        yield return new WaitForSeconds(VisibleTime);
        Color c = Text.color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            c.a = alpha;
            Text.color = c;
            yield return new WaitForSeconds(.1f);
        }

        Destroy(gameObject);
    }


}
