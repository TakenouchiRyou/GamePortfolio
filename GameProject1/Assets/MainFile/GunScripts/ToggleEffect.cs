using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ToggleEffect : MonoBehaviour
{
    bool flg = false;

    public void ToggleEffects()
    {
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();

        foreach (var p in particles)
        {
            if (flg)
                p.Stop();
            else
                p.Play();
        }

        flg = !flg;
    }
}
