using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    public static void SaveGame(GameManager _gameManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/Game_Data";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(_gameManager);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadGame(GameManager _gameManager)
    {
        string path = Application.persistentDataPath + "/Game_Data";

        BinaryFormatter formatter = new BinaryFormatter();

        GameData data;

        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return data;
        }
        else
        {
            SaveGame(_gameManager);
            return null;
        }
    }
}