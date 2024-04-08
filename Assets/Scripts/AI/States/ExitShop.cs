using UnityEngine;

public class ExitShop : State
{
    public override void OnStateEnter(AICore core)
    {
        AIGeneral.CreatePath(core.Agent, ShopManager.current.OutOfScreen.position);
    }

    public override void ExecuteState(AICore core) 
    {
        
    }


    public override State UpdateState(AICore core)
    {
        if (AIGeneral.AgentIsAtDestinationPoint(core.Agent)) 
        {
            core.Despawn();
            return this;
        }
        else
            return this;
    }

    public override void OnStateExit(AICore core)
    {
    }
}
