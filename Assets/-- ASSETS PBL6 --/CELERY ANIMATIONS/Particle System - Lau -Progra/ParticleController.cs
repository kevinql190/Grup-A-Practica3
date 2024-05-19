using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    ParticleSystem _particleSystem;
    private bool _togglePlay;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (ShouldChangePlay())
        {
            ChangePlay();
            ChangeAngle();
            
        }
        ChangeStrength();
    }

    private void ChangePlay()
    {
        _togglePlay = !_togglePlay;
        if (_togglePlay)
        {
            Play();
        }
        else
        {
            Stop();
        }
    }
    private void ChangeAngle() 
    {
        var shapeModule = _particleSystem.shape;
       // shapeModule = Random.Range(0, 90);
    }
    private static bool ShouldChangePlay()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    private void Stop()
    {
        _particleSystem.Stop();
    }
    private void ChangeStrength()
    {
        var emision = _particleSystem.emission;
        emision.rateOverTime = Mathf.Sin(Time.time)*0.5f+0.5f;

        //No es pot fer, s'ha de guardar com una variable local
        // emision.GetBurst(0).count = 99;
        var burst = emision.GetBurst(0);
        burst.count = 99f;
        emision.SetBurst(0, burst);
    }

    private void Play()
    {
        _particleSystem.Play();
    }
}
