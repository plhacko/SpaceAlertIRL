using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleProgressBar : MonoBehaviour
{
    // UI
    [SerializeField] GameObject bubble_full_prefab;
    [SerializeField] GameObject bubble_empty_prefab;

    List<GameObject> Bubbles = new List<GameObject>();

    int currentAmount = -1;
    int currentMaxAmount = -1;

    public void Clean()
    {
        // reset self
        foreach (GameObject child in Bubbles)
        {
            GameObject.Destroy(child.transform.gameObject);
        }
        Bubbles = new List<GameObject>();
    }

    public void UpdateUI(int amount, int maxAmount)
    {
        if (currentAmount == amount && currentMaxAmount == maxAmount)
            return;

        Clean();

        // spawn bubbles
        for (int i = 0; i < amount; i++)
        {
            var go = Instantiate(bubble_full_prefab, parent: transform);
            Bubbles.Add(go);
        }
        for (int j = amount; j < maxAmount; j++)
        {
            var go = Instantiate(bubble_empty_prefab, parent: transform);
            Bubbles.Add(go);
        }

        currentAmount = amount;
        currentMaxAmount = maxAmount;
    }
}
