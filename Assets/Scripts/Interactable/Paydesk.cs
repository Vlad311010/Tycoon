using Interfaces;
using UnityEngine;

public class Paydesk : MonoBehaviour, IInteractable
{
    public Transform InteractionPoint { get => interactionPoint; }
    public Transform LookAt { get => lookAt; }


    [SerializeField] Transform interactionPoint;
    [SerializeField] Transform lookAt;

    public void Interact(AICore core)
    {
        ShopManager.current.AddMoney(core.CustomerData.goodsCost);
    }
}
