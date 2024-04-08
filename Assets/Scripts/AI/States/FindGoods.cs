using System.Collections.Generic;
using System.Linq;

public class FindGoods : State
{
    public State pay;
    public State getGoods;
    public State exit;

    public override void OnStateEnter(AICore core) { }

    public override void ExecuteState(AICore core) { }

    public override State UpdateState(AICore core)
    {
        if (CanBuyMore(core.CustomerData.Money - core.CustomerData.goodsCost, core.CustomerData.Mood, core))
            return getGoods;
        else
            return core.CustomerData.goodsCost > 0 ? pay : exit;
            
    }

    public override void OnStateExit(AICore core)
    {
    }


    private bool CanBuyMore(int availableMoney, int mood, AICore core)
    {
        if (mood <= 0) return false;
        List<GoodsContainer> containers = core.AvailableGoodsContainers(availableMoney, mood).ToList();
        // IEnumerable<GoodsContainer> containers = ShopManager.current.Containers.Where(c => c.ItemMinimalPrice() <= availableMoney);
        return containers.Count() > 0;
    }


}
