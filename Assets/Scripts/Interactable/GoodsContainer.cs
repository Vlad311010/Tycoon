using Interfaces;
using UnityEngine;

public class GoodsContainer : MonoBehaviour, IInteractable
{
    public Transform LookAt { get => lookAt; }

    [SerializeField] Transform interactionPoint;
    [SerializeField] PlaceableSO objectData;
    [SerializeField] Transform lookAt;

    public Vector2 interacionTime;

    public void Interact(AICore core)
    {
        core.CustomerData.goodsCost += objectData.goodsCost;
    }

    public int ItemMinimalPrice()
    {
        return objectData.goodsCost;
    }
}
