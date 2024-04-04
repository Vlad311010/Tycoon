using Interfaces;
using UnityEngine;

public class GoodsContainer : MonoBehaviour, IInteractable
{
    [SerializeField] Transform interactionPoint;
    [SerializeField] PlaceableSO objectData;

    [SerializeField] Vector2 interacionTime;

    public void Interact(AICore core)
    {
        Debug.Log("Buy");
    }
}
