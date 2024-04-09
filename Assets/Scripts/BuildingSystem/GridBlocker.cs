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
        LoadGameData();
    }

    public void LoadGameData()
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
        else
        {
            int idx = PersistentDataManager.GameData.grids.IndexOf(gridData);
            blocked = PersistentDataManager.GameData.grids[idx].blocked;
            if (!blocked)
            {
                Remove();
            }
        }
    }

    public void Remove()
    {
        blocked = false;
        grid.gameObject.SetActive(true);
        gameObject.SetActive(false);
        Save();
    }

    public void OnLeftClick()
    {
        Debug.Log("Window popup Open");
        GameEvents.current.PopupWindowCall(popupWindowText, true, true, () => Remove());
    }

    public void OnRightClick()
    {
        // show object info
    }
    
    public void Save()
    {

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
        GridData? gridData = PersistentDataManager.GameData.grids.Where(g => g.id == this.id).SingleOrDefault();

        if (!gridData.Value.blocked)
        {
            Remove();
        }
    }
}
