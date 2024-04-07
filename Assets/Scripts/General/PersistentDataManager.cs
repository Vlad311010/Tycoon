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
            IContainPersistentData[] persistentDataContainers = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<IContainPersistentData>().ToArray();
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

    private static PersistentData CreateInitialData()
    {
        gameData = new PersistentData();
        Debug.Log("Persistent Data Initiated");
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
