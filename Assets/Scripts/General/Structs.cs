using System;
using UnityEngine;

public struct PlacedObject
{
    public Transform instanceRef;
    public Vector2Int origin;
    public float rotation;
    public PlaceableSO placeableSO;

    public PlacedObject(Transform instance, Vector2Int origin, float rotation, PlaceableSO so)
    {
        instanceRef = instance;
        this.origin = origin;
        this.rotation = rotation;
        this.placeableSO = so;
    }

    public override string ToString()
    {
        return placeableSO.name;
    }

}
