using UnityEngine;
using System.IO;
using Structs;
using Interfaces;
using System.Linq;

public class PersistentDataManager
{
    public static PersistentData GameData { get => gameData == null ? Load() : gameData; set => gameData = value; }

    private static PersistentData gameData;

    private static string path = Application.persistentDataPath + "/GameData.json";


    public static void Save()
    {
        string dataJson = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(path, dataJson);
        // Debug.Log("Persistent Data Saved");
    }

    public static PersistentData Load()
    {
        Debug.Log(GameDataAvailable());
        if (File.Exists(path))
        {
            gameData = JsonUtility.FromJson<PersistentData>(File.ReadAllText(path));
            Debug.Assert(gameData != null, "Failed to load data");
            // Debug.Log("Persistent Data Loaded");
            IContainPersistentData[] persistentDataContainers = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<IContainPersistentData>().OrderBy(o => o.LoadOrder).ToArray();
            foreach (IContainPersistentData persistentDataContainer in persistentDataContainers) 
            {
                persistentDataContainer.Load();
            }

            return gameData;
        }
        else
        {
            return CreateInitialData();
        }
    }

    public static bool GameDataAvailable()
    {
        if (!File.Exists(path)) return false;

        gameData = JsonUtility.FromJson<PersistentData>(File.ReadAllText(path));
        return gameData.grids.Count > 0;
    }

    public static PersistentData CreateInitialData()
    {
        if (!File.Exists(path)) // create data first time
        {
            gameData = new PersistentData();
            // Debug.Log("Persistent Data Initiated");
        }
        else // create data but keep settins data
        {
            gameData = JsonUtility.FromJson<PersistentData>(File.ReadAllText(path));
            gameData = new PersistentData(gameData.settings);
            // Debug.Log("Persistent Data Reinitiated");
        }
        Save();

        return gameData;
    }


    
    private static void RestoreObjects(PersistentData gameData)
    {
        for (int i = 0; i < gameData.containers.Count; i++)
        {
            gameData.containers[i].Load();
        }

    }

}
