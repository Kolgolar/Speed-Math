using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsDisplayer : MonoBehaviour
{
    private TextMeshProUGUI ScreenText;
    
    private void OnEnable()
    {
        MainCircuit.onStatsChanged += UpdateStats;    
        MainCircuit.onGameOver += ShowGameOverScreen;    
    }

    private void OnDisable()
    {
        MainCircuit.onStatsChanged -= UpdateStats;
        MainCircuit.onGameOver -= ShowGameOverScreen;      
    }

    private void Start() 
    {
        ScreenText = GetComponent<TextMeshProUGUI>();
    }

    public void Clear()
    {
        ScreenText.text = ":)";
    }

    void UpdateStats(int score, int lives)
    {
        // ScreenText.text = string.Format("Score:\n{0}\nLives:\n{1}", score.ToString(), lives.ToString());
        ScreenText.text = string.Format("Score:\n{0}", score.ToString());
    }

    void ShowGameOverScreen(int score, int highscore)
    {
        ScreenText.text = string.Format("Score:\n{0}\nHighscore:\n{1}", score.ToString(), highscore.ToString());
    }
}
