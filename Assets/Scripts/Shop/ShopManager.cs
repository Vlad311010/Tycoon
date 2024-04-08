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
    private int improvmentTools;
    private int cleaningTools;

    private void Awake()
    {
        current = this;
        money = PersistentDataManager.GameData.money;
        
        GameEvents.current.onGoodsContainerPlaced += AddGoodsContainer;
        GameEvents.current.onGoodsContainerRemoved += RemoveGoodsContainer;
    }

    private void Start()
    {
        containers = new List<GoodsContainer>();
        containers.AddRange(GameObject.FindObjectsOfType<GoodsContainer>());

        GameEvents.current.MoneyAmountChange(money, 0);
    }

    private void AddGoodsContainer(GoodsContainer container)
    {
        containers.Add(container);
        Save();
    }

    private void RemoveGoodsContainer(GoodsContainer container)
    {
        containers.Remove(container);
        Save();
    }

    private bool HaveEnoughMoney(int required)
    {
        return money >= required;
    }

    public void AddMoney(int amount)
    {
        money = System.Math.Clamp(money + amount, 0, int.MaxValue);
        GameEvents.current.CustomerPays(money);
        GameEvents.current.MoneyAmountChange(money, amount);
    }

    public void Save()
    {
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
