using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int highscore;
    public string nickname;

    public GameData(MainCircuit circuit)
    {
        highscore = circuit.highscore;
        nickname = circuit.nickname;
    }
}
