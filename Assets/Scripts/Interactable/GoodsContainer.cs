using Interfaces;
using UnityEngine;

public class GoodsContainer : MonoBehaviour, IInteractable
{
    [SerializeField] Transform interactionPoint;

    public void Interact(AICore core)
    {
        throw new System.NotImplementedException();
    }
}
