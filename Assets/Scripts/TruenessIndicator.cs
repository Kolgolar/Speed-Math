using UnityEngine;

public class TruenessIndicator : MonoBehaviour
{
    private enum States { Off, TurningOff, TurningOn, On};
    private States state;
    private const float lightTogglingTime = 0.1f;
    private const float lightMaxIntensity = 1f;
    private float lightIntensityDelta = lightMaxIntensity / lightTogglingTime;
    private int lightIntensityMult = -1;
    private const float fullBrightnessTime = 0.25f;
    private float fullBrightnessTimeLeft = fullBrightnessTime;
    
    private void OnEnable()
    {
        MainCircuit.onAnswerRecieved += LightUp;
    }

    private void OnDisable()
    {
        MainCircuit.onAnswerRecieved -= LightUp;
    }

    private void Start() 
    {
        LightDown();    
    }

    private void Update()
    {
        if (state == States.On)
            TimerUpdate();
        else if (state != States.Off)
            LightUpdate();
    }

    private void LightUpdate()
    {
        var light = GetComponent<Light>();
        light.intensity += Time.deltaTime * lightIntensityDelta * lightIntensityMult;
        // light.intensity = Math.Clamp(light.intensity, 0, lightMaxIntensity);
        if (state == States.TurningOn)
        {
            if (light.intensity >= lightMaxIntensity)
                state = States.On;
        }
        else if (state == States.TurningOff)
        {
            if (light.intensity <= 0)
            {                
                state = States.Off;
                fullBrightnessTimeLeft = fullBrightnessTime;
            }

        }
    }

    private void TimerUpdate()
    {
        fullBrightnessTimeLeft -= Time.deltaTime;
        if (fullBrightnessTimeLeft <= 0)
        {
            LightDown();
        }
    }

    private void LightUp(bool isRight)
    {
        state = States.TurningOn;
        var lightColor = Color.red;
        if (isRight)
            lightColor = Color.green;
        var light = GetComponent<Light>();
        lightIntensityMult = 1;
        light.color = lightColor;
    }

    private void LightDown()
    {
        state = States.TurningOff;
        lightIntensityMult = -1;
    }
}
