#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[ExecuteAlways]
public class UniqueID : MonoBehaviour
{
    // Serialize/saves the unique ID
    // but hide it in the Inspector -> we don't want to edit it manually
    // uint allows around 4 Million IDs that should be enough for most cases ;)
    private uint _id;
    private static List<uint> usedIds;

    // Public read-only accessor
    public uint ID => _id;

#if UNITY_EDITOR
    // Due to ExecuteAllways this is called once the component is created
    private void Awake()
    {
        if (!Application.isPlaying && _id == 0)
        {
            _id = GetFreeID();
            usedIDs.Add(_id);

            EditorUtility.SetDirty(this);
        }
    }
#endif

    // For runtime e.g. when spawning prefabs
    // in that case Start is delayed so you can right after instantiating also assign a specific ID
    private void Start()
    {
        if (Application.isPlaying && _id == 0) _id = GetFreeID();
    }

    // Allows to set a specific ID e.g. when instantiating on runtime
    public void SetSpecificID(uint id)
    {
        if (Application.isPlaying) _id = id;
        else Debug.LogWarning("Only use in play mode!", this);
    }

    // Stores all already used IDs
    private readonly static HashSet<uint> usedIDs = new HashSet<uint>();

    private static readonly System.Random random = new System.Random();

    // Get a random uint
    private static uint RandomUInt()
    {
        uint thirtyBits = (uint)random.Next(1 << 30);
        uint twoBits = (uint)random.Next(1 << 2);
        return (thirtyBits << 2) | twoBits;
    }

    // This is called ONCE when the project is opened in the editor and ONCE when the app is started
    // See https://docs.unity3d.com/ScriptReference/InitializeOnLoadMethodAttribute.html
    [InitializeOnLoadMethod]
    [RuntimeInitializeOnLoadMethod]
    private static void InitUsedIDs()
    {
        // Find all instances of this class in the scene
        var instances = FindObjectsOfType<UniqueID>(true);
        if (usedIds == null)
            usedIds = new List<uint>();

        foreach (var instance in instances.Where(i => i._id != 0))
        {
            usedIds.Add(instance._id);
        }

        foreach (var instance in instances.Where(i => i._id == 0))
        {
            instance._id = GetFreeID();

            #if UNITY_EDITOR
                EditorUtility.SetDirty(instance);
            #endif

            usedIDs.Add(instance._id);
        }
    }

    private static uint GetFreeID()
    {
        uint id = 0;

        do
        {
            id = RandomUInt();
        }
        while (id == 0 || usedIDs.Contains(id));

        return id;
    }
}