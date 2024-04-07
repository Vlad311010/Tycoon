using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Structs;

public class PersistentDataManager : MonoBehaviour
{
    public static PersistentData GameData { get => gameData == null ? Load() : gameData; set => gameData = value; }

    private static PersistentData gameData;
    
    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/GameData.dat");
        bf.Serialize(file, gameData);
        file.Close();
        
        Debug.Log("Persistent Data Saved");
    }

    public static PersistentData Load()
    {
        if (File.Exists(Application.persistentDataPath + "/GameData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + "/GameData.dat", FileMode.Open);
            PersistentData data = bf.Deserialize(fs) as PersistentData;
            fs.Close();

            Debug.Assert(data != null, "Failed to load data");
            Debug.Log("Persistent Data Loaded");
            gameData = data;
            return data;
        }
        else
        {
            return CreateInitialData();
        }
    }

    private static PersistentData CreateInitialData()
    {
        Debug.Log("Persistent Data Initialized");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/GameData.dat");

        PersistentData data = new PersistentData();
        bf.Serialize(file, data);
        file.Close();

        return data;
    }

}
