using Interfaces;
using Structs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopManager : MonoBehaviour, IContainPersistentData
{
    public Paydesk Paydesk { get => paydesk; }
    public Transform Entrance { get => entrance; }
    public Transform OutOfScreen { get => outOfScreen; }
    public List<GoodsContainer> Containers { get => containers; }

    public uint LoadOrder => 1;

    public static ShopManager current;

    [SerializeField] Paydesk paydesk;
    [SerializeField] Transform entrance;
    [SerializeField] Transform outOfScreen;
    
    List<GoodsContainer> containers;

    private int money;
    private int cleaningTools;

    private void Awake()
    {
        current = this;
        PersistentDataManager.Load();
        money = PersistentDataManager.GameData.money;
        cleaningTools = PersistentDataManager.GameData.cleaningTools;


        GameEvents.current.onGoodsContainerPlaced += AddGoodsContainer;
        GameEvents.current.onGoodsContainerRemoved += RemoveGoodsContainer;
    }

    private void OnDestroy()
    {
        GameEvents.current.onGoodsContainerPlaced -= AddGoodsContainer;
        GameEvents.current.onGoodsContainerRemoved -= RemoveGoodsContainer;
    }

    private void Start()
    {
        containers = new List<GoodsContainer>();
        containers.AddRange(GameObject.FindObjectsOfType<GoodsContainer>());

        GameEvents.current.MoneyAmountChange(money, 0);
        GameEvents.current.CleaningToolUsage(cleaningTools);
    }

    private void AddGoodsContainer(GoodsContainer container)
    {
        ShopManager.current.AddMoney(-container.ObjectData.price);
        containers.Add(container);
        Save();
    }

    private void RemoveGoodsContainer(GoodsContainer container)
    {
        ShopManager.current.AddMoney(container.ObjectData.price);
        containers.Remove(container);
        Save();
    }

    public bool HaveEnoughMoney(int required)
    {
        return money >= required;
    }

    public void AddMoney(int amount)
    {
        money = System.Math.Clamp(money + amount, 0, int.MaxValue);
        GameEvents.current.CustomerPays(money);
        GameEvents.current.MoneyAmountChange(money, amount);
        Save();
    }

    public void UseCleaninigTool()
    {
        cleaningTools--;
        GameEvents.current.CleaningToolUsage(cleaningTools);
        Save();
    }

    public bool PossessesCleaningTool()
    {
        return cleaningTools > 0;
    }


    public void Save()
    {
        PersistentDataManager.GameData.money = money;
        PersistentDataManager.GameData.cleaningTools = cleaningTools;
        PersistentDataManager.GameData.containers = containers.Select(c => new GoodsContainerData(c)).ToList();
        PersistentDataManager.Save();
    }

    public void Load()
    {
        foreach (var conteinerSavedData in PersistentDataManager.GameData.containers)
        {
            conteinerSavedData.Load();
        }
    }
}
