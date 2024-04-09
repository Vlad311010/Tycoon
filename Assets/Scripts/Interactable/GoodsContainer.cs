using Interfaces;
using UnityEngine;

public class GoodsContainer : MonoBehaviour, IInteractable, IClickable
{
    public Transform LookAt { get => lookAt; }
    public PlaceableSO ObjectData { get => objectData; }
    public BuildingGrid ConnectedGrid { get => connectedGrid; set => connectedGrid = value; }
    public Vector2Int GridCoordinates { get => gridCoordinates; set => gridCoordinates = value; }

    [SerializeField] PlaceableSO objectData;
    [SerializeField] Transform lookAt;
    [SerializeField] Transform interactionZoneAnchor;
    [SerializeField] Vector2 interactionZoneSize;

    public Vector2 interacionTime;

    private BuildingGrid connectedGrid;
    private Vector2Int gridCoordinates;

    public void Interact(AICore core)
    {
        core.CustomerData.OnInteraction(objectData);
    }

    public Vector3 RandomInteractionPoint()
    {
        Vector3 offset = new Vector3(Random.Range(-interactionZoneSize.x / 2, interactionZoneSize.x / 2), interactionZoneAnchor.position.y, Random.Range(-interactionZoneSize.y / 2, interactionZoneSize.y / 2));
        return interactionZoneAnchor.position + offset;
    }

    public int ItemMinimalPrice()
    {
        return objectData.goodsCost;
    }


    public void OnLeftClick()
    {
        throw new System.NotImplementedException();
    }

    public void OnRightClick()
    {
        throw new System.NotImplementedException();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 zoneSize = new Vector3(interactionZoneSize.x, 0, interactionZoneSize.y);
        Gizmos.DrawWireCube(interactionZoneAnchor.position, zoneSize);
    }
}
