using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{
    [SerializeField] Vector2Int size;
    [SerializeField] float cellSize;
    [SerializeField] Vector3 origin;
    [SerializeField] PlaceableSO objectToPlace;
    
    [SerializeField] Material positive;
    [SerializeField] Material negative;

    private GridSystem<CellData> grid;
    private float placingRotation = 0f;

    private GameObject preview;





    private void Awake()
    {
        grid = new GridSystem<CellData>(size.x, size.y, cellSize, origin, (GridSystem<CellData> grid, int x, int y) => new CellData(grid, x, y) );
        preview = Instantiate(objectToPlace.prefab, transform);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (Utils.GetMouseWorldPositionRaycast(out Vector3 worldPos))
            {
                PlaceObject(objectToPlace, worldPos, placingRotation);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            placingRotation = (placingRotation + 90f) % 360;
        }

        if (Utils.GetMouseWorldPositionRaycast(out Vector3 mousePos))
        {
            Vector2Int cellPos = grid.GetXY(mousePos);
            Debug.Log(cellPos);
            Vector2Int rotationOffset = CalculateRotationOffset(objectToPlace.extents, placingRotation);
            preview.transform.position = grid.GetWorldPosition(cellPos + rotationOffset);
            preview.transform.rotation = Quaternion.Euler(0, placingRotation, 0);
            Material previeMaterial = IsObjectCanBePlaced(objectToPlace, cellPos, placingRotation) ? positive : negative;
            foreach (var mesh in preview.GetComponentsInChildren<MeshRenderer>())
            {
                mesh.material = previeMaterial;
            }
        }
    }

    /*private Vector2Int CalculateRotationOffset(Vector2Int extents, Quaternion rotation)
    {
        switch (rotation)
        {
            case Quaternion q when q == Quaternion.Euler(0, 0, 0):
                return Vector2Int.zero;
            case Quaternion q when q == Quaternion.Euler(0, 90, 0):
                return new Vector2Int(0, extents.x);
            case Quaternion q when q == Quaternion.Euler(0, 180, 0):
                return extents;
            case Quaternion q when q == Quaternion.Euler(0, 270, 0):
                return new Vector2Int(extents.y, 0);
            default:
                Debug.Log("DEFF");
                return Vector2Int.zero;
        }
    }*/

    private Vector2Int CalculateRotationOffset(Vector2Int extents, float rotation)
    {
        if (Mathf.Approximately(rotation, 0))
            return Vector2Int.zero;
        else if (Mathf.Approximately(rotation, 90))
            return new Vector2Int(0, extents.x);
        else if (Mathf.Approximately(rotation, 180))
            return extents;
        else if (Mathf.Approximately(rotation, 180))
            return new Vector2Int(extents.y, 0);
        else
            return Vector2Int.zero;
    }
    
    private bool IsObjectCanBePlaced(PlaceableSO obj, Vector2Int gridCoordinates, float rotation) 
    {
        Vector2Int extents = obj.extents.Rotate(rotation).Abs();
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

    private void PlaceObject(PlaceableSO obj, Vector3 clickWorldPos, float rotation)
    {
        Vector2Int cellPos = grid.GetXY(clickWorldPos);
        if (IsObjectCanBePlaced(obj, cellPos, rotation))
        {
            Vector2Int rotationOffset = CalculateRotationOffset(obj.extents, rotation);
            Transform placedObj = Instantiate(obj.prefab, grid.GetWorldPosition(cellPos + rotationOffset), Quaternion.Euler(0, rotation, 0)).transform;
            Vector2Int extents = obj.extents.Rotate(rotation).Abs();
            for (int y = 0; y < extents.y; y++)
            {
                for (int x = 0; x < extents.x; x++)
                {
                    CellData occupiedCell = grid.GetCell(cellPos + new Vector2Int(x, y));
                    occupiedCell.PlaceObject(placedObj);
                }
            }
            
        }
    }


    private void OnDrawGizmos()
    {
        if (grid == null) return;

        grid.DebugDraw();
        /* Gizmos.color = Color.black; 
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                Gizmos.DrawCube(grid.GetWorldPosition(x, y), Vector3.one * 0.1f);
            }
        }*/
    }

    public class CellData
    {
        private GridSystem<CellData> grid;
        private int x; 
        private int y;

        Transform placedObject = null;

        public CellData(GridSystem<CellData> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y; 
        }

        public void PlaceObject(Transform obj)
        {
            placedObject = obj;
            grid.TriggerGridObjectChanged(x, y);
        }

        public void RemoveObject()
        {
            placedObject = null;
            grid.TriggerGridObjectChanged(x, y);
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
