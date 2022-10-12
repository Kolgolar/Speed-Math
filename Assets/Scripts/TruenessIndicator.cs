using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TruenessIndicator : MonoBehaviour
{
    private const float lightMaxIntensity = 20.0f;
    private const float lightIntensityDelta = 200.0f;
    private int lightIntensityMult = -1;
    private bool isTimerOn = false;
    private const float timerStartingTime = 0.25f;
    private float timerTimeLeft = timerStartingTime;
    
    private void OnEnable()
    {
        PlayCircuit.onAnswerRecieved += LightUp;
    }

    private void OnDisable()
    {
        PlayCircuit.onAnswerRecieved -= LightUp;
    }

    private void Start() 
    {
        LightDown();    
    }

    private void Update()
    {
        TimerUpdate();
        LightUpdate();

    }

    private void LightUpdate()
    {
        var light = GetComponent<Light>();
        light.intensity += Time.deltaTime * lightIntensityDelta * lightIntensityMult;
        light.intensity = Math.Clamp(light.intensity, 0, lightMaxIntensity);
    }

    private void TimerUpdate()
    {
        if (isTimerOn)
        {
            timerTimeLeft -= Time.deltaTime;
            if (timerTimeLeft <= 0)
            {
                isTimerOn = false;
                LightDown();
            }
        }
    }

    private void LightUp(bool isRight)
    {
        var lightColor = Color.red;
        if (isRight)
            lightColor = Color.green;
        var light = GetComponent<Light>();
        lightIntensityMult = 1;
        light.color = lightColor;
        isTimerOn = true;
        timerTimeLeft = timerStartingTime;
    }

    private void LightDown()
    {
        lightIntensityMult = -1;
    }
}
