using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using static BuildingGrid;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] PlaceableSO objectToPlace;

    [SerializeField] LayerMask gridLayerMask;
    [SerializeField] LayerMask blockerLayerMask;
    [SerializeField] Material positive;
    [SerializeField] Material negative;

    private float placingRotation = 0f;
    private GameObject preview;

    private bool showPreview = false;


    private void Awake()
    {
        GameEvents.current.onSelectedPlacableObjectChange += ChangePlacableObject;
        
        GameEvents.current.onBuilingModeEnter += EnterBuildingMode;
        GameEvents.current.onBuilingModeExit += ExitBuildingMode;
        
    }

    void Start()
    {
        CreatePreview();
        GameEvents.current.ExitBuildngMode();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            placingRotation = (placingRotation + 90f) % 360;
        }

        if (showPreview)
            ShowPreview();
        /*if (isInBuildingMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Build();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Remove();
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                placingRotation = (placingRotation + 90f) % 360;
            }

            ShowPreview();
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnLeftClick();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                OnRightClick();
            }
        }*/
    }

    private void ChangePlacableObject(PlaceableSO objectData)
    {
        objectToPlace = objectData;
        CreatePreview();
    }
    
    private void CreatePreview()
    {
        if (preview != null)
        {
            Destroy(preview);
        }

        preview = Instantiate(objectToPlace.preview, transform);
    }

    private void ShowPreview()
    {
        // if mouse is on grid
        if (Utils.GetMouseWorldPositionRaycast(out Vector3 mousePos, gridLayerMask | blockerLayerMask, out Collider gridCollider) && !blockerLayerMask.CheckLayer(gridCollider.gameObject.layer)) 
        {
            preview.SetActive(true);
            BuildingGrid grid = gridCollider.GetComponent<BuildingGrid>();
            preview.transform.position = grid.PreviewPosition(objectToPlace, mousePos, placingRotation);
            preview.transform.rotation = Quaternion.Euler(0, placingRotation, 0);

            Material previeMaterial = grid.CanBePlaced(objectToPlace, mousePos, placingRotation) ? positive : negative;
            foreach (var mesh in preview.GetComponentsInChildren<MeshRenderer>())
            {
                mesh.material = previeMaterial;
            }
        }
        else
            preview.SetActive(false);
    }

    private void EnterBuildingMode() 
    {
        showPreview = true;
        placingRotation = 0;
    }

    private void ExitBuildingMode()
    {
        showPreview = false;
        preview.SetActive(false);
    }


    public void Build(InputAction.CallbackContext ctx)
    {
        if (Utils.GetMouseWorldPositionRaycast(out Vector3 mousePos, gridLayerMask, out Collider gridCollider))
        {
            BuildingGrid grid = gridCollider.GetComponent<BuildingGrid>();
            if (grid.PlaceObject(objectToPlace, mousePos, placingRotation, out GameObject placedGO))
            {
                GoodsContainer container = placedGO.GetComponent<GoodsContainer>();
                Debug.Assert(container != null, "Placable object prefab does not contaion GoodsContainer script");
            }
        }
    }

    public void Remove(InputAction.CallbackContext ctx)
    {
        if (Utils.GetMouseWorldPositionRaycast(out Vector3 mousePos, gridLayerMask, out Collider gridCollider))
        {
            BuildingGrid grid = gridCollider.GetComponent<BuildingGrid>();
            if (!grid.CellIsEmpty(mousePos))
                GameEvents.current.PopupWindowCall("Are you sure you want to sell this object", () => grid.RemoveObject(mousePos));
        }
    }

    public void OnLeftClick(InputAction.CallbackContext ctx)
    {
        if (Utils.GetMouseWorldPositionRaycast(out Vector3 mousePos, blockerLayerMask, out Collider collider))
        {
            if (collider.TryGetComponent(out IClickable clickable))
            {
                clickable.OnClick();
            }
        }
    }

    public void OnRightClick(InputAction.CallbackContext ctx)
    {

    }
}
