using UnityEngine;
using System.IO;
using Structs;
using Interfaces;
using System.Linq;
using System.Threading.Tasks;

public class PersistentDataManager
{
    public static PersistentData GameData { get => gameData == null ? Load() : gameData; set => gameData = value; }

    private static PersistentData gameData;

    private static string path = Application.persistentDataPath + "/GameData.json";


    public static void Save()
    {
        string dataJson = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(path, dataJson);
    }
    public static PersistentData Load()
    {
        if (File.Exists(path))
        {
            gameData = JsonUtility.FromJson<PersistentData>(File.ReadAllText(path));
            Debug.Assert(gameData != null, "Failed to load data");
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
        }
        else // create data but keep settins data
        {
            gameData = JsonUtility.FromJson<PersistentData>(File.ReadAllText(path));
            gameData = new PersistentData(gameData.settings);
        }
        Save();

        return gameData;
    }

}
