using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GetGoods : State
{
    public State findGoods;


    private GoodsContainer target;
    private bool atDestinationPoint = false;
    private bool goodsTaken = false;
    private bool interactionIsActive = false;

    private AICore aiCore;

    private void OnDestroy()
    {
        GameEvents.current.onGoodsContainerRemoved -= Reselect;
    }


    private void Reselect(GoodsContainer _)
    {
        aiCore.ForceStateChange(findGoods);
    }

    public override void OnStateEnter(AICore core)
    {
        GameEvents.current.onGoodsContainerRemoved += Reselect;

        aiCore = core;
        goodsTaken = false;
        atDestinationPoint = false;
        interactionIsActive = false;
        target = SelectContainer(core.CustomerData.Money - core.CustomerData.goodsCost, core);
        
        AIGeneral.CreatePath(core.Agent, target.RandomInteractionPoint());
    }

    public override void ExecuteState(AICore core)
    {
        atDestinationPoint = AIGeneral.AgentIsAtDestinationPoint(core.Agent);
        if (!interactionIsActive && atDestinationPoint)
        {
            interactionIsActive = true;
            core.lookAt = target.LookAt;
            core.Interact(target.interacionTime.RandomRange(), target, OnInteraction);
        }
    }

    private void OnInteraction(AICore core)
    {
        goodsTaken = true;
    }

    public override State UpdateState(AICore core)
    {
        if (goodsTaken)
            return findGoods;
        else    
            return this;
    }

    public override void OnStateExit(AICore core)
    {
        GameEvents.current.onGoodsContainerRemoved -= Reselect;
    }

    private GoodsContainer SelectContainer(int availableMoney, AICore core)
    {
        List<GoodsContainer> containers = core.AvailableGoodsContainers(availableMoney).ToList();

        if (containers.Count > 0)
            return containers.RandomChoice();
        else 
            return null;
    } 

    private void OnDrawGizmos()
    {
        if (target == null) return;

        Gizmos.color = atDestinationPoint ? Color.green : Color.red;
        Gizmos.DrawCube(target.LookAt.position, Vector3.one * 0.5f); 
    }
}
