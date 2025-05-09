using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class saveSystem
{
    public static void SaveData(gameData stats)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/saves.game";
        FileStream file = new FileStream(path, FileMode.Create);
        playerData data = new playerData(stats);
        formatter.Serialize(file, data);
        file.Close();
    }

    public static playerData LoadData()
    {
        string path = Application.persistentDataPath + "/saves.game";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = new FileStream(path, FileMode.Open);
            playerData data = formatter.Deserialize(file) as playerData;
            file.Close();
            return data; 
        }
        else
        {
            Debug.LogError("Save data isnt found in" + path);
            return null; 
        }
    }
}
