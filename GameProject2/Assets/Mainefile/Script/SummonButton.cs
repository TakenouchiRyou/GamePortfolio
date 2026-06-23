using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonButton : MonoBehaviour
{
    public PlayerGenerator gen;
    public Directo630 costManager;
    public Button summonButton;

    void Update()
    {
        if (costManager.cost < gen.SummonCost)
        {
            summonButton.interactable = false;
        }
        else
        {
            summonButton.interactable = true;
        }
    }
}
