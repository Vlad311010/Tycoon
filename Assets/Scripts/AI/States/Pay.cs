using UnityEngine;

public class Pay : State
{
    public State exit;

    private bool atPaymentdesk = false;
    private bool paymentDone = false;
    private bool interactionIsActive = false;
    private Paydesk paydesk;
    
    public override void OnStateEnter(AICore core)
    {
        atPaymentdesk = false;
        paymentDone = false;
        interactionIsActive = false;
        paydesk = ShopManager.current.Paydesk;

        AIGeneral.CreatePath(core.Agent, paydesk.InteractionPoint.position);
    }

    public override void ExecuteState(AICore core)
    {
        atPaymentdesk = AIGeneral.AgentIsAtDestinationPoint(core.Agent);
        if (atPaymentdesk)
        {
            core.lookAt = paydesk.LookAt;
        }

        if (!interactionIsActive && atPaymentdesk && !paymentDone)
        {
            interactionIsActive = true;
            core.Interact(2, paydesk, OnPayment);
        }
    }

    private void OnPayment(AICore core)
    {
        paymentDone = true;
    }


    public override State UpdateState(AICore core)
    {
        if (paymentDone)
            return exit;
        else 
            return this;
    }
}
