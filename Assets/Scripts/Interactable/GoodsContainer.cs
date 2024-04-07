using Interfaces;
using UnityEngine;

public class GoodsContainer : MonoBehaviour, IInteractable
{
    public Transform LookAt { get => lookAt; }
    public PlaceableSO ObjectData { get => objectData; }
    public BuildingGrid ConnectedGrid { get => connectedGrid; set => connectedGrid = value; }
    public Vector2Int GridCoordinates { get => gridCoordinates; set => gridCoordinates = value; }

    [SerializeField] Transform interactionPoint;
    [SerializeField] PlaceableSO objectData;
    [SerializeField] Transform lookAt;

    public Vector2 interacionTime;

    private BuildingGrid connectedGrid;
    private Vector2Int gridCoordinates;

    public void Interact(AICore core)
    {
        core.CustomerData.goodsCost += objectData.goodsCost;
    }

    public int ItemMinimalPrice()
    {
        return objectData.goodsCost;
    }
}
