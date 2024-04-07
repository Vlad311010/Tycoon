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
        if (CanBuyMore(core.CustomerData.Money - core.CustomerData.goodsCost, core.CustomerData.Mood))
            return getGoods;
        else
            return core.CustomerData.goodsCost > 0 ? pay : exit;
            
    }


    private bool CanBuyMore(int availableMoney, int mood)
    {
        if (mood <= 0) return false;

        IEnumerable<GoodsContainer> containers = ShopManager.current.Containers.Where(c => c.ItemMinimalPrice() <= availableMoney);
        return containers.Count() > 0;
    }


}
