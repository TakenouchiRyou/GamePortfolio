using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineAudioPlayer : MonoBehaviour
{
    AudioSource _audio;
    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _audio.Play();        
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            _audio.Stop();
        }
    }

}
