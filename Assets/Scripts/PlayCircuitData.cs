using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayCircuitData
{
    public int highscore;

    public PlayCircuitData(PlayCircuitData circuit)
    {
        highscore = circuit.highscore;
    }
}
