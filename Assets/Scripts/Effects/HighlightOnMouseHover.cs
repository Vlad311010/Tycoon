using System.Linq;
using UnityEngine;

public class HighlightOnMouseHover : MonoBehaviour
{
    MeshRenderer meshRenderer;

    [SerializeField] Material highlightMaterial;

    private Material[] defaultMaterials;
    private Material[] replaceMaterials;



    void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        defaultMaterials = meshRenderer.materials;

        replaceMaterials = Enumerable.Repeat(highlightMaterial, defaultMaterials.Length).ToArray();
    }

    public void OnMouseOver()
    {
        meshRenderer.materials = replaceMaterials;
    }

    public void OnMouseExit()
    {
        meshRenderer.materials = defaultMaterials;
    }

}
