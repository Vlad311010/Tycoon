using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    [SerializeField] Vector2Int size;
    [SerializeField] float cellSize;
    [SerializeField] Vector3 origin;

    private GridSystem<CellData> grid;
    private new BoxCollider collider;



    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        grid = new GridSystem<CellData>(size.x, size.y, cellSize, origin, (GridSystem<CellData> grid, int x, int y) => new CellData(grid, x, y));

        SetUpCollider();
    }

    private void SetUpCollider()
    {
        collider.center = new Vector3(origin.x + (size.x / 2f), origin.y, origin.z + (size.y / 2f));
        collider.size = new Vector3(size.x, 0f, size.y);
    }

    private Vector2Int CalculateRotationOffset(Vector2Int extents, float rotation)
    {
        if (Mathf.Approximately(rotation, 0))
            return Vector2Int.zero;
        else if (Mathf.Approximately(rotation, 90))
            return new Vector2Int(0, extents.x);
        else if (Mathf.Approximately(rotation, 180))
            return extents;
        else if (Mathf.Approximately(rotation, 270))
            return new Vector2Int(extents.y, 0);
        else
            return Vector2Int.zero;
    }


    public bool CanBePlaced(PlaceableSO obj, Vector3 worldPos, float rotation)
    {
        Vector2Int cellPos = grid.GetXY(worldPos);
        return IsObjectCanBePlaced(obj, cellPos, rotation);
    }

    private bool IsObjectCanBePlaced(PlaceableSO obj, Vector2Int gridCoordinates, float rotation) 
    {
        Vector2Int extents = obj.GetExtents(rotation);
        for (int y = 0; y < extents.y; y++)
        {
            for (int x = 0; x < extents.x; x++)
            {
                if (!grid.ValidateCoordinates(gridCoordinates + new Vector2Int(x, y))) return false;

                CellData cellToOccupy = grid.GetCell(gridCoordinates + new Vector2Int(x, y));
                if (!cellToOccupy.IsEmpty())
                    return false;
            }
        }

        return true;
    }

    public void PlaceObject(PlaceableSO obj, Vector3 clickWorldPos, float rotation)
    {
        Vector2Int cellPos = grid.GetXY(clickWorldPos);
        if (IsObjectCanBePlaced(obj, cellPos, rotation))
        {
            Vector2Int rotationOffset = CalculateRotationOffset(obj.extents, rotation);
            Transform placedObjectTransform = Instantiate(obj.prefab, grid.GetWorldPosition(cellPos + rotationOffset), Quaternion.Euler(0, rotation, 0)).transform;
            Vector2Int extents = obj.GetExtents(rotation);
            PlacedObject placedObject = new PlacedObject(placedObjectTransform, cellPos, rotation, obj);
            for (int y = 0; y < extents.y; y++)
            {
                for (int x = 0; x < extents.x; x++)
                {
                    CellData occupiedCell = grid.GetCell(cellPos + new Vector2Int(x, y));
                    occupiedCell.PlaceObject(placedObject);
                }
            }
            
        }
    }

    public void RemoveObject(Vector3 clickWorldPos)
    {
        Vector2Int cellPos = grid.GetXY(clickWorldPos);
        CellData targetCell = grid.GetCell(cellPos);
        if (targetCell.IsEmpty()) return;

        PlacedObject placedObject = targetCell.PlacedObject;
        Vector2Int objectOrigin = grid.GetCell(targetCell.GetPlacedObjectOrigin()).Coordinates;
        Vector2Int extents = placedObject.placeableSO.GetExtents(placedObject.rotation);
        
        Destroy(targetCell.PlacedGO);
        for (int y = 0; y < extents.y; y++)
        {
            for (int x = 0; x < extents.x; x++)
            {
                Debug.Log(objectOrigin + new Vector2Int(x, y));
                CellData occupiedCell = grid.GetCell(objectOrigin + new Vector2Int(x, y));
                occupiedCell.RemoveObject();
            }
        }
    }

    public Vector3 PreviewPosition(PlaceableSO obj, Vector3 worldPos, float rotation)
    {
        Vector2Int cellPos = grid.GetXY(worldPos);
        Vector2Int rotationOffset = CalculateRotationOffset(obj.extents, rotation);
        return grid.GetWorldPosition(cellPos + rotationOffset);
    }




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(origin.x + (size.x / 2f), origin.y, origin.z + (size.y / 2f)), new Vector3(size.x, 0f, size.y));
        
        if (grid == null) return;

        grid.DebugDraw();
    }

    public class CellData
    {
        public PlacedObject PlacedObject { get => placedObject.Value; }
        public GameObject PlacedGO { get => placedObject.Value.instanceRef.gameObject; }
        public Vector2Int Coordinates { get => new Vector2Int(x, y); }

        private GridSystem<CellData> grid;
        private int x; 
        private int y;

        PlacedObject? placedObject = null;

        public CellData(GridSystem<CellData> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y; 
        }

        public void PlaceObject(PlacedObject obj)
        {
            placedObject = obj;
            grid.TriggerCellhanged(x, y);
        }

        public void RemoveObject()
        {
            placedObject = null;
            grid.TriggerCellhanged(x, y);
        }

        public Vector2Int GetPlacedObjectOrigin()
        {
            if (!placedObject.HasValue) 
                return new Vector2Int(-1, -1);
            
            return placedObject.Value.origin;
        }

        public bool IsEmpty()
        {
            return placedObject == null;
        }

        public override string ToString()
        {
            return x + "." + y + ":" + placedObject?.ToString();
        }
    }

}