using UnityEngine;

[CreateAssetMenu(fileName = "Placable")]
public class PlaceableSO : ScriptableObject
{
    [Header("Placing data")]
    public GameObject preview;
    public GameObject prefab;
    public Vector2Int extents;

    [Header("Object data")]
    public Sprite icon;
    public string description;
    public int price;
    public int goodsCost;



    public Vector2Int GetExtents(float rotation = 0)
    {
        return extents.Rotate(rotation).Abs();
    }

}
