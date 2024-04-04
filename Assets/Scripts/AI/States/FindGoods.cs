using UnityEngine;

public class FindGoods : State
{
    public State Pay;
    public State GetGoods;

    public override void OnStateEnter(AICore core)
    {
    }

    public override void ExecuteState(AICore core)
    {
        
    }

    public override State UpdateState(AICore core)
    {
        if (core.CustomerData.money <= 0)
            return Pay;
        else
            return GetGoods;
    }

    
}
