using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.AI;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] PlaceableSO objectToPlace;

    [SerializeField] LayerMask gridLayerMask;
    [SerializeField] Material positive;
    [SerializeField] Material negative;

    private float placingRotation = 0f;
    private GameObject preview;

    void Start()
    {
        preview = Instantiate(objectToPlace.preview, transform);
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

        if (Utils.GetMouseWorldPositionRaycast(out Vector3 mousePos, gridLayerMask, out Collider gridCollider))
        {
            BuildingGrid grid = gridCollider.GetComponent<BuildingGrid>();
            preview.transform.position = grid.PreviewPosition(objectToPlace, mousePos, placingRotation);
            preview.transform.rotation = Quaternion.Euler(0, placingRotation, 0);

            Material previeMaterial = grid.CanBePlaced(objectToPlace, mousePos, placingRotation) ? positive : negative;
            foreach (var mesh in preview.GetComponentsInChildren<MeshRenderer>())
            {
                mesh.material = previeMaterial;
            }
        }

    }

    private void Build()
    {
        if (Utils.GetMouseWorldPositionRaycast(out Vector3 mousePos, gridLayerMask, out Collider gridCollider))
        {
            BuildingGrid grid = gridCollider.GetComponent<BuildingGrid>();
            grid.PlaceObject(objectToPlace, mousePos, placingRotation);
        }
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
