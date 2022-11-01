using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveGameData(PlayCircuit playCircuit)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savedata.lmao";
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayCircuitData pcData = new PlayCircuitData(playCircuit);
    }
}
