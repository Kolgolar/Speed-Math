using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveBulbs : MonoBehaviour
{
    Color on = new Color(255f, 0, 0);
    Color off = new Color(50f, 0, 0);
    
    private void OnEnable()
    {
        MainCircuit.onStatsChanged += UpdateLives;       
    }

    private void OnDisable()
    {
        MainCircuit.onStatsChanged -= UpdateLives;    
    }
    
    private void UpdateLives(int score, int lives)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Material[] materials = getBulb(i).GetChild(1).GetComponent<MeshRenderer>().materials;
            Light light = getBulb(i).GetChild(2).GetComponent<Light>();
            if (i < lives)
            {
                light.enabled = true;
                // materials[0].color = on;
            }
            else
            {
                light.enabled = false;
                // materials[0].color = off;
            }
        }
    }

    private Transform getBulb(int num)
    {
        return transform.GetChild(num);
    }
}
