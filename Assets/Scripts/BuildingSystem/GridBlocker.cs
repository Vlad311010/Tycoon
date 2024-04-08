using Interfaces;
using Structs;
using System.Linq;
using UnityEngine;

public class GridBlocker : MonoBehaviour, IClickable, IContainPersistentData
{
    public int ID => id;
    public BuildingGrid Grid { get => grid; }

    [SerializeField] BuildingGrid grid;
    [SerializeField][TextArea] string popupWindowText;
    [SerializeField] int id;

    public bool blocked = true;

    public uint LoadOrder { get => 0; }

    private void Awake()
    {
        grid.gridId = id;
        // init save data
        GridData gridData = new Structs.GridData(this);
        if (!PersistentDataManager.GameData.grids.Contains(gridData))
        {
            PersistentDataManager.GameData.grids.Add(gridData);
            PersistentDataManager.Save();
            if (!blocked)
            {
                Remove();
            }
        }
    }

    private void OnLoad()
    {

    }

    public void Remove()
    {
        blocked = false;
        grid.gameObject.SetActive(true);
        gameObject.SetActive(false);
        Save();
    }

    public void OnClick()
    {
        GameEvents.current.PopupWindowCall(popupWindowText, () => Remove());
    }

    public void Save()
    {
        Debug.Log("Save");

        GridData gridData = new Structs.GridData(this);
        if (PersistentDataManager.GameData.grids.Contains(gridData))
        {
            PersistentDataManager.GameData.grids.Remove(gridData);
        }
        PersistentDataManager.GameData.grids.Add(gridData);
        PersistentDataManager.Save();
    }

    public void Load()
    {
        Debug.Log("Load");
        // GridData? gridData = new Structs.GridData(this);
        GridData? gridData = PersistentDataManager.GameData.grids.Where(g => g.id == this.id).SingleOrDefault();

        if (!gridData.Value.blocked)
        {
            Remove();
        }
    }
}
