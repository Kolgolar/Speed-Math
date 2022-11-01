using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsDisplayer : MonoBehaviour
{
    private TextMeshProUGUI ScreenText;
    
    private void OnEnable()
    {
        PlayCircuit.onStatsChanged += UpdateStats;    
    }

    private void OnDisable()
    {
        PlayCircuit.onStatsChanged -= UpdateStats;    
    }

    private void Start() 
    {
        ScreenText = GetComponent<TextMeshProUGUI>();
    }
    void UpdateStats(int score, int lives)
    {
        ScreenText.text = string.Format("Score:\n{0}\nLives:\n{1}", score.ToString(), lives.ToString());
    }
}
