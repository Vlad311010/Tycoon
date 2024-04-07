using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Structs;

public class PersistentDataManager : MonoBehaviour
{
    public static PersistentData GameData { get => gameData == null ? Load() : gameData; set => gameData = value; }

    private static PersistentData gameData;

    private static string path = Application.persistentDataPath + "/GameData.json";


    public static void Save()
    {
        string dataJson = JsonUtility.ToJson(gameData);
        File.WriteAllText(path, dataJson);
        Debug.Log("Persistent Data Saved");
    }

    public static PersistentData Load()
    {
        if (File.Exists(path))
        {
            gameData = JsonUtility.FromJson<PersistentData>(File.ReadAllText(path));
            Debug.Assert(gameData != null, "Failed to load data");
            Debug.Log("Persistent Data Loaded");
            return gameData;
        }
        else
        {
            return CreateInitialData();
        }
    }

    private static PersistentData CreateInitialData()
    {

        gameData = new PersistentData();
        Save();

        return gameData;
    }

}
