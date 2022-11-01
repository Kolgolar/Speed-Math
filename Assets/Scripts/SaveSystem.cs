using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/savedata.lmao";
    public static void SaveData(MainCircuit mainCircuit)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Create);
        GameData data = new GameData(mainCircuit);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadData()
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);
            GameData data = formatter.Deserialize(stream) as GameData;
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" + savePath);
            return null;
        }
    }
}
