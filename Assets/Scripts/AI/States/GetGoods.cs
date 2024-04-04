using System.Collections.Generic;
using UnityEngine;

public class GetGoods : State
{
    public State FindGoods;

    private GoodsContainer target;
    private bool atDestinationPoint = false;
    private bool goodsTaken = false;

    [SerializeField] List<GoodsContainer> goodscontainers;


    public override void OnStateEnter(AICore core)
    {
        goodsTaken = false;
        atDestinationPoint = false;
        target = goodscontainers.RandomChoice();
        AIGeneral.CreatePath(core.Agent, target.transform.position);
    }

    public override void ExecuteState(AICore core)
    {
        atDestinationPoint = AIGeneral.AgentIsAtDestinationPoint(core.Agent);
        if (atDestinationPoint)
        {
            target.Interact(core);
            goodsTaken = true;
            // interact
        }

    }

    public override State UpdateState(AICore core)
    {
        if (goodsTaken)
            return FindGoods;
        else return this;
    }
}
