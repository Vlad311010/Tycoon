using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public Paydesk Paydesk { get => paydesk; }
    public Transform Entrance { get => entrance; }
    public Transform OutOfScreen { get => outOfScreen; }
    public List<GoodsContainer> Containers { get => containers; }
    
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
        containers = new List<GoodsContainer>();
        containers.AddRange(GameObject.FindObjectsOfType<GoodsContainer>());

        GameEvents.current.onGoodsContainerPlaced += AddGoodsContainer;
        // GameEvents.current.onMoneyAmountChange += OnMoneyAmountChange;
    }

    private void Start()
    {
        GameEvents.current.MoneyAmountChange(money, 0);
    }

    private void AddGoodsContainer(GoodsContainer container)
    {
        containers.Add(container);
    }

    private void RemoveGoodsContainer(GoodsContainer container)
    {
        containers.Remove(container);
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
}
