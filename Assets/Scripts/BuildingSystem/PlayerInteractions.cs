using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] PlaceableSO objectToPlace;

    [SerializeField] LayerMask gridLayerMask;
    [SerializeField] LayerMask blockerLayerMask;
    [SerializeField] LayerMask containersLayerMask;

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

    private void OnDestroy()
    {
        GameEvents.current.onSelectedPlacableObjectChange -= ChangePlacableObject;

        GameEvents.current.onBuilingModeEnter -= EnterBuildingMode;
        GameEvents.current.onBuilingModeExit -= ExitBuildingMode;
    }

    void Start()
    {
        CreatePreview();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            placingRotation = (placingRotation + 90f) % 360;
        }

        if (showPreview)
            ShowPreview();
    }

    private void ChangePlacableObject(PlaceableSO objectData)
    {
        Debug.Log(objectToPlace.name + " -> " + objectData.name);
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
        if (ShopManager.current.HaveEnoughMoney(objectToPlace.price) && 
            Utils.GetMouseWorldPositionRaycast(out Vector3 mousePos, gridLayerMask, out Collider gridCollider))
        {
            BuildingGrid grid = gridCollider.GetComponent<BuildingGrid>();
            Debug.Log("objectToPlace " + objectToPlace.name);
            if (grid.PlaceObject(objectToPlace, mousePos, placingRotation, out GameObject placedGO))
            {
                ShopManager.current.AddMoney(-objectToPlace.price);
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
                GameEvents.current.PopupWindowCall("Are you sure you want to sell this object", true, true, () => RemoveCall(mousePos, grid));
        }
    }

    private void RemoveCall(Vector3 mousePos, BuildingGrid grid)
    {
        ShopManager.current.AddMoney(objectToPlace.price);
        grid.RemoveObject(mousePos);
    }

    public void OnLeftClick(InputAction.CallbackContext ctx)
    {
        if (Utils.GetMouseWorldPositionRaycast(out Vector3 mousePos, blockerLayerMask | containersLayerMask, out Collider collider))
        {
            if (collider.TryGetComponent(out IClickable clickable))
            {
                clickable.OnLeftClick();
            }
        }
    }

    public void OnRightClick(InputAction.CallbackContext ctx)
    {

    }
}
