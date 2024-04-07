using Interfaces;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] PlaceableSO objectToPlace;

    [SerializeField] LayerMask gridLayerMask;
    [SerializeField] LayerMask blockerLayerMask;
    [SerializeField] Material positive;
    [SerializeField] Material negative;

    private float placingRotation = 0f;
    private GameObject preview;


    private void Awake()
    {
        GameEvents.current.onSelectedPlacableObjectChange += ChangePlacableObject;
    }

    void Start()
    {
        CreatePreview();
    }



    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Build();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Remove();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            placingRotation = (placingRotation + 90f) % 360;
        }

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


    private void Build()
    {
        if (Utils.GetMouseWorldPositionRaycast(out Vector3 mousePos, blockerLayerMask, out Collider collider))
        {
            if (collider.TryGetComponent(out IClickable clickable))
            {
                clickable.OnClick();
            }
        }

        /*if (Utils.GetMouseWorldPositionRaycast(out Vector3 mousePos, gridLayerMask, out Collider gridCollider))
        {
            BuildingGrid grid = gridCollider.GetComponent<BuildingGrid>();
            grid.PlaceObject(objectToPlace, mousePos, placingRotation, out GameObject placedGO);
            if (placedGO.TryGetComponent(out GoodsContainer container))
            {
                GameEvents.current.GoodsContainerPlaced(container);
            }
        }*/
    }

    private void Remove()
    {
        if (Utils.GetMouseWorldPositionRaycast(out Vector3 mousePos, gridLayerMask, out Collider gridCollider))
        {
            BuildingGrid grid = gridCollider.GetComponent<BuildingGrid>();
            grid.RemoveObject(mousePos);
        }
    }
}
