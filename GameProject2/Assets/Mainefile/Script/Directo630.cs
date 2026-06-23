using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Directo630 : MonoBehaviour
{
    public Text scoreText;
    private float timer = 0f;
    public int cost = 0;
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            cost += 1;
            timer = 0f;
            scoreText.text = "Cost: " + cost.ToString();
        }
    }
    public void SpendScore(int amount)
    {
        cost -= amount;
        scoreText.text = "Cost: " + cost.ToString();
    }
}


