using UnityEngine;

public class EnterShop : State
{
    public State FindGoods;


    public override void OnStateEnter(AICore core)
    {
        AIGeneral.CreatePath(core.Agent, ShopManager.current.Entrance.position);
    }

    public override void ExecuteState(AICore core) { }


    public override State UpdateState(AICore core)
    {
        if (AIGeneral.AgentIsAtDestinationPoint(core.Agent, 0.1f))
            return FindGoods;
        else
            return this;
    }

    public override void OnStateExit(AICore core)
    {
    }
}
