using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLogic : MonoBehaviour
{
    [SerializeField] private GameObject mainGameCamTarget;
    [SerializeField] private GameCamera gameCam;
    [SerializeField] private MainCircuit mainCircuit;

    private void OnEnable() 
    {
        GMButton.onClicked += onGameModeClicked;
    }

    private void OnDisable() 
    {
        GMButton.onClicked -= onGameModeClicked;
    }

    void Start()
    {
        
    }

    private void GoToMainCircuit()
    {
        
    }

    private void GoToMainMenu()
    {

    }

    private void GoToModeMenu()
    {

    }

    private void onGameModeClicked(int gamemode)
    {
        gameCam.MoveTo(mainGameCamTarget);
        mainCircuit.Gamemode = gamemode;
    }
    
}
